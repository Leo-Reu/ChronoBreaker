using UnityEngine;

public class FinalBoss_MeteorState : IState<FinalBoss>
{
    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Meteor패턴 시작");
        obj.FallMeteor();
    }
    public void Update(FinalBoss obj)
    {

    }

    public void Exit(FinalBoss obj)
    {
        Debug.Log("최종보스 Meteor패턴 종료");
    }
}
