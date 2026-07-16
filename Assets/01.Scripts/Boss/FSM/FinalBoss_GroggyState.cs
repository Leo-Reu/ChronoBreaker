using UnityEngine;

public class FinalBoss_GroggyState : IState<FinalBoss>
{

    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Groggy상태 돌입");
    }
    public void Update(FinalBoss obj)
    {

    }

    public void Exit(FinalBoss obj)
    {
        Debug.Log("최종보스 Groggy상태 종료");
    }
}
