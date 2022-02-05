using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float _INIT_SPEED = 100f;
    private const float _BASE_Y = 14f;

    public static GameManager instance;

    [HideInInspector] public float speed;
    [HideInInspector] public List<ObstacleManager> obstacles;

    [SerializeField] private GameObject _obstaclePrefab;

    private float _popMinWait;
    private float _popMaxWait;

    void Start()
    {
        if (instance == null)
            instance = this;

        _popMinWait = 3f;
        _popMaxWait = 6f;

        speed = _INIT_SPEED;
        obstacles = new List<ObstacleManager>();
        StartCoroutine(_PoppingObstacles());
    }

    private void Update()
    {
        speed = _INIT_SPEED + 2f * Time.time;

        if (Time.time > 10f)
        {
            _popMinWait = 1.5f;
            _popMaxWait = 4f;
        }
        else if (Time.time > 60f)
        {
            _popMinWait = 1f;
            _popMaxWait = 3f;
        }
    }

    private void OnEnable()
    {
        EventManager.AddListener("ObstacleDestroyed", _OnObstacleDestroyed);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("ObstacleDestroyed", _OnObstacleDestroyed);
    }

    private void _OnObstacleDestroyed()
    {
        if (Random.Range(0f, 1f) < 0.5f)
            _PopObstacle();
    }

    private IEnumerator _PoppingObstacles()
    {
        yield return new WaitForSeconds(1f);
        _PopObstacles();

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_popMinWait, _popMaxWait));
            _PopObstacles();
        }
    }

    private void _PopObstacles()
    {
        for (int i = 0; i < Random.Range(1, 4); i++)
            _PopObstacle();
    }

    private void _PopObstacle()
    {
        Vector3 p = new Vector3(
            Random.Range(-4f, 4f),
            _BASE_Y + Random.Range(1f, 4f),
            1f);
        GameObject o = Instantiate(_obstaclePrefab, p, Quaternion.identity);
        o.transform.localScale = new Vector3(
            Random.Range(0.6f, 1.2f),
            Random.Range(0.6f, 1.2f),
            Random.Range(0.6f, 1.2f));
        o.GetComponent<ObstacleManager>().SetRotation(new Vector3(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)));
    }
}
