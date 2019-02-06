using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackPref;
    private float modelYOffset;
    private bool isometric;

    private void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        isometric = Managers.Managers.Settings.Isometric;
        modelYOffset = GetComponent<CharacterController>().height / 2f;
    }

    private void OnDestroy() => Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    private void OnIsometricEnabled(bool value) => isometric = value;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isometric && !EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        yield return null;
        var go = Instantiate(attackPref);

        Debug.Log(RelativeMovement.targetPos);
        go.transform.position =
            new Vector3(transform.position.x, transform.position.y - modelYOffset, transform.position.z);
            
        go.transform.rotation = Quaternion.LookRotation(RelativeMovement.targetPos - go.transform.position) *
                                go.transform.rotation;
            

        Destroy(go.gameObject, 1f);
        
    }
}