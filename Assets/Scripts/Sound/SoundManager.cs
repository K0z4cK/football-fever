using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum SoundType
{
    None,
    Button,
    DragDrop,
    Lose,
    Win,
    Match,
    Coins
}

[System.Serializable]
public class SoundEffectInfo
{
    public SoundType type;
    public AudioClip clip;
}

public class SoundManager : SingletonComponent<SoundManager>
{
    [Header("Main Mixer")]
    [SerializeField] private AudioMixer _mixer;

    [Header("Sounds By Type")]
    [SerializeField] private List<SoundEffectInfo> _sounds = new List<SoundEffectInfo>();

    [Header("Pool info")]
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _prefab;

    private AudioSource music;
    //private List<MusicItem> _musicItems;

    private ObjectPool<GameObject> _pool;

    public void Init(PlayerData data, List<MusicItem> musicItems)
    {
        _pool = new ObjectPool<GameObject>(Create, Get, Release);

        music = GetComponent<AudioSource>();
        //_musicItems = musicItems;

        //ChangeVolume("Sound");
        //ChangeVolume("Music");
        
        music.clip = musicItems.Find(x => x.id == data.selectedMusic).clip;

        
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        Instance.ChangeVolume("Music");
        Instance.ChangeVolume("Sound");

        //PlayMusic();
    }

    public void ButtonSound() => MakeSound(SoundType.Button);

    public void MakeSound(SoundType type)
    {
        if (type == SoundType.None)
            return;

        var sound = _sounds.Find(x => x.type == type);

        var gameObject = _pool.Get();

        gameObject.GetComponent<SoundEffect>().SetInfo(sound.clip.length, delegate { _pool.Release(gameObject); });
        var audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = sound.clip;
        audioSource.Play();
    }

    public void ChangeVolume(string groupName)
    {
        if (PlayerPrefs.GetInt(groupName) == 1)
            _mixer.SetFloat(groupName, 0);
        if (PlayerPrefs.GetInt(groupName) == 0)
            _mixer.SetFloat(groupName, -80);

        print(groupName + " changed volume to " + _mixer.GetFloat(groupName, out float vol) + " " + vol);
    }

    public void SetMusicVolume(float value) => _mixer.SetFloat("Music", value);
    public float GetMusicVolume()
    {
        _mixer.GetFloat("Music", out float volume);
        return volume;
    }

    public void SetSoundVolume(float value) => _mixer.SetFloat("Sound", value);

    public void PlayMusic()
    {
        StartCoroutine(MusicCoroutine());
    }

    public void StopMusic()
    {
        StopCoroutine(MusicCoroutine());
        music.Stop();
    }

    public void ResetMusic()
    {
        StopMusic();
        PlayMusic();
    }

    public void ChangeMusic(AudioClip clip)
    {
        music.clip = clip;
    }

    private IEnumerator MusicCoroutine()
    {
        yield return new WaitUntil(() => !music.isPlaying);
        while (true)
        {
            //music.clip = ChangeMusic();
            music.Play();
            yield return new WaitUntil(() => !music.isPlaying);
        }
    }

    //
    //For Pool
    //
    private GameObject Create()
    {
        var sound = Instantiate(_prefab, _parent.transform);

        sound.SetActive(false);

        return sound;
    }

    private void Get(GameObject sound)
    {
        sound.SetActive(true);
    }

    private void Release(GameObject sound)
    {
        sound.SetActive(false);
    }
}