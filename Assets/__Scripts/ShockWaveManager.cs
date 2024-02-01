using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShokWave : MonoBehaviour
{
    [SerializeField] private float _shockWaveTime = 0.5f;
    
    private Coroutine _shockWaveCoroutine;

    private Material _material;

    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void CallShockWave()
    {
        _shockWaveCoroutine = StartCoroutine(ShockWaveCoroutine(-0.1f, 1f));
    }

    private IEnumerator ShockWaveCoroutine(float startPos, float endPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;

        float elapsedTime = 0f;

        while (elapsedTime < _shockWaveTime)
        {
            elapsedTime += Time.deltaTime;

            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / _shockWaveTime));
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }
    }

}
