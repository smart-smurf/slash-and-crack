using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private const float _STAR_UNIT_SIZE = 720f;

    [SerializeField] private Text _scoreText;
    [SerializeField] private RectTransform _starsParent;
    private float d;

    private int _score;

    private void Start()
    {
        d = _STAR_UNIT_SIZE;
        _starsParent.anchoredPosition = Vector2.up * d;

        _score = 0;
        _scoreText.text = _score.ToString();
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
    }

    private void OnDisable()
    {
        EventManager.RemoveListener("ObstacleDestroyed", _OnObstacleDestroyed);
    }

    private void _OnObstacleDestroyed()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }
}
