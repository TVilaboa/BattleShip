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
            ScrapperDataContext.InitializeDatabase();
        }
        public  void InstanceContext(bool hasLazyLoad = true)
        {

            if (ScrapperDataContext.GetInstance == null)
                System.Web.HttpContext.Current.Items["ScrapperDataContext"] = new ScrapperDataContext();
        }
        public  void DisposeContext()
        {

            if (ScrapperDataContext.GetInstance != null)
            {
                //((PiramideDbContext)System.Web.HttpContext.Current.Items["PiramideDbContext"]).Dispose();
                System.Web.HttpContext.Current.Items["ScrapperDataContext"] = null;
            }
        } 
    }
}