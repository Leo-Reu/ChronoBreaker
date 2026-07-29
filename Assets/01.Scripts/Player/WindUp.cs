using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public struct WindUpData
{
    public Vector2 position;
    public Quaternion rotation;
    public Quaternion weaponRotation;
    public Vector3 scale;
    public Sprite sprite;

    public WindUpData(Vector2 _position, Quaternion _rotation, Quaternion _weaponRotation, Vector3 _scale, Sprite _sprite)
    {
        position = _position;
        rotation = _rotation;
        weaponRotation = _weaponRotation;
        scale = _scale;
        sprite = _sprite;
    }
}

public class WindUp : MonoBehaviour
{
    private PlayerSetting setting;

    private WindUpData[] history;
    private int nextIndex = 0;
    private int count = 0;
    private int maxSize;

    [SerializeField] private GameObject ghostPrefab;
    private GameObject ghost;
    private SpriteRenderer ghostSr;

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

    private void Start()
    {
        if (DataManager.instance != null)
        {
            setting = DataManager.instance.PlayerSetting;
        }
        if (setting != null)
        {
            maxSize = Mathf.CeilToInt(setting.windUpDuration / Time.fixedDeltaTime);
        }
        else
        {
            maxSize = 150; // setting을 못 불러와도 3초(150프레임) 기본값 지정
        }

        history = new WindUpData[maxSize];

        ShowGhost();
        isWindUp = false;

        StartCoroutine(WindUpCoolTime());
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && isWindUp == false && canWindUp) // R키를 누르고 WindUp중이 아니면 Start
        {
            StartWindUp();
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f || history == null)
        {
            return;
        }

        if (isWindUp)   // isWindUp이 true이면 Play
        {
            PlayWindUp();
        }
        else           // 아니면 녹화
        {
            RecordHistory();
        }
        UpdateGhost();
    }

    private void RecordHistory()
    {
        WindUpData data = new WindUpData(
            transform.position,
            transform.rotation,
            weaponTransform != null ? weaponTransform.rotation : Quaternion.identity,
            transform.localScale,
            sr != null ? sr.sprite : null
        );

        history[nextIndex] = data;
        nextIndex = (nextIndex + 1) % maxSize;

        if(count < maxSize)
        {
            count++;
        }
    }

    private void PlayWindUp()
    {
        for (int i = 0; i < setting.windUpSpeed; i++)
        {
            if (count > 0)
            {
                nextIndex = (nextIndex - 1 + maxSize) % maxSize;
                WindUpData target = history[nextIndex];

                transform.position = target.position;
                transform.rotation = target.rotation;
                if (weaponTransform != null) weaponTransform.rotation = target.weaponRotation;
                transform.localScale = target.scale;

                count--;
            }
            else
            {
                StopWindUp();
                break;
            }
        }
    }

    private void StartWindUp()
    {
        isWindUp = true;

        if(anim != null)
        {
            anim.SetBool("isWindUp", true);
        }

        rb.gravityScale = 0f;
        col.isTrigger = true;
        rb.linearVelocity = Vector2.zero;

        Color color = sr.color;
        color.a = 0.4f;
        sr.color = color;

        if (ghost != null) ghost.SetActive(false);

        Debug.Log("역행 시작");
    }

    private void StopWindUp()
    {
        isWindUp = false;

        if (anim != null)
        {
            anim.SetBool("isWindUp", false);
        }

        // 원상복구
        rb.gravityScale = 1f;
        col.isTrigger = false;
        rb.linearVelocity = Vector2.zero;


        // 투명도 되돌리기
        Color color = sr.color;
        color.a = 1f;
        sr.color = color;

        count = 0;
        nextIndex = 0;

        canWindUp = false;

        Debug.Log("역행 종료");
    }

    IEnumerator WindUpCoolTime()
    {
        UIManager.instance?.UpdateWindUpCool(setting.windUpCoolTime, setting.windUpCoolTime);
        while (true)
        {
            yield return new WaitWhile(() => canWindUp);

            windUpCoolTimeTimer = setting.windUpCoolTime;

            while (windUpCoolTimeTimer > 0f)
            {
                windUpCoolTimeTimer -= Time.deltaTime;

                float currentCool = setting.windUpCoolTime - windUpCoolTimeTimer;
                UIManager.instance?.UpdateWindUpCool(currentCool, setting.windUpCoolTime);

                yield return null;
            }
            windUpCoolTimeTimer = 0f;

            UIManager.instance?.UpdateWindUpCool(setting.windUpCoolTime, setting.windUpCoolTime);

            canWindUp = true;

            Debug.Log("시간역행 쿨타임 끝");
        }
    }

    private void ShowGhost()
    {
        ghost = (ghostPrefab != null) ? Instantiate(ghostPrefab) : new GameObject("Ghost");

        ghostSr = ghost.GetComponentInChildren<SpriteRenderer>();
        if (ghostSr == null)
        {
            ghostSr = ghost.AddComponent<SpriteRenderer>();
        }

        ghost.SetActive(false);
    }
    private void UpdateGhost()
    {
        if (ghost == null) 
        {
            return;
        }

        if (canWindUp && !isWindUp && count > 0)
        {
            ghost.SetActive(true);

            int oldestIndex = (nextIndex - count + maxSize) % maxSize;
            WindUpData ghostData = history[oldestIndex];

            ghost.transform.position = ghostData.position;
            ghost.transform.rotation = ghostData.rotation;
            ghost.transform.localScale = ghostData.scale;

            if (ghostSr != null && sr != null)
            {
                ghostSr.sprite = ghostData.sprite;
                ghostSr.color = new Color(0.3f, 0.7f, 1f, 0.45f);
                if (sr != null)
                {
                    ghostSr.sortingLayerID = sr.sortingLayerID;
                    ghostSr.sortingOrder = sr.sortingOrder - 1;
                }
            }
        }
        else
        {
            ghost.SetActive(false);
        }
    }
    public void HideGhost()
    {
        if (ghost != null)
        {
            ghost.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (ghost != null)
        {
            Destroy(ghost);
        }
    }
}
