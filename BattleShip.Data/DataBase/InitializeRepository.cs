namespace BattleShip.Data.DataBase
{
    public class InitializeRepository
    {
        private static InitializeRepository instance;
        public static InitializeRepository GetInstance
        {
            get
            {
                return instance ?? (instance = new InitializeRepository());
            }
        }
        public  void InitializeDatabase()
        {
            BattleShipDataContext.InitializeDatabase();
        }
        public  void InstanceContext(bool hasLazyLoad = true)
        {

            if (BattleShipDataContext.GetInstance == null)
                System.Web.HttpContext.Current.Items["BattleShipDataContext"] = new BattleShipDataContext();
        }
        public  void DisposeContext()
        {

            if (BattleShipDataContext.GetInstance != null)
            {
          
                System.Web.HttpContext.Current.Items["BattleShipDataContext"] = null;
            }
        } 
    }
}