using System;
using UnityEngine;

public class LaserWarning : MonoBehaviour, IPoolable
{
    private Action<Component> reAction;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
    }

    public void OnSpawn()
    {
        SoundManager.instance?.PlaySFX(SFXType.LaserWarning);
    }
    private void Update()
    {
        if (sr != null)
        {
            float blink = Mathf.PingPong(Time.time * 12f, 0.4f) + 0.2f;
            Color c = sr.color;
            c.a = blink;
            sr.color = c;
        }
    }

    public void OnDeSpawn()
    {

    }
}
