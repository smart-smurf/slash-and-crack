using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private const float _COMBO_MAX_TIME = 1f;

    [SerializeField] private Transform _trail;
    private TrailRenderer _trailRenderer;

    Camera _mainCamera;

    private bool _touching;
    private Vector2 _touchPos;
    private Touch _t;
    private Coroutine _trailingCoroutine;

    private Ray _ray;
    private RaycastHit _hit;
    private LayerMask _obstacleLayerMask;

    private float _topUIThreshold;

    private float _comboStartTime;
    private Coroutine _comboResetCoroutine;

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
                _PointerDown(_t.position);
            else if (_t.phase == TouchPhase.Moved)
                _PointerMove(_t.position);
            else if (_t.phase == TouchPhase.Ended)
                _PointerUp();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                _PointerDown(Input.mousePosition);
            else if (Input.GetMouseButton(0))
                _PointerMove(Input.mousePosition);
            else if (Input.GetMouseButtonUp(0))
                _PointerUp();
        }
    }

    private void _PointerDown(Vector2 pos)
    {
        if (pos.y >= _topUIThreshold) return;

        _trailRenderer.enabled = true;
        _touching = true;
        _comboStartTime = -1f;
        _touchPos = pos;
        _trail.position = _GetWorldPoint(pos);
        _trailingCoroutine = StartCoroutine(_Trailing());
    }

    private void _PointerMove(Vector2 pos)
    {
        _touchPos = pos;
        _ray = _mainCamera.ScreenPointToRay(pos);
        if (Physics.Raycast(_ray, out _hit, 100f, _obstacleLayerMask))
        {
            (bool hit, bool destroyed) =
                _hit.transform.GetComponent<ObstacleManager>().TakeHit();
            if (hit)
            {
                if (
                    destroyed &&
                    _comboStartTime != -1f &&
                    Time.time - _comboStartTime <= _COMBO_MAX_TIME
                )
                {
                    EventManager.TriggerEvent("IncreasedCombo");
                    if (_comboResetCoroutine != null)
                        StopCoroutine(_comboResetCoroutine);
                    _comboResetCoroutine = StartCoroutine(_ResettingCombo());
                }
                _comboStartTime = Time.time;
            }
        }
    }

    private void _PointerUp()
    {
        _trailRenderer.enabled = false;
        _touching = false;
        _comboStartTime = -1f;
        if (_trailingCoroutine != null)
        {
            StopCoroutine(_trailingCoroutine);
            _trailingCoroutine = null;
        }
    }

    private Vector3 _GetWorldPoint(Vector2 screenPoint)
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
                _trail.position = _GetWorldPoint(_touchPos);
            yield return null;
        }
    }

    private IEnumerator _ResettingCombo()
    {
        yield return new WaitForSeconds(_COMBO_MAX_TIME);
        EventManager.TriggerEvent("ResetCombo");
    }
}
