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
            pauseResumeButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.ResumeGame();
            });
        }
        if(pauseOptionButton != null)
        {
            pauseOptionButton.onClick.RemoveAllListeners();
            pauseOptionButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.OpenOptionPanel();
            });
        }
        if(pauseMainMenuButton != null)
        {
            pauseMainMenuButton.onClick.RemoveAllListeners();
            pauseMainMenuButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.OnClickMainMenu();
            });
        }
        if(restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.OnClickRestart();
            });
        }
        if (GameOverMainMenuButton != null)
        {
            GameOverMainMenuButton.onClick.RemoveAllListeners();
            GameOverMainMenuButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.OnClickMainMenu();
            });
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                SaveLoadManager.instance?.ResetProgress();
                GameManager.instance?.ResetData();
                SceneChanger.instance?.ChangeScene("TutorialScene");
            });
        }
        if (continueButton != null)
        {
            var saveData = SaveLoadManager.instance?.gameSaveData;

            bool canContinue = saveData != null && !saveData.isFinalBossClear;

            continueButton.interactable = canContinue;

            CanvasGroup cg = continueButton.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = canContinue ? 1.0f : 0.7f;
            }

            continueButton.onClick.RemoveAllListeners();
            if (canContinue)
            {
                continueButton.onClick.AddListener(() => {
                    SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                    SceneChanger.instance?.ContinueScene();
                });
            }
        }
        if (optionButton != null)
        {
            optionButton.onClick.RemoveAllListeners();
            optionButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.OpenOptionPanel();
            });
        }
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                SceneChanger.instance?.QuitGame();
            });
        }
    }
}
