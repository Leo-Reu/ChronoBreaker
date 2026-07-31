using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button closeBtn;
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("Sub Panels")]
    [SerializeField] private GameObject controlGuidePanel;

    private void Start()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(BGMVolumeChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
        }
        if (closeBtn != null)
        {
            closeBtn.onClick.RemoveAllListeners();
            closeBtn.onClick.AddListener(() => {
                SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
                UIManager.instance?.CloseOptionPanel();
            });
        }
    }

    private void OnEnable()
    {
        ResetSubPanels();

        if (bgmSlider != null)
        {
            bgmSlider.value = SoundManager.instance.GetBGMVolume();
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = SoundManager.instance.GetSFXVolume();
        }
    }

    private void OnDisable()
    {
        ResetSubPanels();
    }

    private void ResetSubPanels()
    {
        if (controlGuidePanel != null)
        {
            controlGuidePanel.SetActive(false);
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