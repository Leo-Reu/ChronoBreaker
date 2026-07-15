using UnityEngine;

public class FinalBoss : BossMonster
{
    [SerializeField] GameObject weakness;

    protected override void Start()
    {
        base.Start();

        isInvincible = true;    // 평소엔 무적(약점이 노출될때만 공격가능)

        weakness.SetActive(false);  // 약점 비활성화
    }

    public void EnterGroggy()
    {
        // 패턴 파훼 후 그로기
        isInvincible = false;
        weakness.SetActive(true);
    }

    public void ExitGroggy()
    {
        isInvincible = true;
        weakness.SetActive(false);
    }


    protected override void Die()
    {
        Debug.Log("최종보스 처치");
        Destroy(gameObject);
    }
}
