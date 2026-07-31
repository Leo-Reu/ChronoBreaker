using System;
using UnityEngine;

public class MeteorWarning : MonoBehaviour, IPoolable
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
        SoundManager.instance?.PlaySFX(SFXType.MeteorWarning);
    }

    private void Update()
    {
        if(sr != null)
        {
            float blink = Mathf.PingPong(Time.time * 8f, 0.5f) + 0.3f;
            Color c = sr.color;
            c.a = blink;
            sr.color = c;
        }
    }

    public void OnDeSpawn()
    {

    }
}
