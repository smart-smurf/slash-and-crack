using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIAnimations
{
    public enum AnimationEasing
    {
        None,
        BounceOut,
    }

    private static Dictionary<AnimationEasing, System.Func<float, float>> _EASING_FUNCS
        = new Dictionary<AnimationEasing, System.Func<float, float>>()
    {
    { AnimationEasing.None, (float t) => t },
    { AnimationEasing.BounceOut, (float t) => {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (t < 1 / d1) {
            return n1 * t * t;
        } else if (t < 2 / d1) {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        } else if (t < 2.5 / d1) {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        } else {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    } },
    };

    public static IEnumerator ScaleUIElement(
        RectTransform rect,
        float from = 0f,
        float to = 1f,
        float duration = 1f,
        float wait = 0f,
        AnimationEasing easing = AnimationEasing.None)
    {
        rect.localScale = new Vector3(from, from, 1);

        if (wait > 0)
            yield return new WaitForSeconds(wait);

        float t = 0f;
        float x;
        System.Func<float, float> f = _EASING_FUNCS[easing];
        while (t < duration)
        {
            x = f(Mathf.Lerp(from, to, t / duration));
            rect.localScale = new Vector3(x, x, 1);
            t += Time.deltaTime;
            yield return null;
        }

        rect.localScale = new Vector3(to, to, 1);
    }

}
