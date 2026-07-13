using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringWeapon : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float maxSpringDistance = 10f;
    [SerializeField] private LayerMask wallLayer;

    private Vector2 mousePos;
    private Vector2 mouseDir;
    private Vector2 hitPoint;
    private Vector2 anchorPoint;

    private bool isWallHit;
    private bool isAnchored;

    private Coroutine springTimerCoroutine;


    void Start()
    {
        
    }

    void Update()
    {
        if(isAnchored == false)
            AimCheck();

        if(isAnchored == false && Mouse.current.leftButton.wasPressedThisFrame && isWallHit)
        {
            isAnchored = true;
            anchorPoint = hitPoint;
            Debug.Log("태엽 2초간 고정");

            springTimerCoroutine = StartCoroutine(SpringTimer());
        }

        if (isAnchored && Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            StopCoroutine(springTimerCoroutine);
            isAnchored = false;
 


            player.Dash(anchorPoint);
        }
    }

    void AimCheck()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        mouseDir = (mousePos - (Vector2)transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDir, maxSpringDistance, wallLayer);

        isWallHit = hit.collider == null ? false : true;

        hitPoint = isWallHit == true ? hit.point : (Vector2)transform.position + (mouseDir * maxSpringDistance);
    }

    IEnumerator SpringTimer()
    {
        yield return new WaitForSeconds(2f);
        isAnchored = false;
        Debug.Log("2초가 지나 태엽 자동 회수");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isWallHit == true ? Color.green : Color.red;
        Vector2 gizmosPos = isAnchored ? anchorPoint : hitPoint;
        Gizmos.DrawLine(transform.position, gizmosPos);

        if (isWallHit)
        {
            Gizmos.DrawWireSphere(gizmosPos, 0.2f);
        }
    }
}
