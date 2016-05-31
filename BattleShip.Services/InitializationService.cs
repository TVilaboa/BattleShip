using BattleShip.Data.DataBase;

namespace BattleShip.Services
{
    public class InitializationService
    {
        private static InitializationService instance;
        public static InitializationService GetInstance
        {
            get
            {
                return instance ?? (instance = new InitializationService());
            }
        }
        public  void Initialize()
        {
            InitializeRepository.GetInstance.InstanceContext();
            InitializeRepository.GetInstance.InitializeDatabase();
            
            InitializeRepository.GetInstance.DisposeContext();
        }
        public  void InstanceContext()
        {
            InitializeRepository.GetInstance.InstanceContext();
        }
        public  void DisposeContext()
        {
            InitializeRepository.GetInstance.DisposeContext();
        } 
    }
}