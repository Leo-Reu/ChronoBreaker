using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearSceneUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private TextMeshProUGUI deathCountText;

    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        // 1. 이번 판 결과 가져오기
        float currentRunTime = GameManager.instance != null ? GameManager.instance.CurrentPlayTime : 0f;
        int currentRunDeaths = GameManager.instance != null ? GameManager.instance.CurrentDeathCount : 0;

        var saveData = SaveLoadManager.instance?.gameSaveData;

        if (saveData != null)
        {
            saveData.deathCount += currentRunDeaths;

            if (currentRunTime < saveData.bestClearTime)
            {
                saveData.bestClearTime = currentRunTime;
            }

            SaveLoadManager.instance.Save();
        }

        if (clearTimeText != null)
            clearTimeText.text = $"Clear Time : {FormatTime(currentRunTime)}";

        if (bestTimeText != null)
            bestTimeText.text = $"Best Time  : {FormatTime(saveData != null ? saveData.bestClearTime : currentRunTime)}";

        if (deathCountText != null)
            deathCountText.text = $"Total Deaths : {saveData?.deathCount ?? currentRunDeaths}";

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(() => {
                SceneChanger.instance?.ChangeScene("MainScene");
            });
        }
    }
    private string FormatTime(float timeInSeconds)
    {
        if (timeInSeconds >= 9999f) return "--:--";

        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
