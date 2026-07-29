using UnityEngine;

public class FinalBoss_IdleState : IState<FinalBoss>
{
    private float timer;

    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Idle상태 돌입");
        if (obj.Anim != null)
        {
            obj.Anim.speed = 1.0f;
            obj.Anim.Play("FinalBoss_Idle");
        }
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
