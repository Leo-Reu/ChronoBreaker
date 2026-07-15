using UnityEngine;

public class MidBoss_GroggyState : IState<MidBoss>
{
    private float timer;

    public void Enter(MidBoss obj)
    {
        Debug.Log("Groggy상태 돌입");
        timer = 0f;
        obj.Stop();
    }

    public void Update(MidBoss obj)
    {
        timer += Time.deltaTime;
        if(timer > obj.Setting.groggyDuration)
        {
            obj.stateMachine.ChangeState(obj.idleState);
        }
    }
    public void Exit(MidBoss obj)
    {
        Debug.Log("Groggy상태 종료");
    }
}
