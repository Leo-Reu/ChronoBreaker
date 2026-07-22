using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 4f;    ScriptableObject로 구현
    //[SerializeField] private float jumpPower = 5f;
    //[SerializeField] private float dashSpeed = 15f;
    //[SerializeField] private int playerDamage = 10;

    [SerializeField] private PlayerSetting setting;

    private float dir;
    private bool isGround;
    private bool isDash;
    private bool isBound;
    private bool isAttack;
    private bool isInvincible;
    private bool isDead = false;

    [SerializeField] private float hp;

    private int jumpCount;
    private int jumpCountMax;

    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask groundLayer;

    //[SerializeField] private float dashCoolTime = 1f;
    private float dashCoolTimeTimer = 0f;
    private bool canDash = true;

    private Vector2 dashDir;
    private Vector2 currentDashDir;

    private int weaknessLayerIndex;

    private WindUp windUp;

    private Vector3 localScale;

    private Coroutine dashTimeout;

    private CameraMove cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        windUp = GetComponent<WindUp>();
    }

    void Start()
    {
        cam = Camera.main.GetComponent<CameraMove>();
        setting = DataManager.instance.PlayerSetting;
        jumpCount = 0;
        jumpCountMax = 2;
        hp = setting.maxHp;

        weaknessLayerIndex = LayerMask.NameToLayer("Weakness");

        localScale = transform.localScale;

        StartCoroutine(DashCoolTime());
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

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
        if (isDead)
        {
            return;
        }

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

        if(hit.collider != null && rb.linearVelocity.y <= 0.05f)
        {
            isGround = true;
            jumpCount = 0;
        }
        else
        {
            isGround = false;
        }
    }

    public bool CanDashCheck() 
    {
        return canDash == true && isDash == false && windUp.isWindUp == false;
    }


    void Move()
    {
        float moveX = dir * setting.moveSpeed;
        if (isGround)
        {
            rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);
        }
        else
        {
            if (dir != 0f)
            {
                rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);
            }
            else
            {
                float boundX = Mathf.MoveTowards(rb.linearVelocity.x, 0f, 4f * Time.fixedDeltaTime);
                rb.linearVelocity = new Vector2(boundX, rb.linearVelocity.y);
            }
        }
        if (dir != 0f)
        {
            transform.localScale = new Vector3(dir * localScale.x, localScale.y, localScale.z); 
        }
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
        dashDir = (targetPos - (Vector2)transform.position).normalized;
        currentDashDir = dashDir;
        isDash = true;

        jumpCount = 0;

        rb.gravityScale = 0f;
        rb.linearVelocity = dashDir * setting.dashSpeed;
        Debug.Log("대쉬 시작");

        if (dashTimeout != null)
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
            Vector2 reboundDir = new Vector2(-dashDir.x, setting.upForce).normalized;  // 튕겨나갈 방향
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

    void StopDash(Vector2 reboundDir)
    {
        if (dashTimeout != null)
        {
            StopCoroutine(dashTimeout);
        }

        isDash = false;
        isAttack = false;
        rb.gravityScale = 1f;

        canDash = false;

        
        rb.linearVelocity = reboundDir * setting.reboundPower;
        StartCoroutine(Rebounding());   // 벽면 반동을 위해 Move()함수 0.1초 억제

        Debug.Log("대쉬 종료");
    }

    IEnumerator Rebounding()
    {
        isBound = true;
        yield return new WaitForSeconds(setting.reboundTime);
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
    IEnumerator PlayerHitCoolTime()
    {
        isInvincible = true;

        yield return new WaitForSeconds(setting.hitCoolTime);

        isInvincible = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        int hitLayer = collision.collider.gameObject.layer;
        string layerName = LayerMask.LayerToName(hitLayer);

        if (isDash)
        {
            Vector2 normalDir = collision.contacts[0].normal;
            Vector2 reboundDir;
            if(normalDir.y < -0.5f)
            {
                float dashDirX = 0f;
                if(currentDashDir.x > 0f)
                {
                    dashDirX = 1f;
                }
                else if(currentDashDir.x < 0f)
                {
                    dashDirX = -1f;
                }
                reboundDir = new Vector2(dashDirX, -0.3f).normalized;
            }
            else
            {
                reboundDir = new Vector2(normalDir.x, setting.upForce).normalized;  // 튕겨나갈 방향
            }

            if (hitLayer == weaknessLayerIndex)
            {
                Debug.Log("약점과 충돌");
                BossMonster boss = collision.gameObject.GetComponentInParent<BossMonster>();
                cam?.ShakeCamera(0.25f, 0.6f);
                if (boss != null)
                {
                    Debug.Log("보스를 공격");

                    boss.TakeDamage(setting.playerDamage);
                }

                StopDash(reboundDir);
                return;
            }
            if (layerName == "Ground" || layerName == "Wall" || layerName == "Monster")
            {
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
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("플레이어 무적 상태");
            return;
        }
        else
        {
            hp -= damage;
            cam?.ShakeCamera(0.3f, 0.5f);
            Debug.Log("플레이어 체력 감소");
        }

        if(hp <= 0)
        {
            hp = 0;
            Die();
        }
        else
        {
            StartCoroutine(PlayerHitCoolTime());
        }
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        if (isDead)
        {
            return;
        }
        isDead = true;

        StartCoroutine(PlayerDie());
    }

    private IEnumerator PlayerDie()
    {
        rb.linearVelocity = Vector3.zero;

        BossMonster boss = FindFirstObjectByType<BossMonster>();
        if(boss != null)
        {
            boss.StopAllCoroutines();
        }

        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.8f, 0), 0.3f);
    }
}
