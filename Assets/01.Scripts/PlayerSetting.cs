using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Data/PlayerSetting")]
public class PlayerSetting : ScriptableObject
{
    public float moveSpeed = 4f;
    public float jumpPower = 5f;
    public float dashSpeed = 15f;
    public float dashCoolTime = 1f;
    public float reboundPower = 8f;
    public float upForce = 1.2f;

    public float windUpDuration = 3f;
    public float windUpCoolTime = 10f;

    public float maxHp = 10;
    public int playerDamage = 10;
    public float hitCoolTime = 1.0f;
}