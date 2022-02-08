using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform _logoShadow;
    [SerializeField] private RectTransform _logo;
    [SerializeField] private RectTransform _titleShadow;
    [SerializeField] private RectTransform _title;

    private void Start()
    {
        StartCoroutine(_BouncingRectTransformSize(_logoShadow, 2, 0.2f));
        StartCoroutine(_BouncingRectTransformSize(_logo, 2, 0.25f));
        StartCoroutine(_BouncingRectTransformSize(_titleShadow, 2, 0.4f));
        StartCoroutine(_BouncingRectTransformSize(_title, 2, 0.45f));
    }

    public void Play()
    {
        SceneBooter.instance.LoadGame();
    }

    public void QuitGame()
    {
        SceneBooter.instance.QuitGame();
    }

    private IEnumerator _BouncingRectTransformSize(
        RectTransform rectTransform, float delay, float wait = 0,
        float from = 0f, float to = 1f)
    {
        rectTransform.localScale = from * Vector3.one;

        yield return new WaitForSeconds(wait);

        float t = 0f;
        while (t < delay)
        {
            rectTransform.localScale = _BouncingEaseFunction(t / delay) * Vector3.one;
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.localScale = to * Vector3.one;
    }

    private float _BouncingEaseFunction(float t)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1)
        {
            return n1 * t * t;
        }
        else if (t < 2 / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        }
        else if (t < 2.5 / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        }
        else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }
}
