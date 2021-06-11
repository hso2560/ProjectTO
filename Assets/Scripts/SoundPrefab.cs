using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrefab : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    private bool soundStart;

    public void SoundPlay(AudioClip clip, float volume)
    {   
        _audio.clip = clip;
        _audio.volume = volume;
        _audio.Play();
        soundStart = true;
    }

    private void OnEnable() => soundStart = false;

    private void Update()
    {
        if (!_audio.isPlaying && soundStart)
        {
            gameObject.SetActive(false);
        }
    }
}
