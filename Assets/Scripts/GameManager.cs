using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const float INVULNERABILITY_DELAY = 1f;
    private const float _INIT_SPEED = 100f;
    private const float _BASE_Y = 14f;

    public static GameManager instance;

    [HideInInspector] public float speed;
    [HideInInspector] public List<ObstacleManager> obstacles;
    [HideInInspector] public bool invulnerable;

    [SerializeField] private GameObject _obstaclePrefab;
    [SerializeField] private Material[] _obstacleMaterials;

    private float _popMinWait;
    private float _popMaxWait;

    void Start()
    {
        if (instance == null)
            instance = this;

        speed = _INIT_SPEED;
        obstacles = new List<ObstacleManager>();
        invulnerable = false;

        _popMinWait = 3f;
        _popMaxWait = 6f;

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
        EventManager.AddListener("GameOver", _OnGameOver);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("ObstacleDestroyed", _OnObstacleDestroyed);
        EventManager.RemoveListener("GameOver", _OnGameOver);
    }

    private void _OnObstacleDestroyed(object data)
    {
        if (Random.Range(0f, 1f) < 0.5f)
            _PopObstacle();
    }

    public void CheckInvulnerability()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            Invoke("_RemoveInvulnerability", INVULNERABILITY_DELAY);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        EventManager.TriggerEvent("PauseToggled", true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        EventManager.TriggerEvent("PauseToggled", false);
    }

    public void Replay()
    {
        EventManager.TriggerEvent("Reset");

        List<ObstacleManager> obs = new List<ObstacleManager>(obstacles);
        foreach (ObstacleManager om in obs)
            om.Die(direct: true);

        speed = _INIT_SPEED;
        invulnerable = false;

        _popMinWait = 3f;
        _popMaxWait = 6f;

        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneBooter.instance.LoadMenu();
    }

    private void _OnGameOver()
    {
        Time.timeScale = 0;
    }

    private IEnumerator _PoppingObstacles()
    {
        yield return new WaitForSeconds(3f);
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
        ObstacleManager om = o.GetComponent<ObstacleManager>();
        int level = 0;
        float r = Random.Range(0f, 1f);
        if (r < 0.28f) level = 1;
        else if (r < 0.34f) level = 2;
        om.Initialize(level, _obstacleMaterials[level], new Vector3(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            Random.Range(0f, 360f)));
    }

    private void _RemoveInvulnerability()
    {
        invulnerable = false;
    }
}
