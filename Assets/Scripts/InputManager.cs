using System.Collections;
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

    private float _topUIThreshold;

    private void Start()
    {
        _obstacleLayerMask = LayerMask.GetMask("Obstacle");

        _topUIThreshold = Screen.height - 100;

        _mainCamera = Camera.main;
        _trailRenderer = _trail.GetComponent<TrailRenderer>();
        _trailingCoroutine = null;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.touches.Length > 0)
        {
            _t = Input.touches[0];
            if (_t.phase == TouchPhase.Began)
            {
                if (_t.position.y >= _topUIThreshold) return;

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
                    _hit.transform.GetComponent<ObstacleManager>().TakeHit();
                }
            }
            else if (_t.phase == TouchPhase.Ended)
            {
                _trailRenderer.enabled = false;
                _touching = false;
                if (_trailingCoroutine != null)
                {
                    StopCoroutine(_trailingCoroutine);
                    _trailingCoroutine = null;
                }
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
