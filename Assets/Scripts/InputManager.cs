using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float _MIN_SWIPE_DISTANCE = 1f;
    private const float _MAX_SWIPE_TIME = 1f;
    private const float _DIRECTION_THRESHOLD = 0.8f;

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
        float x1 = Mathf.Min(_startPos.x, _endPos.x);
        float x2 = Mathf.Max(_startPos.x, _endPos.x);
        float y1 = Mathf.Min(_startPos.y, _endPos.y);
        float y2 = Mathf.Max(_startPos.y, _endPos.y);

        float a = (_endPos.y - _startPos.y) / (_endPos.x - _startPos.x);
        float b = _startPos.y - a * _startPos.x;
        float scaleFactor = 1.5f;
        float yRef, x, y, xMin, xMax, yMin, yMax;
        List<ObstacleManager> obstacles =
            new List<ObstacleManager>(GameManager.instance.obstacles);
        foreach (ObstacleManager om in obstacles)
        {
            x = om.transform.position.x;
            y = om.transform.position.y;
            xMin = x - scaleFactor;
            xMax = x + scaleFactor;
            yMin = y - scaleFactor;
            yMax = y + scaleFactor;

            // check if obstacle is between swipe extrema points
            if (x1 <= x && x <= x2 && y1 <= y && y <= y2)
            {
                yRef = a * x + b;
                // check if swipe is going over obstacle
                if (yMin <= yRef && yRef <= yMax)
                {
                    om.Die();
                }
            }
        }
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
