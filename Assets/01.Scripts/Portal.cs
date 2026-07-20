using UnityEngine;
using UnityEngine.InputSystem;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetScene;
    private bool isInPortal;

    void Update()
    {
        if(isInPortal && Keyboard.current.wKey.wasPressedThisFrame)
        {
            Debug.Log($"포탈 작동 {targetScene}으로 이동합니다");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInPortal = false;
        }
    }
}
