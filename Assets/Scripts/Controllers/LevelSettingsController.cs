using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Помещается в каждую сцену для задания уникальных настроек
    /// </summary>
    public class LevelSettingsController : MonoBehaviour
    {
        [SerializeField] private bool isometric;
        
        private void Awake()
        {
            Managers.Managers.Settings.IsometricToggle(isometric);
        }
    }
}