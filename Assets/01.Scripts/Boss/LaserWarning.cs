using System;
using UnityEngine;

public class LaserWarning : MonoBehaviour, IPoolable
{
    private Action<Component> reAction;

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
    }

    public void OnSpawn()
    {
        SoundManager.instance?.PlaySFX(SFXType.LaserWarning);
    }

    public void OnDeSpawn()
    {

    }
}
