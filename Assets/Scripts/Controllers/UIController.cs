using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private bool scoreLabelOn;
        [SerializeField] private bool healthLabelOn;
        [SerializeField] private bool settingsOn;
        [SerializeField] private bool inventoryOn;

        private void Start()
        {
            Managers.Managers.UI.scoreLabelOn = scoreLabelOn;
            Managers.Managers.UI.healthLabelOn = healthLabelOn;
            Managers.Managers.UI.settingsOn = settingsOn;
            Managers.Managers.UI.inventoryOn = inventoryOn;
            Managers.Managers.UI.RefreshUI();
        }
    }
}