using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    [Serializable]
    public class SpawnableObject
    {
        [SerializeField] private Creature prefab;
        [SerializeField] private int quantity;
        public int Quantity => quantity;

        private MonoObjectsPool<Creature> pool;
        private MonoObjectsPool<Creature> Pool => pool ?? (pool = new MonoObjectsPool<Creature>(prefab));

        private HashSet<Creature> activeInstances;
        private HashSet<Creature> ActiveInstances => activeInstances ?? (activeInstances = new HashSet<Creature>());

        public int CountActiveInstances => ActiveInstances.Count;
        
        public Creature AddInstance()
        {
            var instance = Pool.CreateInstance();
            instance.Reset();
            ActiveInstances.Add(instance);
            instance.OnDeath += OnDeath;
            return instance;
        }

        private void OnDeath(Creature instance)
        {
            Pool.RemoveInstance(instance);
            ActiveInstances.Remove(instance);
            Messenger.Broadcast(GameEvent.SCORE_EARNED);
            instance.OnDeath -= OnDeath;
        }


    }
}