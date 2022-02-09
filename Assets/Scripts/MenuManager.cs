using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform _logoShadow;
    [SerializeField] private RectTransform _logo;
    [SerializeField] private RectTransform _titleShadow;
    [SerializeField] private RectTransform _title;

    private void Start()
    {
        StartCoroutine(UIAnimations.ScaleUIElement(
            _logoShadow,
            duration: 2,
            wait: 0.2f,
            easing: UIAnimations.AnimationEasing.BounceOut));
        StartCoroutine(UIAnimations.ScaleUIElement(
            _logo,
            duration: 2,
            wait: 0.25f,
            easing: UIAnimations.AnimationEasing.BounceOut));
        StartCoroutine(UIAnimations.ScaleUIElement(
            _titleShadow,
            duration: 2,
            wait: 0.4f,
            easing: UIAnimations.AnimationEasing.BounceOut));
        StartCoroutine(UIAnimations.ScaleUIElement(
            _title,
            duration: 2,
            wait: 0.45f,
            easing: UIAnimations.AnimationEasing.BounceOut));
    }

    public void Play()
    {
        SceneBooter.instance.LoadGame();
    }

    public void QuitGame()
    {
        SceneBooter.instance.QuitGame();
    }
}
