using UnityEngine;

public class MidBoss : BossMonster
{
    [SerializeField] GameObject weakness;
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }
    public float CurrentSpeed => isPhaseTwo ? setting.speed * 1.3f : setting.speed;
    public float CurrentDashSpeed => isPhaseTwo ? setting.dashSpeed * 1.3f : setting.dashSpeed;

    public StateMachine<MidBoss> stateMachine;

    public MidBoss_IdleState idleState;
    public MidBoss_TraceState traceState;
    public MidBoss_DashState dashState;
    public MidBoss_GroggyState groggyState;

    private float dashCoolTimer;
    public bool canDash;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CameraMove cam;

    private Vector3 localScale;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        idleState = new MidBoss_IdleState();
        traceState = new MidBoss_TraceState();
        dashState = new MidBoss_DashState();
        groggyState = new MidBoss_GroggyState();

        stateMachine = new StateMachine<MidBoss>(this, idleState);

        dashCoolTimer = 2f;
        canDash = false;

        localScale = transform.localScale;

        cam = Camera.main.GetComponent<CameraMove>();
    }

    public void Update()
    {
        stateMachine?.Update();

        if(dashCoolTimer > 0f)
        {
            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer <= 0f)
            {
                canDash = true;
            }
        }
    }

    public override BossType BossType { get { return BossType.MidBoss; } }

    public void DashCoolTime()
    {
        dashCoolTimer = setting.dashCoolTime;
        canDash = false;
    }

    bool CheckFlip()
    {
        return transform.position.x > playerTransform.position.x ? true : false;
    }

    public void Move(float dirX)
    {
        rb.linearVelocity = new Vector2(dirX * CurrentSpeed, rb.linearVelocity.y);

        if (CheckFlip())
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void Dash(float dashDirX)
    {
        rb.linearVelocity = new Vector2(dashDirX * CurrentDashSpeed, rb.linearVelocity.y);
        
        if (CheckFlip())
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
    }

    protected override void EnterPhaseTwo()
    {
        base.EnterPhaseTwo();
        sr.color = new Color(1f, 0.3f, 0.3f);
        cam?.ShakeCamera(0.6f, 0.7f);
    }

    protected override void Die()
    {
        Debug.Log("중간보스 처치");
        if (GameManager.instance != null)
        {
            GameManager.instance.BossDead(this);
        }
        Destroy(gameObject);
    }
}
