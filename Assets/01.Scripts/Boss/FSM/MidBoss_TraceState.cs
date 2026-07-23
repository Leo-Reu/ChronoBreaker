using UnityEngine;

public class MidBoss_TraceState : IState<MidBoss>
{
    private Transform playerTransform;

    private float dashRange = 5f;

    public void Enter(MidBoss obj)
    {
        Debug.Log("중간보스 Trace상태 돌입");
        playerTransform = obj.PlayerTransform;
    }
    public void Update(MidBoss obj)
    {
        if (playerTransform == null)
        {
            return;
        }
        float distance = Vector2.Distance(obj.transform.position, playerTransform.position);
        if(distance <= dashRange && obj.canDash)
        {
            obj.stateMachine.ChangeState(obj.dashState);
            return;
        }
        float dirX = playerTransform.position.x > obj.transform.position.x ? 1f : - 1f;
        obj.Move(dirX);
    }

    public void Exit(MidBoss obj)
    {
        Debug.Log("중간보스 Trace상태 종료");
        obj.Stop();
    }
}
