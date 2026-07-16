using UnityEngine;

public class FinalBoss_IdleState : IState<FinalBoss>
{
    private float timer;

    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Idle상태 돌입");
        timer = 0f;
    }
    public void Update(FinalBoss obj)
    {
        timer += Time.deltaTime;
        if(timer >= obj.Setting.finalBossIdleDuration)
        {
            obj.stateMachine.ChangeState(obj.meteorState);
        }
    }

    public void Exit(FinalBoss obj)
    {
        Debug.Log("최종보스 Idle상태 종료");
    }
}
