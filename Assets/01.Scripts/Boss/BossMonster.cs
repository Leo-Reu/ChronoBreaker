using System.Collections;
using UnityEngine;

public enum BossType
{
    MidBoss,
    FinalBoss
}

public abstract class BossMonster : MonoBehaviour
{
    [SerializeField] protected BossSetting setting;
    public BossSetting Setting { get { return setting; } }

    [SerializeField] protected float hp;

    protected bool isHitInvincible;
    protected bool isStateInvincible;
    protected bool isPhaseTwo;

    public bool IsInvincible => isHitInvincible || isStateInvincible;

    protected Collider2D col;


    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        hp = setting.maxHp;
        isHitInvincible = false;
        isStateInvincible = false;
        isPhaseTwo = false;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsInvincible)
        {
            Debug.Log("보스 무적 상태");
            return;
        }

        hp -= damage;
        Debug.Log("보스 공격 성공");

        if (!isPhaseTwo && hp <= setting.maxHp / 2)
        {
            isPhaseTwo = true;
            EnterPhaseTwo();
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
        isHitInvincible = true;

        yield return new WaitForSeconds(setting.hitCoolTime);

        isHitInvincible = false;
    }

    protected virtual void EnterPhaseTwo()
    {
        Debug.Log("보스 2페이즈 진입");
    }

    protected abstract void Die();

    public abstract BossType BossType { get; }
}