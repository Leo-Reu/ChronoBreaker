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
    private bool isDashing;
    private bool isBounding;
    private bool isAttacking;

    private int jumpCount;
    private int jumpCountMax;

    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask groundLayer;

    //[SerializeField] private float dashCoolTime = 1f;
    private float dashCoolTimeTimer = 1f;
    private bool canDash = true;

    private int weaknessLayerIndex;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
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
            if (isDashing && isAttacking == false)
            {
                DashJump();
            }
            else if(isDashing == false)
            {
                jump();
            }
        }
    }

    private void FixedUpdate()
    {
        if(isDashing == false && isBounding == false)
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
        return canDash == true && isDashing == false;
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
        isAttacking = isAttack;
        Vector2 dashDir = (targetPos - (Vector2)transform.position).normalized;
        isDashing = true;

        rb.gravityScale = 0f;
        rb.linearVelocity = dashDir * setting.dashSpeed;
        Debug.Log("대쉬 시작");
    }

    void DashJump()
    {
        isDashing = false;
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 1.1f, setting.jumpPower);
        jumpCount = 1; // 대쉬 중 점프 후 한번 더 점프할 수 있도록
        Debug.Log("대쉬 중 점프");
    }

    void StopDash(Vector2 reboundPower)
    {
        isDashing = false;
        isAttacking = false;
        rb.gravityScale = 1f;

        canDash = false;

        float power = 3f;
        rb.linearVelocity = reboundPower * power;
        StartCoroutine(Rebounding());   // 벽면 x축 반동을 위해 Move()함수 0.1초 억제

        Debug.Log("대쉬 종료");
    }

    IEnumerator Rebounding()
    {
        isBounding = true;
        yield return new WaitForSeconds(0.1f);
        isBounding = false;
    }

    IEnumerator DashCoolTime()
    {
        while (true)
        {
            yield return new WaitWhile(() => canDash);

            dashCoolTimeTimer = dashCoolTime;

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
        if (isDashing)
        {
            if(collision.gameObject.layer == weaknessLayerIndex && boss != null)
            {
                boss.TakeDamage(setting.playerDamage);
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
