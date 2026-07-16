using UnityEngine;

public class FinalBoss_LaserState : IState<FinalBoss>
{
    public void Enter(FinalBoss obj)
    {
        Debug.Log("최종보스 Laser패턴 시작");
        obj.ShootLaser();
    }
    public void Update(FinalBoss obj)
    {

    }

    public void Exit(FinalBoss obj)
    {
        Debug.Log("최종보스 Laser패턴 종료");
    }
}
