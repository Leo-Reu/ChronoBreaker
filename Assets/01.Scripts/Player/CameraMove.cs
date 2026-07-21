using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;


    private float defaultSize;
    [SerializeField] private float zoomSize = 7f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    public void ZoomIn(bool isZoom)
    {
        float size = isZoom ? zoomSize : defaultSize;
        float timeScale = isZoom ? 0.3f : 1f;

        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * timeScale;

        cam.DOOrthoSize(size, 0.2f).SetUpdate(true);
    }
}
