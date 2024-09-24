using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    [SerializeField] private float _discoBallPartyTime = 2f;
    [SerializeField] private float _discoGlobalLightIntensity = 0.2f;
    [SerializeField] private Light2D _globalLight;

    public static Action OnDiscoBallHitEvent;

    private Coroutine _discoCoroutine;

    private float _defaultGlobalLightIntensity;

    private ColorSpotlight[] _allSpotlights;

    private void Start()
    {
        _defaultGlobalLightIntensity = _globalLight.intensity;
        _allSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }

    private void OnEnable() 
    {
        OnDiscoBallHitEvent += DimTheLights;
    }

    private void OnDisable() 
    {
        OnDiscoBallHitEvent -= DimTheLights;
    }

    public void DiscoParty()
    {
        if(_discoCoroutine != null) return;
        
        OnDiscoBallHitEvent?.Invoke();
    }

    private void DimTheLights()
    {
        foreach(var spotlight in _allSpotlights)
        {
            StartCoroutine(spotlight.SpotLightDiscoParty(_discoBallPartyTime));
        }

        _discoCoroutine = StartCoroutine(GlobalLightResetRoutine());
    }

    private IEnumerator GlobalLightResetRoutine()
    {
        _globalLight.intensity = _discoGlobalLightIntensity;
        yield return new WaitForSeconds(_discoBallPartyTime);
        _globalLight.intensity = _defaultGlobalLightIntensity;
        _discoCoroutine = null;
    }
}
