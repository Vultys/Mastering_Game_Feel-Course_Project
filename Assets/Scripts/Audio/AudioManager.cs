using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionSO _soundCollectionSO;

    private void OnEnable() 
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
    }

    private void OnDisable() 
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
    }

    private void PlayRandomSound(SoundSO[] sounds)
    {
        if(sounds != null && sounds.Length > 0)
        {
            SoundSO soundSO = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(soundSO);
        }
    }

    private void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.Loop;

        if(soundSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        }

        PlaySound(soundSO.Clip, pitch, volume, loop);
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        if(!loop)
        {
            Destroy(soundObject, clip.length);
        }
    }

    private void Gun_OnShoot()
    {
        PlayRandomSound(_soundCollectionSO.GunShoot);
    }

    private void PlayerController_OnJump()
    {
        PlayRandomSound(_soundCollectionSO.Jump);
    }

    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(_soundCollectionSO.Splat);
    }
}
