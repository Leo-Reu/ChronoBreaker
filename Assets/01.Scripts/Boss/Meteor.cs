using System;
using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour, IPoolable
{
    [SerializeField] private float fallSpeed = 12f;
    [SerializeField] private float lifeTime = 3f;

    private int damage;

    private Rigidbody2D rb;

    private Action<Component> reAction;

    CameraMove cam;

    private int groundLayerIndex;
    private int playerLayerIndex;

    private Coroutine autoReturnCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundLayerIndex = LayerMask.NameToLayer("Ground");
        playerLayerIndex = LayerMask.NameToLayer("Player");
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
        SoundManager.instance?.PlaySFX(SFXType.MeteorFall);

        if (autoReturnCoroutine != null)
        {
            StopCoroutine(autoReturnCoroutine);
        }
        autoReturnCoroutine = StartCoroutine(AutoReturnRoutine());
    }

    public void OnDeSpawn()
    {
        Debug.Log("메테오 회수");
        if (autoReturnCoroutine != null)
        {
            StopCoroutine(autoReturnCoroutine);
            autoReturnCoroutine = null;
        }
    }
    private IEnumerator AutoReturnRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        reAction?.Invoke(this);
    }

    void Update()
    {
        if(transform.position.y < -20f)
        {
            reAction?.Invoke(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int hitLayer = collision.collider.gameObject.layer;
        if (hitLayer == groundLayerIndex || hitLayer == playerLayerIndex)
        {
            cam?.ShakeCamera(0.2f, 0.4f);
            if (hitLayer == playerLayerIndex)
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
