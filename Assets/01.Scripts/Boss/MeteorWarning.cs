using System;
using UnityEngine;

public class MeteorWarning : MonoBehaviour, IPoolable
{
    private Action<Component> reAction;

    public void Init(Action<Component> returnAction)
    {
        reAction = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDeSpawn()
    {

    }
}
