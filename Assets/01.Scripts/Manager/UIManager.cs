using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject option;
    private GameObject optionPanel;
    [SerializeField] private Canvas uiCanvas;

    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private Image windUpCoolImg;
    [SerializeField] private Image dashCoolImg;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject dim;

    private bool isPaused = false;

    [SerializeField] private Texture2D cursor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetCursor();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (optionPanel != null && optionPanel.activeSelf)
            {
                CloseOptionPanel();
            }
            else if(pausePanel != null)
            {
                GamePause();
            }
        }
    }

    public void SetUI(
        Canvas _uiCanvas,
        Slider _playerHpBar,
        Slider _bossHpBar,
        Image _windUpCoolImg,
        Image _dashCoolImg,
        GameObject _pausePanel,
        GameObject _gameOverPanel,
        GameObject _dim
    )
    {
        uiCanvas = _uiCanvas;
        playerHpBar = _playerHpBar;
        bossHpBar = _bossHpBar;
        windUpCoolImg = _windUpCoolImg;
        dashCoolImg = _dashCoolImg;
        pausePanel = _pausePanel;
        gameOverPanel = _gameOverPanel;
        dim = _dim;

        if (bossHpBar != null)
        {
            bossHpBar.gameObject.SetActive(false);
        }
    }

    public void SetCursor()
    {
        if(cursor != null)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
        else{
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    public void OpenOptionPanel()
    {
        SetCursor();
        if (optionPanel == null)
        {
            optionPanel = Instantiate(option, uiCanvas.transform);
        }
        else{
            optionPanel.SetActive(true);
        }

        if (dim != null)
        {
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
        }

        optionPanel.transform.SetAsLastSibling();
    }

    public void CloseOptionPanel()
    {
        if(optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
        if (dim != null)
        {
            dim.SetActive(false);
        }
    }

    public void UpdatePlayerHp(float currentHp, float maxHp)
    {
        if (playerHpBar != null)
        {
            playerHpBar.value = currentHp / maxHp;
        }
    }
    public void UpdateBossHp(float currentHp, float maxHp)
    {
        if (bossHpBar != null)
        {
            if(bossHpBar.gameObject.activeSelf == false)
            {
                bossHpBar.gameObject.SetActive(true);
            }
            bossHpBar.value = currentHp / maxHp;
        }
    }

    public void UpdateWindUpCool(float currentCool, float maxCool)
    {
        if(windUpCoolImg != null)
        {
            windUpCoolImg.fillAmount = currentCool / maxCool;
        }
    }

    public void UpdateDashCool(float currentCool, float maxCool)
    {
        if (dashCoolImg != null)
        {
            dashCoolImg.fillAmount = currentCool / maxCool;
        }
    }

    public void GamePause()
    {
        isPaused = !isPaused;
        ChangePauseState();
    }

    public void ResumeGame()
    {
        isPaused = false;
        ChangePauseState();
    }

    private void ChangePauseState()
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
            SetCursor();
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }

    public void ShowGameOver()
    {
        SetCursor();
        gameOverPanel.SetActive(true);
    }

    public void OnClickRestart()
    {
        SceneChanger.instance.RestartScene();
    }

    public void OnClickMainMenu()
    {
        SceneChanger.instance.ChangeScene("MainScene");
    }
}
