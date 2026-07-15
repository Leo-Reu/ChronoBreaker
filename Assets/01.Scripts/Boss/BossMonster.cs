using System.Collections;
using UnityEngine;

public abstract class BossMonster : MonoBehaviour
{
    [SerializeField] protected BossSetting setting;
    public BossSetting Setting { get { return setting; } }

    [SerializeField] protected float hp;

    protected bool isInvincible;

    protected Collider2D col;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        hp = setting.maxHp;
        isInvincible = false;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            Debug.Log("보스 무적 상태");
            return;
        }
        else
        {
            hp -= damage;
            Debug.Log("보스 공격 성공");
        }

        if(hp <= 0)
        {
            hp = 0;
            Die();
        }
        else
        {
            StartCoroutine(BossHitCoolTime());
        }
    }

    IEnumerator BossHitCoolTime()
    {
        isInvincible = true;

        yield return new WaitForSeconds(setting.hitCoolTime);

        isInvincible = false;
    }

    protected abstract void Die();
}