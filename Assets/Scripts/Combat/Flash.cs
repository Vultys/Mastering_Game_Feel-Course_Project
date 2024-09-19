using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = 0.1f;

    private SpriteRenderer[] _spriteRenderers;

    private Color[] _previousColors = new Color[2];

    private void Awake() 
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();    
    }

    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for(int i = 0; i <= _spriteRenderers.Length - 1; i++)
        {
            _spriteRenderers[i].material = _whiteFlashMaterial;
            _previousColors[i] = _spriteRenderers[i].color;
            _spriteRenderers[i].color = Color.white;
        }

        yield return new WaitForSeconds(_flashTime);

        SetDefaultMaterial();
    }

    private void SetDefaultMaterial()
    {
        for(int i = 0; i <= _spriteRenderers.Length - 1; i++)
        {
            _spriteRenderers[i].material = _defaultMaterial;
            _spriteRenderers[i].color = _previousColors[i];
        }
    }
}
