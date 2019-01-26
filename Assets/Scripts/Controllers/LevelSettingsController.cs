using UnityEngine;

namespace Controllers
{
    public class LevelSettingsController : MonoBehaviour
    {
        [SerializeField] private bool isometric;
        
        private void Awake()
        {
            Managers.Managers.Settings.IsometricToggle(isometric);
        }
    }
}