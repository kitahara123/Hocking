using UnityEngine;

public class BaseDevice : MonoBehaviour
{
        private void OnMouseDown()
        {
                Operate();
        }

        public virtual void Operate()
        {
                
        }
}