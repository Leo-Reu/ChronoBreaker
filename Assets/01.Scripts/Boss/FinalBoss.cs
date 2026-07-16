using System.Collections;
using UnityEngine;

public class FinalBoss : BossMonster
{
    [SerializeField] GameObject weakness;
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }

    public StateMachine<FinalBoss> stateMachine;

    public FinalBoss_IdleState idleState;
    public FinalBoss_LaserState laserState;
    public FinalBoss_MeteorState meteorState;
    public FinalBoss_GroggyState groggyState;

    private Rigidbody2D rb;

    [SerializeField] private GObjectPool<Meteor> meteorPool;
    [SerializeField] private GObjectPool<MeteorWarning> meteorWarningPool;

    private WaitForSeconds waitTwo = new WaitForSeconds(2f);
    private WaitForSeconds waitOne = new WaitForSeconds(1f);


    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        idleState = new FinalBoss_IdleState();
        laserState = new FinalBoss_LaserState();
        meteorState = new FinalBoss_MeteorState();
        groggyState = new FinalBoss_GroggyState();

        stateMachine = new StateMachine<FinalBoss>(this, idleState);

        isInvincible = true;    // 평소엔 무적(약점이 노출될때만 공격가능)

        weakness.SetActive(false);  // 약점 비활성화
    }
    public void Update()
    {
        stateMachine?.Update();
    }

    public void FallMeteor()
    {
        StartCoroutine(Meteor());
    }

    private IEnumerator Meteor()
    {
        for(int i = 0; i < 2; i++)
        {
            // 일반 메테오(캐릭터 좌표에 2초 후에 발사)
            if(PlayerTransform == null)
            {
                yield break;
            }
            Debug.Log($"{i + 1}번째 일반 메테오 발사");
            Vector3 targetPos = PlayerTransform.position;

            Vector3 leftPos = targetPos + new Vector3(-3f, 0, 0);
            Vector3 centerPos = targetPos;
            Vector3 rightPos = targetPos + new Vector3(3f, 0, 0);

            MeteorWarning warningLeft = meteorWarningPool.GetObject(leftPos);
            MeteorWarning warningCenter = meteorWarningPool.GetObject(centerPos);
            MeteorWarning warningRight = meteorWarningPool.GetObject(rightPos);

            yield return waitTwo;

            meteorWarningPool.ReturnPool(warningLeft);
            meteorWarningPool.ReturnPool(warningCenter);
            meteorWarningPool.ReturnPool(warningRight);

            Vector3 fallPosition = new Vector3(0, 10f, 0);

            meteorPool.GetObject(leftPos + fallPosition);
            meteorPool.GetObject(centerPos + fallPosition);
            meteorPool.GetObject(rightPos + fallPosition);

            yield return waitOne;
        }
        // 유도형 메테오(캐릭터를 2초간 추적 후 그 좌표에 1초 후에 발사)
        if (PlayerTransform != null)
        {
            MeteorWarning trackingWarning = meteorWarningPool.GetObject(PlayerTransform.position);
            float timer = 0f;
            while (timer < 2f)
            {
                trackingWarning.transform.position = PlayerTransform.position;
                timer += Time.deltaTime;
                yield return null;
            }
            Debug.Log($"유도형 메테오 좌표 고정");
            Vector3 lockedPosition = trackingWarning.transform.position;

            yield return waitOne;
            Debug.Log($"유도형 메테오 발사");

            meteorWarningPool.ReturnPool(trackingWarning);

            meteorPool.GetObject(lockedPosition + new Vector3(0, 10f, 0));
        }
        yield return waitTwo;
        stateMachine.ChangeState(laserState);
    }

    public void EnterGroggy()
    {
        // 패턴 파훼 후 그로기
        isInvincible = false;
        weakness.SetActive(true);
    }

    public void ExitGroggy()
    {
        isInvincible = true;
        weakness.SetActive(false);
    }


    protected override void Die()
    {
        Debug.Log("최종보스 처치");
        Destroy(gameObject);
    }
}
