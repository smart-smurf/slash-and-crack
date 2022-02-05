using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneBooter.instance.LoadGame();
    }

    public void QuitGame()
    {
        SceneBooter.instance.QuitGame();
    }
}
