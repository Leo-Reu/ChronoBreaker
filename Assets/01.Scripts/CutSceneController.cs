using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;
    [SerializeField] private GameObject nextPanel;

    [SerializeField] private Sprite[] cutsceneSprites;

    private int currentIndex = 0;
    private bool isFinished = false;

    private void Start()
    {
        if (cutsceneSprites == null || cutsceneSprites.Length == 0)
        {
            EndCutscene();
            return;
        }

        ShowCutscene(0);
    }

    private void Update()
    {
        if (isFinished) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            NextCutscene();
        }
    }

    private void ShowCutscene(int index)
    {
        if (cutsceneImage != null && index < cutsceneSprites.Length)
        {
            cutsceneImage.sprite = cutsceneSprites[index];
        }
    }

    private void NextCutscene()
    {
        currentIndex++;

        if (currentIndex < cutsceneSprites.Length)
        {
            ShowCutscene(currentIndex);
            SoundManager.instance?.PlaySFX(SFXType.ButtonClick);
        }
        else
        {
            EndCutscene();
        }
    }

    private void EndCutscene()
    {
        isFinished = true;
        SoundManager.instance?.PlaySFX(SFXType.ButtonClick);

        gameObject.SetActive(false);

        if (nextPanel != null)
        {
            nextPanel.SetActive(true);
        }
    }
}