using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionSO _soundCollectionSO;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;

    private void Start() 
    {
        FightMusic();    
    }

    private void OnEnable() 
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent += DiscoBallMusic;
    }

    private void OnDisable() 
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent -= DiscoBallMusic;
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
        AudioMixerGroup audioMixerGroup;

        if(soundSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        }

        switch(soundSO.AudioType)
        {
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        PlaySound(soundSO.Clip, pitch, volume, loop, audioMixerGroup);
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if(!loop)
        {
            Destroy(soundObject, clip.length);
        }

        if(audioMixerGroup == _musicMixerGroup)
        {
            if(_currentMusic != null)
            {
                _currentMusic.Stop();
            }

            _currentMusic = audioSource;
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

    private void FightMusic()
    {
        PlayRandomSound(_soundCollectionSO.FightMusic);
    }

    private void DiscoBallMusic()
    {
        PlayRandomSound(_soundCollectionSO.DiscoBallMusic);
        float soundLength = _soundCollectionSO.DiscoBallMusic[0].Clip.length;
        Invoke("FightMusic", soundLength);
    }
}
