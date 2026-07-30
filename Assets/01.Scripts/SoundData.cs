using System;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    PlayerJumpStart,
    PlayerJumpLand,
    PlayerRun,
    PlayerDash,
    PlayerHurt,
    PlayerDie,
    PlayerHit,

    SpringFire,
    SpringHit,
    WindUpStart,
    WindUpEnd,

    BossDashHit,
    BossDie,
    MeteorWarning,
    MeteorFall,
    LaserWarning,
    LaserShoot,

    ButtonClick,
    PortalEnter
}

[Serializable]
public struct SFXData
{
    public SFXType sfxType;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
}

[CreateAssetMenu(fileName = "SoundData", menuName = "Data/SoundData")]

public class SoundData : ScriptableObject
{
    public AudioClip bgmMain;
    public AudioClip bgmTutorial;
    public AudioClip bgmMidBoss;
    public AudioClip bgmFinalBoss;
    public AudioClip bgmClear;

    public List<SFXData> sfxList;
}
