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
    }

    public void ChangeScene(string sceneName)
    {
        CurrentSceneName = sceneName;
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
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

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
