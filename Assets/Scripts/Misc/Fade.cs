using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Fade : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private float _fadeTime;

    private Image _fader;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake() 
    {
        _fader = GetComponent<Image>();
        _virtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();    
    }

    public void FadeInAndOut()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeRoutine(1f));
        RespawnPlayer();
        FadeOut();
    }

    private void FadeOut()
    {
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float elapsedTime = 0f;
        float startValue = _fader.color.a;

        while(elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAplha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);
            _fader.color = new Color(_fader.color.r, _fader.color.g, _fader.color.b, newAplha);
            yield return null;
        }

        _fader.color = new Color(_fader.color.r, _fader.color.g, _fader.color.b, targetAlpha);
    }

    private void RespawnPlayer()
    {
        GameObject player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        _virtualCamera.Follow = player.transform;
    }
}
