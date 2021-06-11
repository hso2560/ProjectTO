using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] gameSoundEffects;

    public void PlaySoundEffect(int idx, float volume)
    {
        if (volume <= 0) return;

        SoundPrefab sound = PoolManager.GetItem<SoundPrefab>();
        sound.SoundPlay(gameSoundEffects[idx], volume);
    }
}
