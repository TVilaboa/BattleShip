using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BattleShip.Data;
using BattleShip.Domain;
using Newtonsoft.Json.Linq;

namespace BattleShip.Services
{
    public class ScrappedDocumentService
    {
        public ScrappedDocumentService()
        {
            UnitOfWork = new UnitOfWork();

        }

        UnitOfWork UnitOfWork { get; set; }

        public IList<ScrappedDocument> All()
        {
            return UnitOfWork.ScrappedDocuments.All().ToList();
        }

        public IList<ScrappedDocument> FindAll(Expression<Func<ScrappedDocument, bool>> match)
        {
            return UnitOfWork.ScrappedDocuments.FindAll(match).ToList();
        }

        public ScrappedDocument Get(int id)
        {
            return UnitOfWork.ScrappedDocuments.GetById(id);
        }

        public ScrappedDocument AddOrUpdate(ScrappedDocument scrappedDocument)
        {
            return UnitOfWork.ScrappedDocuments.AddOrUpdate(scrappedDocument, true, true);
        }

        public ScrappedDocument GetByCode(string code)
        {
            return UnitOfWork.ScrappedDocuments.Find(s => s.Code == code);
        }

        public string GetNameForDocument(JObject obj,Schema schema)
        {
            if (obj.HasValues)
            {
                if (!string.IsNullOrWhiteSpace(schema.SecondPropertyForTitle))
                {
                    return obj["title"].ToString() + obj[schema.SecondPropertyForTitle].ToString();
                }
                return obj["title"].ToString();
            }
            else
            {
                return "Empty";
            }
        }

        public ScrappedDocument Update(ScrappedDocument doc)
        {
            return UnitOfWork.ScrappedDocuments.Update(doc, false, true);
        }

        public ScrappedDocument Find(Expression<Func<ScrappedDocument, bool>> expression)
        {
            return UnitOfWork.ScrappedDocuments.Find(expression);
        }
    }
}