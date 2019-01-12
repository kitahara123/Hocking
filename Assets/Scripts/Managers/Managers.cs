using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(PlayerManager), typeof(InventoryManager), typeof(ImagesManager))]
    public class Managers : MonoBehaviour
    {
        public static PlayerManager Player { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static ImagesManager Images { get; private set; }

        private List<IGameManager> startSequence;

        private void Awake()
        {
            Player = GetComponent<PlayerManager>();
            Inventory = GetComponent<InventoryManager>();
            Images = GetComponent<ImagesManager>();

            startSequence = new List<IGameManager> {Player, Inventory, Images};
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
                var lastReady = numReady;
                numReady = 0;

                numReady = startSequence.Count(e => e.status == ManagerStatus.Started);
                if (numReady > lastReady)
                    Debug.Log("Progress: " + numReady + "/" + numModules);
                yield return null;
            }

            Debug.Log("All managers started up");
        }
    }
}