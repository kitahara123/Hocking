using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackPref;
    private bool isometric;

    private void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        isometric = Managers.Managers.Settings.Isometric;
    }

    private void OnDestroy() => Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    private void OnIsometricEnabled(bool value) => isometric = value;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isometric && !EventSystem.current.IsPointerOverGameObject())
        {
            var go = Instantiate(attackPref);
            
            // Минус по высоте чтобы анимация начиналась в ногах персонажа
            go.transform.position = new Vector3(transform.position.x, transform.position.y - 1.1f, transform.position.z); 
            go.transform.rotation = Quaternion.LookRotation(transform.forward) * go.transform.rotation;

            Destroy(go.gameObject, 1f);
        }

    }

}