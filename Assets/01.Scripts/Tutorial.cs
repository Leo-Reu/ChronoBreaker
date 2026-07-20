using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    WaitForSeconds openDelay = new WaitForSeconds(5f);

    private void Start()
    {
        StartCoroutine(OpenPortalDelay());
    }

    private IEnumerator OpenPortalDelay()
    {
        yield return openDelay;
        if (GameManager.instance != null)
        {
            GameManager.instance.OpenPortal();
        }
    }
}
