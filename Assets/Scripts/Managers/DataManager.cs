using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class DataManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private string fileName = "game.dat";
        private string filePath;

        public ManagerStatus status { get; private set; }

        public void Startup(NetworkService service)
        {
            Debug.Log("Data manager starting...");

            filePath = Path.Combine(Application.persistentDataPath, fileName);

            status = ManagerStatus.Started;
        }

        public void SaveGameState()
        {
            Dictionary<string, object> gameState = new Dictionary<string, object>();
            gameState.Add("inventory", Managers.Player.Player.Inventory.GetData());
            gameState.Add("HP", Managers.Player.Player.HP);
            gameState.Add("maxHP", Managers.Player.Player.maxHP);
            gameState.Add("curLevel", Managers.Mission.curLevel);
            gameState.Add("maxLevel", Managers.Mission.maxLevel);
            gameState.Add("positionX", Managers.Player.Player.transform.position.x);
            gameState.Add("positionY", Managers.Player.Player.transform.position.y);
            gameState.Add("positionZ", Managers.Player.Player.transform.position.z);

            var stream = File.Create(filePath);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, gameState);
            stream.Close();
        }

        public IEnumerator LoadGameState()
        {
            if (!File.Exists(filePath))
            {
                Debug.Log("No saved game");
                yield break;
            }

            Dictionary<string, object> gameState;
            BinaryFormatter formatter = new BinaryFormatter();
            var stream = File.Open(filePath, FileMode.Open);
            gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();

            Managers.Mission.RestartCurrent();

            yield return new WaitUntil(() => Managers.Mission.SceneLoaded);

            Managers.Player.Player.Inventory.UpdateData(
                gameState["inventory"] as Dictionary<string, List<InventoryItem>>);

            Managers.Player.Player.UpdateData((int) gameState["HP"], (int) gameState["maxHP"]);

            Managers.Player.Player.transform.position = new Vector3((float) gameState["positionX"],
                (float) gameState["positionY"], (float) gameState["positionZ"]);

            Managers.Mission.UpdateData((int) gameState["curLevel"], (int) gameState["maxLevel"]);
        }
    }
}