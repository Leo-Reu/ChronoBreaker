using System;
using UnityEngine;

public class Meteor : MonoBehaviour, IPoolable
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float fallSpeed = 6f;

    private Rigidbody2D rb;

    private Action<Component> reAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
    }

    public void OnSpawn()
    {
        Debug.Log("메테오 생성");
    }

    public void OnDeSpawn()
    {
        Debug.Log("메테오 회수");
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(0, -fallSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int hitLayer = collision.collider.gameObject.layer;
        string layerName = LayerMask.LayerToName(hitLayer);
        if (layerName == "Ground" || layerName == "Player")
        {
            if(layerName == "Player")
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.TakeDamage(damage);
                
            }
            reAction?.Invoke(this);
        }
    }
}
