using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Portal currentPortal;

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

    public void SetPortal(Portal portal)
    {
        currentPortal = portal;
    }

    public void OpenPortal()
    {
        if(currentPortal != null)
        {
            Debug.Log("포탈 활성화");
            currentPortal.gameObject.SetActive(true);
        }
    }

    public void BossDead(BossMonster boss)
    {
        if(boss.BossType == BossType.MidBoss)
        {
            SaveLoadManager.instance.gameSaveData.isMidBossClear = true;
            SaveLoadManager.instance.Save();
            OpenPortal();
        }
        else if (boss.BossType == BossType.FinalBoss)
        {
            SaveLoadManager.instance.gameSaveData.isFinalBossClear = true;
            SaveLoadManager.instance.Save();
            OpenPortal();
        }
    }

    public void GameOver()
    {
        Debug.Log("게임 오버");
    }
}
