using System.Collections;
using UnityEngine;

public class MidBoss_DashState : IState<MidBoss>
{
    private bool isDash;
    private float dashDirX;
    private float timer;
    private int wallLayerMask = LayerMask.GetMask("Wall");
    CameraMove cam;
    public void Enter(MidBoss obj)
    {
        Debug.Log("중간보스 Dash상태 돌입");
        obj.Anim.Play("MidBoss_Dash");
        timer = 0f;
        isDash = false;
        cam = Camera.main?.GetComponent<CameraMove>();
        obj.Stop();

        if (obj.PlayerTransform != null)
        {
            dashDirX = obj.PlayerTransform.position.x > obj.transform.position.x ? 1f : -1f;
            obj.LookDirection(dashDirX);
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
                SoundManager.instance?.PlaySFX(SFXType.BossDashHit);

                cam?.ShakeCamera(0.4f, 0.6f);

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
