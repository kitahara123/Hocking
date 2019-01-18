using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(PlayerManager), typeof(InventoryManager), typeof(ImagesManager))]
    [RequireComponent(typeof(AudioManager), typeof(SettingsManager), typeof(MissionManager))]
    [RequireComponent(typeof(DataManager))]
    public class Managers : MonoBehaviour
    {
        public static PlayerManager Player { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static ImagesManager Images { get; private set; }
        public static AudioManager Audio { get; private set; }
        public static SettingsManager Settings { get; private set; }
        public static MissionManager Mission { get; private set; }
        public static DataManager Data { get; private set; }

        private List<IGameManager> startSequence;

        private void Awake()
        {
            Player = GetComponent<PlayerManager>();
            Inventory = GetComponent<InventoryManager>();
            Images = GetComponent<ImagesManager>();
            Audio = GetComponent<AudioManager>();
            Settings = GetComponent<SettingsManager>();
            Mission = GetComponent<MissionManager>();
            Data = GetComponent<DataManager>();

            startSequence = new List<IGameManager> {Player, Inventory, Images, Audio, Settings, Mission, Data};
            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            NetworkService network = new NetworkService();
            foreach (var manager in startSequence)
            {
                manager.Startup(network);
            }

            yield return null;

            var numModules = startSequence.Count;
            var numReady = 0;

            while (numReady < numModules)
            {
                numReady = startSequence.Count(e => e.status == ManagerStatus.Started);
                Debug.Log($"Progress: {numReady}/{numModules}");
                Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
                yield return null;
            }

            Debug.Log("All managers started up");
            Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
        }
    }
}