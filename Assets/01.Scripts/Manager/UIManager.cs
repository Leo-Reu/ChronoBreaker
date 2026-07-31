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
            bossHpBar.transform.parent.gameObject.SetActive(false);
        }

        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (dim != null) dim.SetActive(false);
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
            bool isGameOverActive = (gameOverPanel != null && gameOverPanel.activeSelf);

            if (isPaused && pausePanel != null)
            {
                dim.transform.SetAsLastSibling();
                pausePanel.transform.SetAsLastSibling();
            }
            else if (isGameOverActive)
            {
                dim.transform.SetAsLastSibling();
                gameOverPanel.transform.SetAsLastSibling();
            }
            else
            {
                dim.SetActive(false);
            }
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
            if(bossHpBar.transform.parent.gameObject.activeSelf == false)
            {
                bossHpBar.transform.parent.gameObject.SetActive(true);
            }
            bossHpBar.value = currentHp / maxHp;
        }
    }

    public void UpdateWindUpCool(float currentCool, float maxCool)
    {
        if(windUpCoolImg != null)
        {
            windUpCoolImg.fillAmount = 1f - (currentCool / maxCool);
        }
    }

    public void UpdateDashCool(float currentCool, float maxCool)
    {
        if (dashCoolImg != null)
        {
            dashCoolImg.fillAmount = 1f - (currentCool / maxCool);
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

            if (dim != null)
            {
                dim.SetActive(true);
                dim.transform.SetAsLastSibling();
            }

            if (pausePanel != null)
            {
                pausePanel.SetActive(true);
                pausePanel.transform.SetAsLastSibling();
            }
        }
        else
        {
            Time.timeScale = 1f;

            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            bool isOptionActive = (optionPanel != null && optionPanel.activeSelf);
            if (dim != null && !isOptionActive)
            {
                dim.SetActive(false);
            }
        }
    }

    public void ShowGameOver()
    {
        SetCursor();

        if (dim != null)
        {
            dim.SetActive(true);
            dim.transform.SetAsLastSibling();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }
    }

    public void OnClickRestart()
    {
        isPaused = false;
        SceneChanger.instance.RestartScene();
    }

    public void OnClickMainMenu()
    {
        isPaused = false;
        SceneChanger.instance.ChangeScene("MainScene");
    }
}
