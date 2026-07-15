using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSetting", menuName = "Data/BossSetting")]
public class BossSetting : ScriptableObject
{
    public float maxHp = 100;
    public float hitCoolTime = 0.5f;

    public int bossDamage = 1;
}