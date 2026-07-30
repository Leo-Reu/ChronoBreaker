using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField, Range(0f, 1f)] private float bgmMasterScale = 0.5f;

    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    [SerializeField] private SoundData soundData;

    private Dictionary<SFXType, SFXData> sfxDict = new Dictionary<SFXType, SFXData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitSoundDictionary();
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

    private void InitSoundDictionary()
    {
        if (soundData == null || soundData.sfxList == null) return;

        sfxDict.Clear();
        foreach (var sfx in soundData.sfxList)
        {
            if (!sfxDict.ContainsKey(sfx.sfxType))
            {
                sfxDict.Add(sfx.sfxType, sfx);
            }
        }
    }

    public void PlaySFX(SFXType type)
    {
        if (sfxDict.TryGetValue(type, out SFXData data))
        {
            if (data.clip != null && SFXSource != null)
            {
                SFXSource.pitch = Random.Range(0.95f, 1.05f);
                SFXSource.PlayOneShot(data.clip, data.volume);
                SFXSource.pitch = 1.0f;
            }
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (BGMSource == null || clip == null) {
            return;
        }
        if (BGMSource.clip == clip && BGMSource.isPlaying)
        {
            return;
        }

        BGMSource.clip = clip;
        BGMSource.loop = loop;
        BGMSource.Play();
    }
    public void StopBGM()
    {
        if (BGMSource != null)
        {
            BGMSource.Stop();
        }
    }

    public SoundData GetSoundData() => soundData;



    public void SetBGMVolume(float volume)
    {
        if (BGMSource == null)
        {
            return;
        }
        BGMSource.volume = volume * bgmMasterScale;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (SFXSource == null) return;

        SFXSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat("BGMVolume", 1f);
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
