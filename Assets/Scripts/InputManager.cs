using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float _MIN_SWIPE_DISTANCE = 1f;
    private const float _MAX_SWIPE_TIME = 1f;
    private const float _DIRECTION_THRESHOLD = 0.9f;

    [SerializeField] private Transform _trail;
    private TrailRenderer _trailRenderer;

    Camera _mainCamera;

    private bool _touching;
    private Touch _t;
    private Vector3 _startPos, _endPos;
    private float _startTime, _endTime;
    private Coroutine _trailingCoroutine;

    private void Start()
    {
        _mainCamera = Camera.main;
        _trailRenderer = _trail.GetComponent<TrailRenderer>();
    }

    void Update()
    {
        if (Input.touches.Length > 0)
        {
            _t = Input.touches[0];
            if (_t.phase == TouchPhase.Began)
            {
                _trailRenderer.enabled = true;
                _touching = true;
                _startPos = _GetWorldPoint(_t.position);
                _startTime = Time.time;
                _trail.position = _startPos;
                _trailingCoroutine = StartCoroutine(_Trailing());
            }
            else if (_t.phase == TouchPhase.Ended)
            {
                _trailRenderer.enabled = false;
                _touching = false;
                _endPos = _GetWorldPoint(_t.position);
                _endTime = Time.time;
                StopCoroutine(_trailingCoroutine);
                if (
                    Vector3.Distance(_startPos, _endPos) >= _MIN_SWIPE_DISTANCE &&
                    (_endTime - _startTime) <= _MAX_SWIPE_TIME
                ) _GetSwipe();
            }
        }
    }

    private Vector3 _GetWorldPoint(Vector3 screenPoint)
    {
        Vector3 p = _mainCamera.ScreenToWorldPoint(screenPoint);
        p.z = 0f;
        return p;
    }

    private void _GetSwipe()
    {
    }

    private IEnumerator _Trailing()
    {
        while (true)
        {
            if (_touching)
                _trail.position = _GetWorldPoint(_t.position);
            yield return null;
        }
    }
}
