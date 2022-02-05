using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Transform _trail;
    private TrailRenderer _trailRenderer;

    Camera _mainCamera;

    private bool _touching;
    private Touch _t;
    private Coroutine _trailingCoroutine;

    private Ray _ray;
    private RaycastHit _hit;
    private LayerMask _obstacleLayerMask;

    private void Start()
    {
        _obstacleLayerMask = LayerMask.GetMask("Obstacle");

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
                _trail.position = _GetWorldPoint(_t.position);
                _trailingCoroutine = StartCoroutine(_Trailing());
            }
            else if (_t.phase == TouchPhase.Moved)
            {
                _ray = _mainCamera.ScreenPointToRay(_t.position);
                if (Physics.Raycast(_ray, out _hit, 100f, _obstacleLayerMask))
                {
                    _hit.transform.GetComponent<ObstacleManager>().Die();
                }
            }
            else if (_t.phase == TouchPhase.Ended)
            {
                _trailRenderer.enabled = false;
                _touching = false;
                StopCoroutine(_trailingCoroutine);
            }
        }
    }

    private Vector3 _GetWorldPoint(Vector3 screenPoint)
    {
        Vector3 p = _mainCamera.ScreenToWorldPoint(screenPoint);
        p.z = 0f;
        return p;
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
