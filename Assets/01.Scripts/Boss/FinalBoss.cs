using System.Collections;
using UnityEngine;

public class FinalBoss : BossMonster
{
    [SerializeField] GameObject weakness;
    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } }

    [SerializeField] float groundYPos = -3.5f;

    public StateMachine<FinalBoss> stateMachine;

    public FinalBoss_IdleState idleState;
    public FinalBoss_LaserState laserState;
    public FinalBoss_MeteorState meteorState;
    public FinalBoss_GroggyState groggyState;

    [SerializeField] private GObjectPool<Meteor> meteorPool;
    [SerializeField] private GObjectPool<MeteorWarning> meteorWarningPool;
    [SerializeField] private GObjectPool<Laser> laserPool;
    [SerializeField] private GObjectPool<LaserWarning> laserWarningPool;

    private WaitForSeconds waitTwo = new WaitForSeconds(2f);
    private WaitForSeconds waitOne = new WaitForSeconds(1f);

    private MeteorWarning[] normalWarning = new MeteorWarning[3];
    private MeteorWarning[] finalWarning = new MeteorWarning[5];

    protected override void Awake()
    {
        base.Awake();
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

    public override BossType BossType { get { return BossType.FinalBoss; } }

    public void FallMeteor()
    {
        StartCoroutine(Meteor());
    }

    public void ShootLaser()
    {
        StartCoroutine(Laser());
    }

    private IEnumerator Meteor()
    {
        int normalCount = 3;
        float normalSpacing = 2.5f;

        for (int i = 0; i < 2; i++)
        {
            // 일반 메테오(캐릭터 좌표에 2초 후에 발사)
            if (PlayerTransform == null)
            {
                yield break;
            }

            Debug.Log($"{i + 1}번째 일반 메테오 발사 준비 (3개)");
            float targetXPos = PlayerTransform.position.x;

            for (int j = 0; j < normalCount; j++)
            {
                float spacing = (j - 1) * normalSpacing;
                Vector3 warningSpawnPos = new Vector3(targetXPos + spacing, groundYPos, 0);
                normalWarning[j] = meteorWarningPool.GetObject(warningSpawnPos);
            }
            yield return waitTwo;

            for (int j = 0; j < normalCount; j++)
            {
                if (normalWarning[j] != null)
                {
                    meteorWarningPool.ReturnPool(normalWarning[j]);
                }

                float spacing = (j - 1) * normalSpacing;
                Vector3 meteorSpawnPos = new Vector3(targetXPos + spacing, 10f, 0); // Y축 10 위에서 소환
                Meteor meteor = meteorPool.GetObject(meteorSpawnPos);
                if (meteor != null && setting != null)
                {
                    meteor.SetDamage(setting.bossDamage);
                }
            }
            yield return waitOne;
        }

        if (PlayerTransform != null)
        {
            Vector3 InitTrackingPos = new Vector3(PlayerTransform.position.x, groundYPos, 0);
            MeteorWarning trackingWarning = meteorWarningPool.GetObject(InitTrackingPos);
            float timer = 0f;

            while (timer < 2f)
            {
                if (PlayerTransform == null || trackingWarning == null)
                {
                    yield break;
                }

                trackingWarning.transform.position = new Vector3(PlayerTransform.position.x, groundYPos, 0);
                timer += Time.deltaTime;
                yield return null;
            }

            Debug.Log($"유도형 메테오 좌표 고정");
            Vector3 lockedPosition = trackingWarning.transform.position;

            meteorWarningPool.ReturnPool(trackingWarning);

            int trackingCount = 5;
            float trackingSpacing = 2.5f;

            for (int j = 0; j < trackingCount; j++)
            {
                float spacing = (j - 2) * trackingSpacing;
                Vector3 warningSpawnPos = lockedPosition + new Vector3(spacing, 0, 0);
                finalWarning[j] = meteorWarningPool.GetObject(warningSpawnPos);
            }

            yield return waitOne;

            Debug.Log($"유도형 메테오 5개 동시 발사");

            for (int j = 0; j < trackingCount; j++)
            {
                if (finalWarning[j] != null)
                {
                    meteorWarningPool.ReturnPool(finalWarning[j]);
                }

                float spacing = (j - 2) * trackingSpacing;
                Vector3 meteorSpawnPos = lockedPosition + new Vector3(spacing, 10f, 0);
                Meteor meteor = meteorPool.GetObject(meteorSpawnPos);
                if (meteor != null && setting != null)
                {
                    meteor.SetDamage(setting.bossDamage);
                }
            }
        }
        yield return waitTwo;
        stateMachine.ChangeState(laserState);
    }

    private IEnumerator Laser()
    {
        if (PlayerTransform == null) 
        {
            yield break;
        }

        Vector3 spawnPos = new Vector3(0f, groundYPos, 0f);

        Quaternion horizontalRotation = Quaternion.Euler(0f, 0f, 90f);

        LaserWarning warning = laserWarningPool.GetObject(spawnPos);
        if (warning != null)
        {
            warning.transform.rotation = horizontalRotation;
        }

        yield return waitTwo;

        if (warning != null)
        {
            laserWarningPool.ReturnPool(warning);
        }

        Laser laser = laserPool.GetObject(spawnPos);
        if (laser != null)
        {
            laser.transform.rotation = horizontalRotation;
            if (setting != null)
            {
                laser.SetDamage(setting.bossDamage);
            }
        }

        yield return waitOne;

        if (laser != null)
        {
            laserPool.ReturnPool(laser);
        }

        yield return waitTwo;
        stateMachine.ChangeState(groggyState);
    }

    public void EnterGroggy()
    {
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
        if (GameManager.instance != null)
        {
            GameManager.instance.BossDead(this);
        }
        Destroy(gameObject);
    }
}
