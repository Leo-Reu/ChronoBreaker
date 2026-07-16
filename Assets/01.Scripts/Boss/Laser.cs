using System;
using UnityEngine;

public class Laser : MonoBehaviour, IPoolable
{
    private int damage;

    private Action<Component> reAction;

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
    }

    public void OnSpawn()
    {
        Debug.Log("레이저 발사");
    }

    public void OnDeSpawn()
    {
        Debug.Log("레이저 종료");
    }
    public void SetDamage(int Bossdamage)
    {
        damage = Bossdamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int hitLayer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(hitLayer);

        if (layerName == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
