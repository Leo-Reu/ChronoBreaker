using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringWeapon : Weapon
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float maxSpringDistance = 10f;
    [SerializeField] private float SpringDuration = 2f;
    [SerializeField] private LayerMask targetLayer;

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

    private WindUp windUp;

    private LineRenderer lr;    // 조준선

    protected override void Start()
    {
        base.Start();
        weaknessLayerIndex = LayerMask.NameToLayer("Weakness");

        windUp = GetComponentInParent<WindUp>();
        lr = GetComponent<LineRenderer>();
    }


    void Update()
    {
        if (windUp.isWindUp)
        {
            isAnchored = false;
            lr.enabled = false;
            return;
        }

        if(lr != null)
        {
            if(isAnchored || isTargetHit)
            {
                lr.enabled = true;
            }
            else
            {
                lr.enabled = false;
            }
        }

        if(isAnchored == false)
        {
            LookMouse();
            AimCheck();
        }
            

        if(isAnchored == false && Mouse.current.leftButton.wasPressedThisFrame && isTargetHit)
        {
            if (player.CanDashCheck() == true)
            {
                Fire();
            }
        }

        if (isAnchored && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            StopCoroutine(springTimerCoroutine);
            isAnchored = false;
 
            player.Dash(anchorPoint, isWeaknessAnchored);
        }

        if (lr.enabled)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, isAnchored ? anchorPoint : hitPoint);
            Color color;
            if (isAnchored)
            {
                color = isWeaknessAnchored == true ? Color.yellow : Color.green;
            }

            else
            {
                color = isTargetHit ? (isWeaknessHit ? Color.yellow : Color.green) : Color.red;
            }
            lr.startColor = color;
            lr.endColor = color;
        }

    }

    protected override void Fire()
    {
        isAnchored = true;
        anchorPoint = hitPoint;
        isWeaknessAnchored = isWeaknessHit;

        Debug.Log($"태엽 {SpringDuration}초간 고정");

        springTimerCoroutine = StartCoroutine(SpringTimer());
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
        yield return new WaitForSeconds(SpringDuration);
        isAnchored = false;
        Debug.Log($"{SpringDuration}초가 지나 태엽 자동 회수");
    }

    //private void OnDrawGizmos()
    //{
    //    if (isAnchored) 
    //    {
    //        Gizmos.color = isWeaknessAnchored == true ? Color.yellow : Color.green;
    //    }

    //    else 
    //    {
    //        Gizmos.color = isTargetHit ? (isWeaknessHit ? Color.yellow : Color.green) : Color.red;
    //    }

    //    Vector2 gizmosPos = isAnchored ? anchorPoint : hitPoint;
    //    Gizmos.DrawLine(transform.position, gizmosPos);

    //    if (isTargetHit)
    //    {
    //        Gizmos.DrawWireSphere(gizmosPos, 0.2f);
    //    }
    //}
}
