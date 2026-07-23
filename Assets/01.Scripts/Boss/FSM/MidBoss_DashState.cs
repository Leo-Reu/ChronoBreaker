using System.Collections;
using UnityEngine;

public class MidBoss_DashState : IState<MidBoss>
{
    private bool isDash;
    private float dashDirX;
    private float timer;
    private int wallLayerMask = LayerMask.GetMask("Wall");

    public void Enter(MidBoss obj)
    {
        Debug.Log("중간보스 Dash상태 돌입");
        timer = 0f;
        isDash = false;

        obj.Stop();

        if (obj.PlayerTransform != null)
        {
            dashDirX = obj.PlayerTransform.position.x > obj.transform.position.x ? 1f : -1f;
        }
    }
    public void Update(MidBoss obj)
    {
        timer += Time.deltaTime;
        if (isDash == false)
        {
            if (timer >= obj.Setting.chargeDuration)
            {
                isDash = true;
                timer = 0f;
                obj.Dash(dashDirX);
                Debug.Log("중간 보스 돌진");
            }
        }
        else
        {
            Vector2 rayDirection = new Vector2(dashDirX, 0);
            RaycastHit2D hit = Physics2D.Raycast(obj.transform.position, rayDirection, 1.1f, wallLayerMask);
            if( hit.collider != null)
            {
                Debug.Log("벽과 충돌해 그로기 상태");
                obj.stateMachine.ChangeState(obj.groggyState);
                return;
            }
            if (timer >= obj.Setting.dashDuration)
            {
                isDash = false;
                Debug.Log("허공에 돌진");
                obj.stateMachine.ChangeState(obj.idleState);
            }
        }
    }

    public void Exit(MidBoss obj)
    {
        Debug.Log("중간보스 Dash상태 종료");
        obj.DashCoolTime();
        obj.Stop();
    }
}
