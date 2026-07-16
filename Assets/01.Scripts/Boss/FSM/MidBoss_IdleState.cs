using UnityEngine;

public class MidBoss_IdleState : IState<MidBoss>
{
    private float timer;

    public void Enter(MidBoss obj)
    {
        Debug.Log("중간보스 Idle상태 돌입");
        timer = 0f;
        obj.Stop();
    }

    public void Update(MidBoss obj)
    {
        timer += Time.deltaTime;
        if(timer >= obj.Setting.midBossIdleDuration)
        {
            obj.stateMachine.ChangeState(obj.traceState);
        }
    }
    public void Exit(MidBoss obj)
    {
        Debug.Log("중간보스 Idle상태 종료");
    }
}
