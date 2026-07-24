using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

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
    }

    void Start()
    {
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1));
    }

    public void SetBGMVolume(float volume)
    {
        if (BGMSource == null)
        {
            return;
        }
        BGMSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (SFXSource == null)
        {
            return;
        }
        SFXSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetBGMVolume()
    {
        if(BGMSource == null)
        {
            return 0;
        }
        return BGMSource.volume;
    }
    public float GetSFXVolume()
    {
        if(SFXSource == null)
        {
            return 0;
        }
        return SFXSource.volume;
    }
}
