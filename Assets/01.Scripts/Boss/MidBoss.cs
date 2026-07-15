using UnityEngine;

public class MidBoss : BossMonster
{
    [SerializeField] GameObject weakness;
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }

    public StateMachine<MidBoss> stateMachine;

    public MidBoss_IdleState idleState;
    public MidBoss_TraceState traceState;
    public MidBoss_DashState dashState;
    public MidBoss_GroggyState groggyState;

    private int wallLayerIndex;

    private float dashCoolTimer;
    public bool canDash;

    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        idleState = new MidBoss_IdleState();
        traceState = new MidBoss_TraceState();
        dashState = new MidBoss_DashState();
        groggyState = new MidBoss_GroggyState();

        stateMachine = new StateMachine<MidBoss>(this, idleState);

        wallLayerIndex = LayerMask.NameToLayer("Wall");

        dashCoolTimer = 2f;
        canDash = false;
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

    public void DashCoolTime()
    {
        dashCoolTimer = setting.dashCoolTime;
        canDash = false;
    }

    public void Move(float dirX)
    {
        rb.linearVelocity = new Vector2(dirX * setting.speed, rb.linearVelocity.y);
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    public void Dash(float dashDirX)
    {
        rb.linearVelocity = new Vector2(dashDirX * setting.dashSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stateMachine.CurrentState == dashState)
        {
            if(collision.gameObject.layer == wallLayerIndex)
            {
                Debug.Log("벽과 충돌해 그로기 상태");
                stateMachine.ChangeState(groggyState);
            }
        }
    }

    protected override void Die()
    {
        Debug.Log("중간보스 처치");
        Destroy(gameObject);
    }
}
