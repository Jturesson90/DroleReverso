using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private bool _isOn;
    public bool IsOn { get { return _isOn; } set { _isOn = value; ReversoPlayerPrefs.SetAudio(value); } }

    [SerializeField] private AudioClip[] _buttonClips;
    [SerializeField] private AudioClip[] _tileTurnedClips;
    [SerializeField] private AudioClip[] _wooshInClips;
    [SerializeField] private AudioClip[] _wooshOutClips;


    [SerializeField] private AudioSource _buttonAudioSource;
    [SerializeField] private AudioSource _tileTurnedAudioSource;
    [SerializeField] private AudioSource _wooshAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _isOn = ReversoPlayerPrefs.GetAudio();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTileTurned()
    {
        if (!_tileTurnedAudioSource) return;
        PlaySfx(_tileTurnedAudioSource, _tileTurnedClips.First());
    }

    public void OnButtonClick()
    {
        if (!_buttonAudioSource) return;
        PlaySfx(_buttonAudioSource, _buttonClips.First(), UnityEngine.Random.Range(0.99f, 1.03f));
    }

    public void OnWooshIn()
    {
        if (!_wooshAudioSource) return;
        PlaySfx(_wooshAudioSource, _wooshInClips.First());
    }

    public void OnWooshOut()
    {
        if (!_wooshAudioSource) return;
        PlaySfx(_wooshAudioSource, _wooshOutClips.First(), UnityEngine.Random.Range(0.95f, 1.05f));
    }

    private void PlaySfx(AudioSource audioSource, AudioClip clip, float pitch = 1f)
    {
        if (!IsOn) return;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);
    }
}
