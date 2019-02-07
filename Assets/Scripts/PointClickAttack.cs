using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickAttack : SpeedControl
{
    [SerializeField] private GameObject attackPref;
    private float modelYOffset;
    private bool isometric;

    protected override void Awake()
    {
        base.Awake();
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        isometric = Managers.Managers.Settings.Isometric;
        modelYOffset = GetComponent<CharacterController>().height / 2f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    }

    private void OnIsometricEnabled(bool value) => isometric = value;

    private void Update()
    {
        if (speed > 0 && Input.GetMouseButtonDown(1) && isometric && !EventSystem.current.IsPointerOverGameObject())
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        yield return null;
        var go = Instantiate(attackPref);

        go.transform.position =
            new Vector3(transform.position.x, transform.position.y - modelYOffset, transform.position.z);
            
        go.transform.rotation = Quaternion.LookRotation(RelativeMovement.targetPos - go.transform.position) *
                                go.transform.rotation;
    }
}