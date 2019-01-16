using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }

        public int health { get; private set; }
        public int maxHealth { get; private set; }

        public void Startup(NetworkService service)
        {
            Debug.Log("Player manager starting...");
            health = 50;
            maxHealth = 100;

            status = ManagerStatus.Started;
        }

        public void AddHealth(int value)
        {
            health += value;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health < 0)
            {
                health = 0;
            }
            
            Debug.Log("Health: " + health + "/" + maxHealth);
        }
        
        public void Hurt(int damage)
        {
            health -= damage;
            Debug.Log("hp " + health);
        }
    }
}