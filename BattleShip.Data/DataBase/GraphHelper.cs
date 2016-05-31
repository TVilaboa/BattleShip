using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace BattleShip.Data.DataBase
{
    public class GraphHelper
    {
        private DbContext context;
        public GraphHelper(DbContext context)
        {
            this.context = context;
        }
        public bool UpdateGraph(object updated)
        {
            if (updated != null)
            {
                bool isNew = true;

                object current = this.context.Set(updated.GetType()).Find(updated.GetType().GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute))).GetValue(updated));
                if (current != null)
                {
                    this.context.Entry(current).State = EntityState.Modified;
                    isNew = false;
                }
                else
                    current = updated;

                foreach (var item in current.GetType().GetProperties())
                {
                    Type t = item.PropertyType;
                    bool IsPrimitive = (t.IsPrimitive || t == typeof(Decimal) || t == typeof(String) || t == typeof(DateTime) || t == typeof(Decimal?) || t == typeof(DateTime?));
                    if (!isNew && IsPrimitive && item.CustomAttributes.All(attr => attr.AttributeType != typeof (KeyAttribute)))
                        item.SetValue(current, updated.GetType().GetProperty(item.Name).GetValue(updated));
                    else if (!IsPrimitive)
                    {
                        if (item.GetValue(updated) != null)
                        {
                            if ((t.IsGenericType && t.GetGenericTypeDefinition().Name.Contains("List")) || t.IsArray)
                                foreach (var i in (IList)item.GetValue(updated))
                                {
                                    if (i != null)
                                    {
                                        if (i.GetType().GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute))).GetValue(i) != null)
                                            this.context.Entry(i).State = EntityState.Modified;
                                        UpdateGraph(i);
                                    }
                                }
                            else
                            {
                                if (item.GetValue(updated).GetType().GetProperties().FirstOrDefault(p => p.CustomAttributes.Any(attr => attr.AttributeType == typeof(KeyAttribute))).GetValue(updated) != null)
                                    this.context.Entry(item.GetValue(updated)).State = EntityState.Modified;
                                UpdateGraph(item.GetValue(updated));
                            }
                        }
                    }
                }

                return isNew;
            }
            return false;
        }
    }
}
