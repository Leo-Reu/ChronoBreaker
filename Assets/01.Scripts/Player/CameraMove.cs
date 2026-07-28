using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;

    [SerializeField] private bool useBounds = true;
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    [SerializeField] private float minY = -1.5f;
    [SerializeField] private float maxY = 3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;


    private float defaultSize;
    [SerializeField] private float zoomSize = 4f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        
        if (useBounds)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void ZoomIn(bool isZoom)
    {
        float size = isZoom ? zoomSize : defaultSize;
        if(Time.timeScale > 0f)
        {
            float timeScale = isZoom ? 0.3f : 1f;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = 0.02f * timeScale;
        }

        cam.DOOrthoSize(size, 0.2f).SetUpdate(true);
    }

    public void ShakeCamera(float duration = 0.2f, float strength = 0.4f, int vibrato = 20)
    {
        cam.transform.DOComplete();

        cam.transform.DOShakePosition(duration, strength, vibrato).SetUpdate(true);
    }
}
