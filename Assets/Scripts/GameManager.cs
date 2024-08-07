using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {None, Menu, Game, Team }

public class GameManager : MonoBehaviour
{
    private const string JsonFileName = "/PlayerData.json";
    private string jsonFilePath;

    [SerializeField]
    private GameObject _loadingScreen;

    [SerializeField]
    private BackgroundsSO _backgroundsSO;
    [SerializeField]
    private MusicsSO _musicSO;

    [SerializeField]
    private TMP_Text _coinsCount;

    [SerializeField]
    private TeamPanel _teamPanel;
    [SerializeField]
    private CardBattle _battle;
    [SerializeField]
    private Shop _shop;
    [SerializeField]
    private Settings _settings;
    [SerializeField]
    private Wheel _fortuneWheel;
    [SerializeField]
    private Tournament _tournament;
    [SerializeField]
    private Button _tournamentBtn;

    [SerializeField]
    private int _startTeamCount = 4;

    private GameState _state;

    //private List<FootballerData> _footballerDatas = new List<FootballerData>();
    private List<BackgroundItem> _backgrounds = new List<BackgroundItem>();
    private List<MusicItem> _music = new List<MusicItem>();

    private PlayerData _playerData = new PlayerData();

    private DateTime _currentDate;

    private void Awake()
    {
        jsonFilePath = Application.persistentDataPath + JsonFileName;
        _currentDate = DateTime.Now;
        _playerData.coins = 0;
        /*SetNewTeam();
        AddRandomTeam(5);*/
        LoadPlayerData();

        SoundManager.Instance.Init(_playerData, _music);
        _tournamentBtn.interactable = false;
        _battle.Init(AddCoins, AddCoins, RemoveFootballers);
        _shop.Init(_playerData, PurchaseFootballer, PurchaseBackground, PurchaseMusic);
        _settings.Init(_playerData, _backgrounds, _music);
        _fortuneWheel.Init(SetLastTime ,AddCoins, AddFootballer);
        _tournament.Init(RemoveCoins, AddCoins, AddCoins, RemoveFootballers);

        SetCoins();
        CheckTeamCount();

        Invoke(nameof(HideLoadingScreen), 3f);
    }

    private void HideLoadingScreen()
    { 
        _loadingScreen.gameObject.SetActive(false);
        SoundManager.Instance.PlayMusic();
    }

    private void SavePlayerData()
    {
        //_backgrounds.ForEach(x => _playerData.backgrounds.Add(x.id));
        //_music.ForEach(x => _playerData.music.Add(x.id));

        string json = JsonUtility.ToJson(_playerData);

        File.WriteAllText(jsonFilePath, json);

        print("data saved");
    }

    private void LoadPlayerData()
    {
        if (!File.Exists(jsonFilePath))
        {
            SetNewPlayerData();
            return;
        }

        string json = File.ReadAllText(jsonFilePath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
        if (playerData == null)
        {
            SetNewPlayerData();
            return;
        }

        _playerData = playerData;

        if (_playerData.lastPlayWheelOfFortune != null)
        {
            DateTime dateTimeUpdate = _playerData.lastPlayWheelOfFortune;
            dateTimeUpdate = dateTimeUpdate.AddHours(24);

            if (_currentDate >= dateTimeUpdate)
                _fortuneWheel.SetSpinAbility(true);
            else
                _fortuneWheel.SetSpinAbility(false);

            StartCoroutine(RealtimeTimer());
        }

        _backgrounds.AddRange(_backgroundsSO.backgroundItems.FindAll(x => _playerData.backgrounds.Contains(x.id)));
        _music.AddRange(_musicSO.musicItems.FindAll(x => _playerData.music.Contains(x.id)));

        SetTeamFromData();
        print("data load");
        // backgrounds
        //musics

    }

    private void SetNewPlayerData()
    {
        print("create new team");
        _playerData.backgrounds.Add("base");
        _playerData.music.Add("base");

        _backgrounds.Add(_backgroundsSO.backgroundItems.Find(x => x.id == "base"));
        _music.Add(_musicSO.musicItems.Find(x => x.id == "base"));

        _playerData.selectedBackground = "base";
        _playerData.selectedMusic = "base";

        _fortuneWheel.SetSpinAbility(true);

        if (!PlayerPrefs.HasKey("Sound"))
            PlayerPrefs.SetInt("Sound", 0);
        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 0);

        //_backgrounds.AddRange(_backgroundsSO.backgroundItems.FindAll(x => _playerData.backgrounds.Contains(x.id)));
        //_music.AddRange(_musicSO.musicItems.FindAll(x => _playerData.music.Contains(x.id)));

        SetNewTeam();
    }

    private void AddFootballer(FootballerItem footballerItem)
    {
        Footballer footballer = new Footballer();
        footballer = new Footballer(footballerItem.name, footballerItem.stats, footballerItem.sprite, footballerItem.points);

        if (footballer.IsStatsZero())
            footballer.SetStatsRandom();
        
        _teamPanel.AddFootballer(footballer);

        FootballerData data = new FootballerData()
        {
            id = footballer.Id,
            name = footballer.Name,
            stats = footballer.Stats,
            points = footballer.Points,
            sprite = footballer.Sprite
        };

        _playerData.footballers.Add(data);
        CheckTeamCount();
    }

    private void AddCoins(int coins)
    {
        _playerData.coins += coins;
        SetCoins();
        SoundManager.Instance.MakeSound(SoundType.Coins);
    }

    private void RemoveCoins(int coins)
    {
        _playerData.coins -= coins;
        SetCoins();
    }

