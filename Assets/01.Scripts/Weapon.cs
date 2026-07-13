using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    protected new Camera camera;

    protected virtual void Start()
    {
        camera = Camera.main;
    }

    protected abstract void Fire();

    protected void LookMouse()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector3 worldPos = camera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        Vector2 dir = worldPos - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
