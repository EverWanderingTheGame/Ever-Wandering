using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private float _dissolveTime = 1f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _material;

    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _verticalDissolveAmount = Shader.PropertyToID("_VerticalDissolve");

    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _material = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _material[i] = _spriteRenderers[i].material;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            StartCoroutine(Vanish(true, false));
        }
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine(Appear(true, false));
        }
    }

    private IEnumerator Vanish(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(0f, 1.1f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(0f, 1.1f, (elapsedTime / _dissolveTime));

            for (int i = 0; i < _material.Length; i++)
            {
                if (useDissolve)
                {
                    _material[i].SetFloat(_dissolveAmount, lerpedDissolve);
                }

                if (useVertical)
                {
                    _material[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
                }
            }

            yield return null;
        }
    }

    private IEnumerator Appear(bool useDissolve, bool useVertical)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));
            float lerpedVerticalDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));

            for (int i = 0; i < _material.Length; i++)
            {
                if (useDissolve)
                {
                    _material[i].SetFloat(_dissolveAmount, lerpedDissolve);
                }

                if (useVertical)
                {
                    _material[i].SetFloat(_verticalDissolveAmount, lerpedVerticalDissolve);
                }
            }

            yield return null;
        }
    }
}
