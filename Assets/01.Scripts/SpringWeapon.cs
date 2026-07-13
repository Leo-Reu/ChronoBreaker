using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringWeapon : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float maxSpringDistance = 10f;
    [SerializeField] private LayerMask targetLayer;

    private Camera camera;

    private Vector2 mousePos;
    private Vector2 mouseDir;
    private Vector2 hitPoint;
    private Vector2 anchorPoint;

    private bool isTargetHit;
    private bool isAnchored;

    private bool isWeaknessHit;
    private bool isWeaknessAnchored;

    private Coroutine springTimerCoroutine;

    private int weaknessLayerIndex;

    private void Start()
    {
        camera = Camera.main;
        weaknessLayerIndex = LayerMask.NameToLayer("Weakness");
    }


    void Update()
    {
        if (player.CanDashCheck() == false && isAnchored == false)  // 대시 불가능일 때 조준 불가
        {
            isTargetHit = false;
            return;
        }

        if(isAnchored == false)
            AimCheck();

        if(isAnchored == false && Mouse.current.leftButton.wasPressedThisFrame && isTargetHit)
        {
            isAnchored = true;
            anchorPoint = hitPoint;
            isWeaknessAnchored = isWeaknessHit;

            Debug.Log("태엽 2초간 고정");

            springTimerCoroutine = StartCoroutine(SpringTimer());
        }

        if (isAnchored && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            StopCoroutine(springTimerCoroutine);
            isAnchored = false;
 


            player.Dash(anchorPoint, isWeaknessAnchored);
        }
    }

    void AimCheck()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        mousePos = camera.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = (mousePos - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDir, maxSpringDistance, targetLayer);

        isTargetHit = hit.collider == null ? false : true;

        if (isTargetHit)
        {
            hitPoint = hit.point;
            isWeaknessHit = (hit.collider.gameObject.layer == weaknessLayerIndex);
        }
        else
        {
            hitPoint = (Vector2)transform.position + (mouseDir * maxSpringDistance);
            isWeaknessHit = false;
        }
    }

    IEnumerator SpringTimer()
    {
        yield return new WaitForSeconds(2f);
        isAnchored = false;
        Debug.Log("2초가 지나 태엽 자동 회수");
    }

    private void OnDrawGizmos()
    {
        if (isAnchored) 
        {
            Gizmos.color = isWeaknessAnchored == true ? Color.yellow : Color.green;
        }
        else 
        {
            Gizmos.color = isTargetHit ? (isWeaknessHit ? Color.yellow : Color.green) : Color.red;
        }
        Vector2 gizmosPos = isAnchored ? anchorPoint : hitPoint;
        Gizmos.DrawLine(transform.position, gizmosPos);

        if (isTargetHit)
        {
            Gizmos.DrawWireSphere(gizmosPos, 0.2f);
        }
    }
}
