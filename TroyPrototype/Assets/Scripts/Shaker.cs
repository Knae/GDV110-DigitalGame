using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    Transform _target;
    Vector3 _initialPos;

    void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.position;
    }

    float _pendingShakeDuration = 0.0f;

    public void Shake (float duration)
    {
        if (duration > 0)
        {
            _pendingShakeDuration += duration;
        }
    }

    bool _isShaking = false;

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
            var randomPoint = new Vector3(Random.Range(-0.2f, 0.2f) + _initialPos.x, Random.Range(-0.2f, 0.2f) + _initialPos.y, _initialPos.z);
            _target.localPosition = randomPoint;
            yield return null;
        }

        _pendingShakeDuration = 0.0f;
        _target.position = _initialPos;
        _isShaking = false;
    }
}
