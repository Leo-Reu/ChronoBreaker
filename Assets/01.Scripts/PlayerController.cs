using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 4f;    ScriptableObject로 구현
    //[SerializeField] private float jumpPower = 5f;
    //[SerializeField] private float dashSpeed = 15f;
    //[SerializeField] private int playerDamage = 10;

    [SerializeField] private BossMonster boss;

    private PlayerSetting setting;

    private float dir;
    private bool isGround;
    private bool isDash;
    private bool isBound;
    private bool isAttack;

    private int jumpCount;
    private int jumpCountMax;

    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask groundLayer;

    //[SerializeField] private float dashCoolTime = 1f;
    private float dashCoolTimeTimer = 0f;
    private bool canDash = true;

    private int weaknessLayerIndex;

    private WindUp windUp;

<<<<<<< Updated upstream:Assets/01.Scripts/PlayerController.cs
=======
    private Vector3 localScale;

    private Coroutine dashTimeout;

>>>>>>> Stashed changes:Assets/01.Scripts/Player/PlayerController.cs
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        windUp = GetComponent<WindUp>();
    }

    void Start()
    {
        setting = DataManager.instance.PlayerSetting;
        jumpCount = 0;
        jumpCountMax = 2;

        weaknessLayerIndex = LayerMask.NameToLayer("Weakness");

        StartCoroutine(DashCoolTime());
    }

    void Update()
    {
        if(windUp.isWindUp) // WindUp중 행동할수 없게
        {
            dir = 0;
            return;
        }

        dir = 0;

        if (Keyboard.current.aKey.isPressed)
        {
            dir += -1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            dir += 1;
        }

        GroundCheck();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isDash && isAttack == false)
            {
                DashJump();
            }
            else if(isDash == false)
            {
                jump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (windUp.isWindUp)    // WindUp중 행동할수 없게
        {
            return;
        }

        if(isDash == false && isBound == false)
        {
            Move();
        }
    }

    void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.3f, Vector2.down, 0.8f, groundLayer);

        isGround = hit.collider == null ? false : true;
        if(isGround && rb.linearVelocity.y <= 0.01f)
        {
            jumpCount = 0;
        }
    }

    public bool CanDashCheck() 
    {
        return canDash == true && isDash == false && windUp.isWindUp == false;
    }


    void Move()
    {
        rb.linearVelocity = new Vector2(dir * setting.moveSpeed, rb.linearVelocity.y);
    }

    void jump()
    {
        if (jumpCount >= jumpCountMax)
        {
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, setting.jumpPower);

        jumpCount++;
        Debug.Log("점프");
    }

    public void Dash(Vector2 targetPos, bool isAttack)
    {
        this.isAttack = isAttack;
        Vector2 dashDir = (targetPos - (Vector2)transform.position).normalized;
        isDash = true;

        rb.gravityScale = 0f;
        rb.linearVelocity = dashDir * setting.dashSpeed;
        Debug.Log("대쉬 시작");

        if(dashTimeout != null)
        {
            StopCoroutine(dashTimeout);
        }
        dashTimeout = StartCoroutine(DashTimeout(targetPos, dashDir));
    }

    private IEnumerator DashTimeout(Vector2 targetPos, Vector2 dashDir)
    {
        float distance = Vector2.Distance(transform.position, targetPos);
        float maxDuration = (distance / setting.dashSpeed) + 0.2f;

        yield return new WaitForSeconds(maxDuration);

        if (isDash)
        {
            Vector2 reboundDir = new Vector2(-dashDir.x, setting.upForce).normalized;
            StopDash(reboundDir);
        }
    }

    void DashJump()
    {
        isDash = false;
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.1f, setting.jumpPower);
        jumpCount = 1; // 대쉬 중 점프 후 한번 더 점프할 수 있도록
        Debug.Log("대쉬 중 점프");
    }

    void StopDash(Vector2 reboundPower)
    {
        if (dashTimeout != null)
        {
            StopCoroutine(dashTimeout);
        }

        isDash = false;
        isAttack = false;
        rb.gravityScale = 1f;

        canDash = false;

        float power = 3f;
        rb.linearVelocity = reboundPower * power;
        StartCoroutine(Rebounding());   // 벽면 x축 반동을 위해 Move()함수 0.1초 억제

        Debug.Log("대쉬 종료");
    }

    IEnumerator Rebounding()
    {
        isBound = true;
        yield return new WaitForSeconds(0.1f);
        isBound = false;
    }

    IEnumerator DashCoolTime()
    {
        while (true)
        {
            yield return new WaitWhile(() => canDash);

            dashCoolTimeTimer = setting.dashCoolTime;

            while (dashCoolTimeTimer > 0f)
            {
                dashCoolTimeTimer -= Time.deltaTime;
                yield return null;
            }
            dashCoolTimeTimer = 0f;
            canDash = true;

            Debug.Log("대쉬 쿨타임 끝");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDash)
        {
            if(collision.gameObject.layer == weaknessLayerIndex && boss != null)
            {
<<<<<<< Updated upstream:Assets/01.Scripts/PlayerController.cs
                boss.TakeDamage(setting.playerDamage);
=======
                Debug.Log("약점과 충돌");
                BossMonster boss = collision.gameObject.GetComponentInParent<BossMonster>();
                if (boss != null)
                {
                    Debug.Log("보스를 공격");

                    boss.TakeDamage(setting.playerDamage);
                }
                Vector2 normalDir = collision.contacts[0].normal;
                Vector2 reboundDir = new Vector2(normalDir.x, setting.upForce).normalized;  // 튕겨나갈 방향
                StopDash(reboundDir);
                return;
            }
            if (layerName == "Ground" || layerName == "Wall" || layerName == "Monster")
            {
                Vector2 normalDir = collision.contacts[0].normal;
                Vector2 reboundDir = new Vector2(normalDir.x, setting.upForce).normalized;
                StopDash(reboundDir);
                return;
            }
        }
        else
        {
            BossMonster boss = collision.gameObject.GetComponent<BossMonster>();
            if (boss == null)
            {
                boss = collision.gameObject.GetComponentInParent<BossMonster>();  
            }
            if (boss != null)
            {
                TakeDamage(boss.Setting.bossDamage);
>>>>>>> Stashed changes:Assets/01.Scripts/Player/PlayerController.cs
            }
            Vector2 reboundPower = collision.contacts[0].normal;
            StopDash(reboundPower);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.8f, 0), 0.3f);
    }
}
