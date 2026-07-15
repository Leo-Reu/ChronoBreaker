using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 velocity;

    private Vector3 targetPos;

    [SerializeField] private float smoothTime = 0.2f;

    void Start()
    {
        velocity = Vector3.zero;
    }

    void LateUpdate()
    {
        targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
