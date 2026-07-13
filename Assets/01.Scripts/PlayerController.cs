using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    private float dir;
    private bool isGround;

    private int jumpCount;
    private int jumpCountMax;

    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        moveSpeed = 5f;
        jumpPower = 5f;

        jumpCount = 0;
        jumpCountMax = 2;
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
            jump();
        }
    }

    private void FixedUpdate()
    {
        Move();
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
    void Move()
    {
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
    }

    void jump()
    {
        if (jumpCount >= jumpCountMax)
        {
            return;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

        jumpCount++;
        Debug.Log("점프");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position - new Vector3(0, 0.8f, 0), 0.3f);
    }
}
