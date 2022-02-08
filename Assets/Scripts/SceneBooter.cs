using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneBooter : MonoBehaviour
{
    public static SceneBooter instance;

    private const float _TRANSITION_DELAY = 1f;
    [SerializeField] private UnityEngine.UI.Image _sceneTransitionerImage;

    void Start()
    {
        if (instance == null)
            instance = this;

        _LoadMenu();
        StartCoroutine(_FadingIn());
    }

    public void LoadMenu() => StartCoroutine(_SwitchingScene("menu"));
    public void LoadGame() => StartCoroutine(_SwitchingScene("game"));

    private IEnumerator _FadingIn()
    {
        _sceneTransitionerImage.color = Color.black;

        float t = 0;
        while (t < _TRANSITION_DELAY)
        {
            _sceneTransitionerImage.color = Color.Lerp(Color.black, Color.clear, t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        _sceneTransitionerImage.color = Color.clear;
    }

    private IEnumerator _SwitchingScene(string to)
    {
        _sceneTransitionerImage.color = Color.clear;

        float t = 0;
        while (t < _TRANSITION_DELAY)
        {
            _sceneTransitionerImage.color = Color.Lerp(Color.clear, Color.black, t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;

        AsyncOperation op;
        if (to == "menu")
            op = _LoadMenu();
        else
            op = _LoadGame();

        yield return new WaitUntil(() => op.isDone);

        t = 0;
        while (t < _TRANSITION_DELAY)
        {
            _sceneTransitionerImage.color = Color.Lerp(Color.black, Color.clear, t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        _sceneTransitionerImage.color = Color.clear;
    }

    private AsyncOperation _LoadGame()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        op.completed += (_) =>
        {
            Scene s = SceneManager.GetSceneByName("Menu");
            if (s != null && s.IsValid())
                SceneManager.UnloadSceneAsync(s);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        };
        return op;
    }

    private AsyncOperation _LoadMenu()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        op.completed += (_) =>
        {
            Scene s = SceneManager.GetSceneByName("Game");
            if (s != null && s.IsValid())
                SceneManager.UnloadSceneAsync(s);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Menu"));
        };
        return op;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
