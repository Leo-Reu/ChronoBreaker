using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button closeBtn;
    public Slider bgmSlider;
    public Slider sfxSlider;

    
    private void Start()
    {
        bgmSlider.onValueChanged.AddListener(BGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
        closeBtn.onClick.AddListener(UIManager.instance.CloseOptionPanel);
    }

    private void OnEnable()
    {
        bgmSlider.value = SoundManager.instance.GetBGMVolume();
        sfxSlider.value = SoundManager.instance.GetSFXVolume();
    }

    public void BGMVolumeChanged(float vol)
    {
        SoundManager.instance.SetBGMVolume(vol);
    }
    public void SFXVolumeChanged(float vol)
    {
        SoundManager.instance.SetSFXVolume(vol);
    }
}