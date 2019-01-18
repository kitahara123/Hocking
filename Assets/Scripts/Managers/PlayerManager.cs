using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private float invulState = 1f; 
        public ManagerStatus status { get; private set; }

        public int health { get; private set; }
        public int maxHealth { get; private set; }

        private float lastHitTime;
        public void Startup(NetworkService service)
        {
            Debug.Log("Player manager starting...");
            health = 50;
            maxHealth = 100;

            status = ManagerStatus.Started;
        }

        public void ChangeHealth(int value)
        {
            health += value;
            if (health > maxHealth)
                health = maxHealth;
            else if (health < 0)
            {
                health = 0;
            }

            Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
            if (health == 0) Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        public void Respawn()
        {
            health = 50;
            maxHealth = 100;
        }

        public void Hurt(int damage)
        {
            if (Time.time < lastHitTime + invulState) return;
            lastHitTime = Time.time;
            ChangeHealth(-damage);
        }
    }
}