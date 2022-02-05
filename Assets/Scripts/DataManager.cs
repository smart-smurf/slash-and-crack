using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private static readonly string SAVE_PREFIX = "__slash-and-crash";

    private static readonly string SAVE_KEY_OPTION_MUTE =
        $"{SAVE_PREFIX}__option_mute";

    [HideInInspector] public int mute;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _LoadData();
    }

    private void _LoadData()
    {
        // use default (fallback) values if none are loaded
        mute = PlayerPrefs.HasKey(SAVE_KEY_OPTION_MUTE)
            ? PlayerPrefs.GetInt(SAVE_KEY_OPTION_MUTE)
            : 0;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(SAVE_KEY_OPTION_MUTE, mute);

        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        SaveData();
    }
#endif
}
