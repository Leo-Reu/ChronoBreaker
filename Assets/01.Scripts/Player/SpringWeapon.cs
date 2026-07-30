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
    private Material lrMat;

    private CameraMove cam;

    private Animator playerAnim;

    [SerializeField] private Texture2D crosshair;
    private bool isCrosshairOn = false;

    private void Awake()
    {
        weaknessLayerIndex = LayerMask.NameToLayer("Weakness");
        windUp = GetComponentInParent<WindUp>();
        lr = GetComponent<LineRenderer>();
        if (player == null)
        {
            player = GetComponentInParent<PlayerController>();
        }
        lrMat = lr.material;
    }

    protected override void Start()
    {
        base.Start();

        if (Camera.main != null)
        {
            cam = Camera.main.GetComponent<CameraMove>();
        }
        playerAnim = player.GetComponent<Animator>();
    }


    void Update()
    {
        if (Time.timeScale == 0f)
        {
            if (isCrosshairOn)
            {
                UIManager.instance?.SetCursor();
                isCrosshairOn = false;
            }
            return;
        }

        if (!isCrosshairOn)
        {
            SetCrosshair();
        }

        if (windUp != null && windUp.isWindUp)
        {
            isAnchored = false;
            if (lr != null) lr.enabled = false;
            return;
        }

        if (lr != null)
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

        if (lr != null && lr.enabled)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = isAnchored ? anchorPoint : hitPoint;

            lr.positionCount = 2;
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);

            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;

            float distance = Vector2.Distance(startPos, endPos);
            if (lrMat != null)
            {
                lrMat.mainTextureScale = new Vector2(distance * 1.5f, 1f);
            }

            float alpha = isAnchored ? 1.0f : 0.4f;
            Color chainColor = new Color(1f, 1f, 1f, alpha);

            lr.startColor = chainColor;
            lr.endColor = chainColor;       
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            cam?.ZoomIn(true);
        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            cam?.ZoomIn(false);
        }
    }
    private void SetCrosshair()
    {
        if (crosshair != null)
        {
            Vector2 hotspot = new Vector2(crosshair.width / 2f, crosshair.height / 2f);
            Cursor.SetCursor(crosshair, hotspot, CursorMode.Auto);
            isCrosshairOn = true;
        }
    }


    protected override void Fire()
    {
        isAnchored = true;
        anchorPoint = hitPoint;
        isWeaknessAnchored = isWeaknessHit;

        if (playerAnim != null)
        {
            playerAnim.SetTrigger("TriggerAttack");
        }
        SoundManager.instance?.PlaySFX(SFXType.SpringFire);
        StartCoroutine(PlaySpringHitSound());

        Debug.Log($"태엽 {SpringDuration}초간 고정");

        springTimerCoroutine = StartCoroutine(SpringTimer());
    }
    private IEnumerator PlaySpringHitSound()
    {
        yield return new WaitForSeconds(0.06f);
        SoundManager.instance?.PlaySFX(SFXType.SpringHit);
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

    private void OnEnable()
    {
        isCrosshairOn = false;
    }
    private void OnDisable()
    {
        isCrosshairOn = false;
        UIManager.instance?.SetCursor();
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
