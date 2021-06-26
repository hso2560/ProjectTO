
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioClip[] gameSoundEffects;
    private GameManager manager;

    private void Start()
    {
        manager = GameManager.Instance;
    }

    public void PlaySoundEffect(int idx)
    {
        if (manager.savedData.option.soundEffect <= 0) return;

        SoundPrefab sound = PoolManager.GetItem<SoundPrefab>();
        sound.SoundPlay(gameSoundEffects[idx]);
    }
}
