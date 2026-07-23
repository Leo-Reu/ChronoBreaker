using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject option;
    private GameObject optionPanel;
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private GameObject dim;

    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private Image windUpCoolImg;
    [SerializeField] private Image dashCoolImg;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    private bool isPaused = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void OpenOptionPanel()
    {
        if(optionPanel == null)
        {
            optionPanel = Instantiate(option, UICanvas.transform);
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
        windUpCoolImg.fillAmount = currentCool / maxCool;
    }

    public void UpdateDashCool(float currentCool, float maxCool)
    {
        dashCoolImg.fillAmount = currentCool / maxCool;
    }

    public void GamePause()
    {
        isPaused = !isPaused;
        if(isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        pausePanel.SetActive(isPaused);
    }

    public void ShowGameOver()
    {
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
