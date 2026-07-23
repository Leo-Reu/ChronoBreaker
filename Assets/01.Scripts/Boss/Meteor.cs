using System;
using UnityEngine;

public class Meteor : MonoBehaviour, IPoolable
{
    [SerializeField] private float fallSpeed = 12f;
    private int damage;

    private Rigidbody2D rb;

    private Action<Component> reAction;

    CameraMove cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
        cam = Camera.main.GetComponent<CameraMove>();
    }

    public void SetDamage(int Bossdamage)
    {
        damage = Bossdamage;
    }

    public void OnSpawn()
    {
        Debug.Log("메테오 생성");
        rb.linearVelocity = new Vector2(0, -fallSpeed);
    }

    public void OnDeSpawn()
    {
        Debug.Log("메테오 회수");
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(0, -fallSpeed);

        if(transform.position.y < -20f)
        {
            reAction?.Invoke(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int hitLayer = collision.collider.gameObject.layer;
        string layerName = LayerMask.LayerToName(hitLayer);
        if (layerName == "Ground" || layerName == "Player")
        {
            cam?.ShakeCamera(0.2f, 0.4f);
            if (layerName == "Player")
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }
            reAction?.Invoke(this);
        }
    }
}
