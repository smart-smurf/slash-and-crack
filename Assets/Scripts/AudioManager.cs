using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClipId
{
    PlayerHit = 0,
    Slash,
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private UnityEngine.UI.Image _audioToggleButtonImage;
    [SerializeField] private Sprite _audioSpriteOn;
    [SerializeField] private Sprite _audioSpriteOff;

    [SerializeField] private AudioSource _soundsSource;
    [SerializeField] private AudioClip[] _clips;

    private void Start()
    {
        if (instance == null)
            instance = this;

        SetMute(DataManager.instance.mute);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMute();
    }

    public void ToggleMute()
    {
        SetMute(1 - DataManager.instance.mute);
    }

    public void SetMute(int mute)
    {
        DataManager.instance.mute = mute;
        if (mute == 1)
        {
            _mixer.SetFloat("volume", -80f);
            _audioToggleButtonImage.sprite = _audioSpriteOff;
        }
        else
        {
            _mixer.SetFloat("volume", 0f);
            _audioToggleButtonImage.sprite = _audioSpriteOn;
        }
    }

    public void PlaySound(AudioClipId clipId)
    {
        if (!_soundsSource) return;
        _soundsSource.PlayOneShot(_clips[(int)clipId]);
    }

}