    private void RemoveFootballers(List<Footballer> footballers)
    {
        foreach (var footballer in footballers)
            _playerData.footballers.Remove(_playerData.footballers.Find(x=> x.id == footballer.Id));
        
        _teamPanel.RemoveFootballers(footballers);
        CheckTeamCount();
    }

    private void SetNewTeam()
    {
        List<Footballer> footballers = new List<Footballer>();
        for (int i = 0; i < _startTeamCount; i ++)
        {
            Footballer newFootballer = new Footballer();
            //newFootballer.SetStatsRandom();
            footballers.Add(newFootballer);

            FootballerData data = new FootballerData()
            {
                id = newFootballer.Id,
                name = newFootballer.Name,
                stats = newFootballer.Stats,
                points = newFootballer.Points,
                sprite = newFootballer.Sprite
            };

            _playerData.footballers.Add(data);
        }

        _teamPanel.Init(footballers);
    }

    private void SetTeamFromData()
    {
        List<Footballer> footballers = new List<Footballer>();
        foreach(var data in _playerData.footballers)
        {
            Footballer newFootballer = new Footballer(data);
            footballers.Add(newFootballer);
        }
        _teamPanel.Init(footballers);
    }

    private void AddRandomTeam(int count)
    {
        List<Footballer> footballers = new List<Footballer>();
        for (int i = 0; i < count; i++)
        {
            Footballer newFootballer = new Footballer();
            newFootballer.SetStatsRandom();

            FootballerData data = new FootballerData()
            {
                id = newFootballer.Id,
                name = newFootballer.Name,
                stats = newFootballer.Stats,
                points = newFootballer.Points,
                sprite = newFootballer.Sprite
            };

            _playerData.footballers.Add(data);

            footballers.Add(newFootballer);
        }

        _teamPanel.AddFootballers(footballers);
    }

    private void PurchaseFootballer(int coins, ShopFootballerTemplate footballer)
    {
        if(CheckCoins(coins))
        {
            _teamPanel.AddFootballer(new Footballer(footballer.Footballer));
            _shop.ShowPurchasePopup("Footballer", footballer.Footballer.Sprite);

            FootballerData data = new FootballerData()
            {
                id = footballer.Footballer.Id,
                name = footballer.Footballer.Name,
                stats = footballer.Footballer.Stats,
                points = footballer.Footballer.Points,
                sprite = footballer.Footballer.Sprite
            };

            _playerData.footballers.Add(data);

            if (footballer.IsUnique)
                Destroy(footballer.gameObject);
            footballer.Refresh();
            CheckTeamCount();
        }
    }

    private void PurchaseBackground(int coins, ShopBackgroundTemplate background )
    {
        if (CheckCoins(coins))
        {
            _backgrounds.Add(background.Background);
            _playerData.backgrounds.Add(background.Background.id);

            _shop.ShowPurchasePopup("Background", background.Background.sprite);

            Destroy(background.gameObject);
        }
    }

    private void PurchaseMusic(int coins, ShopMusicTemplate music)
    {
        if (CheckCoins(coins))
        {
            _music.Add(music.Music);
            _playerData.music.Add(music.Music.id);

            _shop.ShowPurchasePopup("Music", music.Music.sprite);

            Destroy(music.gameObject);
        }
    }

    private bool CheckCoins(int coins)
    {
        if (_playerData.coins < coins)
        {
            _shop.ShowMoneyPopup();
            return false;
        }
        _playerData.coins -= coins;
        SetCoins();

        return true;
    }

    private void SetCoins()
    {
        _coinsCount.text = _playerData.coins.ToString();
        _shop.SetCoins(_playerData.coins);
        _settings.SetCoins(_playerData.coins);
    }

    private void OnDisable()
    {
        SavePlayerData();
    }

    private void SetLastTime()
    {
        _playerData.lastPlayWheelOfFortune = DateTime.Now;
        StartCoroutine(RealtimeTimer());
    }

    private IEnumerator RealtimeTimer()
    {
        DateTime dateTimeUpdate = _playerData.lastPlayWheelOfFortune;
        dateTimeUpdate = dateTimeUpdate.AddHours(24);

        var awaiter = new WaitForSeconds(1f);
        while (_currentDate < dateTimeUpdate)
        {
            yield return awaiter;
            _currentDate = DateTime.Now;

            _fortuneWheel.SetTimer(dateTimeUpdate - DateTime.Now);

            if (_currentDate >= dateTimeUpdate)
                _fortuneWheel.SetSpinAbility(true);
        }
    }

    private void CheckTeamCount()
    {
        if(_playerData.footballers.Count >= 8)
            _tournamentBtn.interactable = true;
        else
            _tournamentBtn.interactable = false;
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            AddCoins(500);
    }*/
}

[Serializable]
public class PlayerData 
{
    public int coins;

    public List<FootballerData> footballers = new List<FootballerData>();
    public List<string> backgrounds = new List<string>();
    public List<string> music = new List<string>();
    public string selectedBackground;
    public string selectedMusic;
    public JsonDateTime lastPlayWheelOfFortune;

}

[Serializable]
public class JsonDateTime
{
    public long value;
    public static implicit operator DateTime(JsonDateTime jdt)
    {
        Debug.Log("Converted to time");
        return DateTime.FromFileTimeUtc(jdt.value);
    }
    public static implicit operator JsonDateTime(DateTime dt)
    {
        Debug.Log("Converted to JDT");
        JsonDateTime jdt = new JsonDateTime();
        jdt.value = dt.ToFileTimeUtc();
        return jdt;
    }
}