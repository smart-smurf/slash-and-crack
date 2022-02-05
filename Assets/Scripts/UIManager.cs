using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private const int _INIT_LIVES = 4;
    private const float _STAR_UNIT_SIZE = 720f;

    [Header("Prefabs")]
    [SerializeField] private GameObject _lifePrefab;

    [Header("References")]
    [SerializeField] private GameObject _startText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Transform _livesParent;
    [SerializeField] private RectTransform _starsParent;
    private float d;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Image _invulnerabilityScreen;
    [SerializeField] private GameObject _pausePanel;

    private int _lives;
    private int _score;

    private void Start()
    {
        _OnReset(true);
    }

    void Update()
    {
        d -= GameManager.instance.speed * Time.deltaTime;
        _starsParent.anchoredPosition = Vector2.up * d;

        if (d <= 0)
        {
            _starsParent.GetChild(_starsParent.childCount - 1).SetAsFirstSibling();
            d = _STAR_UNIT_SIZE;
        }
    }

    private void OnEnable()
    {
        EventManager.AddListener("ObstacleDestroyed", _OnObstacleDestroyed);
        EventManager.AddListener("ObstaclePassed", _OnObstaclePassed);
        EventManager.AddListener("Reset", _OnReset);
        EventManager.AddListener("PauseToggled", _OnPauseToggled);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("ObstacleDestroyed", _OnObstacleDestroyed);
        EventManager.RemoveListener("ObstaclePassed", _OnObstaclePassed);
        EventManager.RemoveListener("Reset", _OnReset);
        EventManager.RemoveListener("PauseToggled", _OnPauseToggled);
    }

    private void _OnObstacleDestroyed()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void _OnObstaclePassed()
    {
        if (GameManager.instance.invulnerable) return;
        GameManager.instance.CheckInvulnerability();

        StartCoroutine(_FadingInvulnerabilityScreen());
        _lives--;
        if (_lives >= 0)
            _livesParent.GetChild(_lives).Find("Fill").gameObject.SetActive(false);

        if (_lives == 0)
            Invoke("_GameOver", 0.5f);
    }

    private void _OnReset() { _OnReset(false); }
    private void _OnReset(bool init)
    {
        d = _STAR_UNIT_SIZE;
        _starsParent.anchoredPosition = Vector2.up * d;

        _lives = _INIT_LIVES;
        if (init)
        {
            for (int i = 0; i < _INIT_LIVES; i++)
                Instantiate(_lifePrefab, _livesParent);
        }
        else
        {
            foreach (Transform child in _livesParent)
                child.Find("Fill").gameObject.SetActive(true);
        }

        _score = 0;
        _scoreText.text = _score.ToString();

        _gameOverPanel.SetActive(false);
        _invulnerabilityScreen.gameObject.SetActive(true);
        _invulnerabilityScreen.color = new Color(1f, 0f, 0f, 0f);
        _pausePanel.SetActive(false);

        StartCoroutine(_ShowingStartText());
    }

    private void _OnPauseToggled(object on)
    {
        _pausePanel.SetActive((bool) on);
    }

    private void _GameOver()
    {
        EventManager.TriggerEvent("GameOver");
        _gameOverPanel.SetActive(true);
    }

    private IEnumerator _ShowingStartText()
    {
        _startText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _startText.SetActive(true);
        yield return new WaitForSeconds(2f);
        _startText.SetActive(false);
    }

    private IEnumerator _FadingInvulnerabilityScreen()
    {
        Color start = new Color(1f, 0f, 0f, 0.5f);
        Color end = new Color(1f, 0f, 0f, 0f);

        _invulnerabilityScreen.color = start;

        float t = 0f;
        while (t < GameManager.INVULNERABILITY_DELAY)
        {
            _invulnerabilityScreen.color = Color.Lerp(start, end, t / GameManager.INVULNERABILITY_DELAY);
            t += Time.deltaTime;
            yield return null;
        }

        _invulnerabilityScreen.color = end;
    }
}
