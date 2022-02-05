using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float _INIT_SPEED = 60f;
    private const float _BASE_Y = 14f;

    public static GameManager instance;

    [HideInInspector] public float speed;
    [HideInInspector] public List<ObstacleManager> obstacles;

    [SerializeField] private GameObject _obstaclePrefab;

    void Start()
    {
        if (instance == null)
            instance = this;

        speed = _INIT_SPEED;
        obstacles = new List<ObstacleManager>();
        StartCoroutine(_PoppingObstacles());
    }

    private void Update()
    {
        speed = _INIT_SPEED + 2f * Time.time;
    }

    private IEnumerator _PoppingObstacles()
    {
        yield return new WaitForSeconds(1f);
        _PopObstacles();

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));
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
            Random.Range(-5f, 5f),
            _BASE_Y + Random.Range(2f, 6f),
            1f);
        GameObject o = Instantiate(_obstaclePrefab, p, Quaternion.identity);
    }
}
