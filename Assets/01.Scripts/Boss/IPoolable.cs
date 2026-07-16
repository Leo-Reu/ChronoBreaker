using System;
using UnityEngine;

public interface IPoolable
{
    void Init(Action<Component> returnAction);

    void OnSpawn();

    void OnDeSpawn();
}
