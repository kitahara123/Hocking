namespace Managers
{
        public interface IGameManager
        {
                ManagerStatus status { get; }

                void Startup();
        }
}