using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SoundManager : SingletonBehavior<SoundManager>
{
    /// <summary>
    /// fixme
    /// </summary>
    private readonly List<AudioSource> audioSources = new List<AudioSource>();
    private readonly HashSet<int> usingIndexs = new HashSet<int>();
    [SerializeField]
    private List<AudioClip> clipList;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayEffectSound(SOUND_NAME soundName, float volume = 1f, float pitch = 1f)
    {
        int emptyAudioIndex = -1;
        for (int i = 0; i < audioSources.Count; ++i)
        {
            if (!usingIndexs.Contains(i) && !audioSources[i].isPlaying)
            {
                emptyAudioIndex = i;
                usingIndexs.Add(emptyAudioIndex);
                break;
            }
        }
        // 만일 모든 AudioSource가 사용중일때
        if (emptyAudioIndex < 0)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
            emptyAudioIndex = audioSources.Count - 1;
        }

        var audioSourceToUse = audioSources[emptyAudioIndex];

        audioSourceToUse.clip = clipList[(int)soundName];
        audioSourceToUse.volume = volume;
        audioSourceToUse.pitch = pitch;
        if (soundName == SOUND_NAME.MAIN_BGM || soundName == SOUND_NAME.LEVEL_BGM || soundName == SOUND_NAME.STAGE_SELECT_BGM)
            audioSourceToUse.loop = true;
        audioSourceToUse.Play();
        usingIndexs.Remove(emptyAudioIndex);
    }

    public void PauseBGM(SOUND_NAME soundName)
    {
        AudioClip clip = clipList[(int)soundName];
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying && audioSources[i].clip == clip)
            {
                audioSources[i].Pause();
            }
        }
    }
    public void ChangeBGM(SOUND_NAME soundName, float volume = 1, float pitch = 1)
    {
        AudioClip clip = clipList[(int)soundName];
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying && audioSources[i].clip == clip)
            {
                audioSources[i].volume = volume;
                audioSources[i].pitch = pitch;
            }
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        int Scenenumber = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(Scenenumber);
        //main: 23, Stage Select: 24, Stage: 0~20
        switch (Scenenumber) {
            case 23:

                PauseBGM(SOUND_NAME.STAGE_SELECT_BGM);
                PlayEffectSound(SOUND_NAME.MAIN_BGM, 0.3f, 1f);
                break;
            case 24:

                PauseBGM(SOUND_NAME.LEVEL_BGM);
                PauseBGM(SOUND_NAME.MAIN_BGM);
                PlayEffectSound(SOUND_NAME.STAGE_SELECT_BGM, 1f, 1f);
                break;
            default:
                PauseBGM(SOUND_NAME.STAGE_SELECT_BGM);
                PauseBGM(SOUND_NAME.MAIN_BGM);
                PlayEffectSound(SOUND_NAME.LEVEL_BGM, 0.5f, 1f);
                break;
        }
    }

    private void OnDisabled()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public enum SOUND_NAME
{
    BUTTON_CLICK_SOUND,
    CHECK_SOUND,
    EQUIP_SOUND,
    CLEAR_SOUND,
    FAILED_SOUND,
    BREAK_SOUND,
    FALLING_SOUND,
    LASER_SOUND,

    MAIN_BGM,
    STAGE_SELECT_BGM,
    LEVEL_BGM
}