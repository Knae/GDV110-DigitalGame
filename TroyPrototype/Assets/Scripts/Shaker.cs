using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Transform _target;
    Vector3 _initialPos;
    float _maxShakeDuration = 1.5f;
    float _ShakeRange = 0.15f;
    float _pendingShakeDuration = 0.0f;
    public bool _isShaking = false;
    public Vector3 randomPoint;

    void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.position;
    }

    public void Shake (float duration)
    {
        if (duration > 0 && duration <= (_maxShakeDuration-duration))
        {
            _pendingShakeDuration += duration;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_pendingShakeDuration > 0 && !_isShaking)
        {
            StartCoroutine(DoShake());
        }

        if (!_isShaking)
        {
            _initialPos = _target.position;
        }
    }

    IEnumerator DoShake()
    {
        _isShaking = true;

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + _pendingShakeDuration)
        {
            randomPoint = new Vector3(Random.Range(-_ShakeRange, _ShakeRange)/* + _initialPos.x*/, Random.Range(-_ShakeRange, _ShakeRange) /*+ _initialPos.y*/, 0.0f);
           // _target.localPosition = randomPoint;
            yield return null;
        }

        _pendingShakeDuration = 0.0f;
        //_target.position = _initialPos;
        _isShaking = false;
    }
}
