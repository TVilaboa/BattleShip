namespace BattleShip.Domain
{
    public interface IEntity
    {

        int Id { get; set; }

            string Code { get; set; }

        bool IsDeleted { get; set; }
         
    }
}