using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;

public struct WindUpData
{
    public Vector2 position;
    public Quaternion rotation;
    public Quaternion weaponRotation;
    
    public WindUpData(Vector2 _position, Quaternion _rotation, Quaternion _weaponRotation)
    {
        position = _position;
        rotation = _rotation;
        weaponRotation = _weaponRotation;   // 무기
        // 애니메이션은 추가예정
    }
}

public class WindUp : MonoBehaviour
{
    private PlayerSetting setting;

    private List<WindUpData> history;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;

    private Transform weaponTransform;

    private float windUpCoolTimeTimer = 0f;
    private bool canWindUp = true;

    public bool isWindUp { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        weaponTransform = GetComponentInChildren<SpringWeapon>().transform;
    }

    void Start()
    {
        setting = DataManager.instance.PlayerSetting;
        history = new List<WindUpData>();
        isWindUp = false;

        StartCoroutine(WindUpCoolTime());
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && isWindUp == false && canWindUp) // R키를 누르고 WindUp중이 아니면 Start
        {
            StartWindUp();
        }
    }

    private void FixedUpdate()
    {
        if (isWindUp)   // isWindUp이 true이면 Play
        {
            PlayWindUp();
        }
        else           // 아니면 녹화
        {
            RecordHistory();
        }
    }

    private void RecordHistory()
    {
        float maxCount = setting.windUpDuration / Time.fixedDeltaTime;  // 설정된시간 / fixedDeltaTime(0.02초) 만큼 저장

        if(history.Count >= maxCount)   // windUpDuration을 넘어가면 오래된 데이터부터 삭제
        {
            history.RemoveAt(0);
        }

        // 현재 상태 리스트에 추가
        WindUpData data = new WindUpData(transform.position, transform.rotation, weaponTransform.rotation);
        history.Add(data);
    }

    private void PlayWindUp()
    {
        if(history.Count > 0)
        {
            int lastidx = history.Count - 1;
            WindUpData target = history[lastidx];

            transform.position = target.position;
            transform.rotation = target.rotation;
            weaponTransform.rotation = target.weaponRotation;

            history.RemoveAt(lastidx);
        }
        else
        {
            StopWindUp();
        }
    }

    private void StartWindUp()
    {
        isWindUp = true;

        // 중력 영향 X, 장애물 영향 X, 속도 영향X
        rb.gravityScale = 0f;
        col.isTrigger = true;
        rb.linearVelocity = Vector2.zero;

        // WindUp중 약간 투명하게
        Color color = sr.color;
        color.a = 0.4f;
        sr.color = color;

        Debug.Log("역행 시작");
    }

    private void StopWindUp()
    {
        isWindUp = false;

        // 원상복구
        rb.gravityScale = 1f;
        col.isTrigger = false;
        rb.linearVelocity = Vector2.zero;


        // 투명도 되돌리기
        Color color = sr.color;
        color.a = 1f;
        sr.color = color;

        history.Clear();

        canWindUp = false;

        Debug.Log("역행 종료");
    }

    IEnumerator WindUpCoolTime()
    {
        while (true)
        {
            yield return new WaitWhile(() => canWindUp);

            windUpCoolTimeTimer = setting.windUpCoolTime;

            while (windUpCoolTimeTimer > 0f)
            {
                windUpCoolTimeTimer -= Time.deltaTime;
                yield return null;
            }
            windUpCoolTimeTimer = 0f;
            canWindUp = true;

            Debug.Log("시간역행 쿨타임 끝");
        }
    }
}
