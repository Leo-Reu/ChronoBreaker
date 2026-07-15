using UnityEngine;

public class MidBoss : BossMonster
{
    protected override void Start()
    {
        base.Start();

    }


    protected override void Die()
    {
        Debug.Log("중간보스 처치");
        Destroy(gameObject);
    }
}
