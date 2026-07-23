using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;

    private string fileName;
    private string savePath;

    public GameSaveData gameSaveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        fileName = "gameSaveData.json";
        savePath = Path.Combine(Application.persistentDataPath, fileName);

        Load();
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(gameSaveData, true);

        File.WriteAllText(savePath, json);

        Debug.Log($"데이터 저장 완료 : {savePath}");
    }

    private void Load()
    {
        if(File.Exists(savePath) == false)
        {
            Debug.Log("불러올 데이터가 없습니다");
            gameSaveData = new GameSaveData();
            Save();
            Debug.Log("새로운 데이터 생성");
        }

        string json = File.ReadAllText(savePath);
        gameSaveData = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("데이터 불러오기 성공");
    }
}
