using UnityEngine;

public class FinalBoss_GroggyState : IState<FinalBoss>
{
    float timer;

    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Groggy상태 돌입");
        timer = 0f;
        obj.EnterGroggy();
    }
    public void Update(FinalBoss obj)
    {
        timer += Time.deltaTime;

        if(timer >= obj.Setting.groggyDuration)
        {
            obj.stateMachine.ChangeState(obj.idleState);
        }
    }

    public void Exit(FinalBoss obj)
    {
        Debug.Log("최종보스 Groggy상태 종료");
        obj.ExitGroggy();
    }
}
