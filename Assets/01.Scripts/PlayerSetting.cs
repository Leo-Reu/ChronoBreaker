using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Data/PlayerSetting")]
public class PlayerSetting : ScriptableObject
{
    public float moveSpeed = 4f;
    public float jumpPower = 5f;
    public float dashSpeed = 15f;
    public int playerDamage = 10;
    public float dashCoolTime = 1f;
}