using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button closeBtn;
    public Slider bgmSlider;
    public Slider sfxSlider;

    
    private void Start()
    {
        if(bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(BGMVolumeChanged);
        }
        if(sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
        }
        if(closeBtn != null)
        {
            closeBtn.onClick.AddListener(UIManager.instance.CloseOptionPanel);
        }
    }

    private void OnEnable()
    {
        if(bgmSlider != null)
        {
            bgmSlider.value = SoundManager.instance.GetBGMVolume();
        }
        if(sfxSlider != null)
        {
            sfxSlider.value = SoundManager.instance.GetSFXVolume();
        }
    }

    public void BGMVolumeChanged(float vol)
    {
        SoundManager.instance?.SetBGMVolume(vol);
    }
    public void SFXVolumeChanged(float vol)
    {
        SoundManager.instance?.SetSFXVolume(vol);
    }
}