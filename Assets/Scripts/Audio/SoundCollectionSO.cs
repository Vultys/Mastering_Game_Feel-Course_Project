using UnityEngine;

[CreateAssetMenu(fileName = "SoundCollectionSO", menuName = "SoundCollectionSO", order = 0)]
public class SoundCollectionSO : ScriptableObject 
{
    [Header("Music")]
    public SoundSO[] FightMusic;
    public SoundSO[] DiscoBallMusic;

    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] Jetpack;
    public SoundSO[] GrenadeShoot;
    public SoundSO[] GrenadeExplode;
    public SoundSO[] GrenadeBeep;
    public SoundSO[] PlayerHit;
}
