using UnityEngine;

public class UIManager : MonoBehaviour
{
    private const float _STAR_UNIT_SIZE = 720f;

    [SerializeField] private RectTransform _starsParent;
    private float d;

    private void Start()
    {
        d = _STAR_UNIT_SIZE;
        _starsParent.anchoredPosition = Vector2.up * d;
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
}
