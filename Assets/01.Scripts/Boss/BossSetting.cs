using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSetting", menuName = "Data/BossSetting")]
public class BossSetting : ScriptableObject
{
    public float maxHp = 100;
    public float hitCoolTime = 0.5f;

    public int bossDamage = 1;
    public float speed = 3f;

    public float midBossIdleDuration = 2f;
    public float groggyDuration = 3f;

    public float chargeDuration = 2f;
    public float dashSpeed = 10f;
    public float dashDuration = 3f;
    public float dashCoolTime = 4f;

    public float finalBossIdleDuration = 3f;
}