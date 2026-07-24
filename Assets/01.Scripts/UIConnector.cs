using UnityEngine;
using UnityEngine.UI;

public class UIConnector : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas;

    [SerializeField] private Slider playerHpBar;
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private Image windUpCoolImg;
    [SerializeField] private Image dashCoolImg;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject dim;

    [SerializeField] private Button pauseResumeButton;
    [SerializeField] private Button pauseOptionButton;
    [SerializeField] private Button pauseMainMenuButton;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button GameOverMainMenuButton;

    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if(uiCanvas == null)
        {
            uiCanvas = GetComponent<Canvas>();
        }

        UIManager.instance?.SetUI(
            uiCanvas,
            playerHpBar,
            bossHpBar,
            windUpCoolImg,
            dashCoolImg,
            pausePanel,
            gameOverPanel,
            dim
        );

        if(pauseResumeButton != null)
        {
            pauseResumeButton.onClick.RemoveAllListeners();
            pauseResumeButton.onClick.AddListener(() => UIManager.instance?.GamePause());
        }
        if(pauseOptionButton != null)
        {
            pauseOptionButton.onClick.RemoveAllListeners();
            pauseOptionButton.onClick.AddListener(() => UIManager.instance?.OpenOptionPanel());
        }
        if(pauseMainMenuButton != null)
        {
            pauseMainMenuButton.onClick.RemoveAllListeners();
            pauseMainMenuButton.onClick.AddListener(() => UIManager.instance?.OnClickMainMenu());
        }
        if(restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => UIManager.instance?.OnClickRestart());
        }
        if (GameOverMainMenuButton != null)
        {
            GameOverMainMenuButton.onClick.RemoveAllListeners();
            GameOverMainMenuButton.onClick.AddListener(() => UIManager.instance?.OnClickMainMenu());
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => {
                SaveLoadManager.instance?.ResetProgress();
                GameManager.instance?.ResetData();
                SceneChanger.instance?.ChangeScene("TutorialScene");
            });
        }
        if (continueButton != null)
        {
            bool hasSaveData = SaveLoadManager.instance?.gameSaveData != null;
            bool isCleared = hasSaveData && SaveLoadManager.instance.gameSaveData.isFinalBossClear;

            bool canContinue = hasSaveData && !isCleared;

            continueButton.interactable = canContinue;

            continueButton.onClick.RemoveAllListeners();
            if (canContinue)
            {
                continueButton.onClick.AddListener(() => SceneChanger.instance?.ContinueScene());
            }
        }
        if (optionButton != null)
        {
            optionButton.onClick.RemoveAllListeners();
            optionButton.onClick.AddListener(() => UIManager.instance?.OpenOptionPanel());
        }
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(() => SceneChanger.instance?.QuitGame());
        }
    }
}
