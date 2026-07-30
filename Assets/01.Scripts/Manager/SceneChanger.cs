using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    public string CurrentSceneName {  get; private set; }

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
        CurrentSceneName = SceneManager.GetActiveScene().name;
        PlaySceneBGM(CurrentSceneName);
    }

    public void ChangeScene(string sceneName)
    {
        CurrentSceneName = sceneName;
        Time.timeScale = 1f;
        PlaySceneBGM(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        PlaySceneBGM(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueScene()
    {
        if ((SaveLoadManager.instance.gameSaveData != null))
        {
            GameSaveData data = SaveLoadManager.instance.gameSaveData;

            if (data.isFinalBossClear)
            {
                ChangeScene("TutorialScene");   // 클리어했다면 이어할땐 처음부터
            }
            else if (data.isMidBossClear)
            {
                ChangeScene("FinalBossScene");
            }
            else
            {
                ChangeScene("MidBossScene");
            }
        }
        else
        {
            ChangeScene("TutorialScene");
        }
    }

    private void PlaySceneBGM(string sceneName)
    {
        SoundData soundData = SoundManager.instance?.GetSoundData();
        if (soundData == null) return;

        switch (sceneName)
        {
            case "MainScene":
                SoundManager.instance?.PlayBGM(soundData.bgmMain);
                break;
            case "TutorialScene":
                SoundManager.instance?.PlayBGM(soundData.bgmTutorial);
                break;
            case "MidBossScene":
                SoundManager.instance?.PlayBGM(soundData.bgmMidBoss);
                break;
            case "FinalBossScene":
                SoundManager.instance?.PlayBGM(soundData.bgmFinalBoss);
                break;
            case "ClearScene":
                SoundManager.instance?.PlayBGM(soundData.bgmClear);
                break;
        }
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
