using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum GameState {None, Menu, Game, Team }

public class GameManager : MonoBehaviour
{
    private const string JsonFileName = "/PlayerData.json";
    private string jsonFilePath;

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
    private int _startTeamCount = 4;

    private GameState _state;

    //private List<FootballerData> _footballerDatas = new List<FootballerData>();
    private List<BackgroundItem> _backgrounds = new List<BackgroundItem>();
    private List<MusicItem> _music = new List<MusicItem>();

    private PlayerData _playerData = new PlayerData();

    private void Awake()
    {
        jsonFilePath = Application.persistentDataPath + JsonFileName;
        _playerData.coins = 4444;
        /*SetNewTeam();
        AddRandomTeam(5);*/
        LoadPlayerData();

        _battle.Init(AddCoins, AddCoins, RemoveFootballers);
        _shop.Init(_playerData, PurchaseFootballer, PurchaseBackground, PurchaseMusic);
        _settings.Init(_playerData, _backgrounds, _music);
        SetCoins();
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

        //_backgrounds.AddRange(_backgroundsSO.backgroundItems.FindAll(x => _playerData.backgrounds.Contains(x.id)));
        //_music.AddRange(_musicSO.musicItems.FindAll(x => _playerData.music.Contains(x.id)));

        SetNewTeam();
    }

    private void AddCoins(int coins)
    {
        _playerData.coins += coins;
        SetCoins();
    }

    private void RemoveFootballers(List<Footballer> footballers)
    {
        foreach (var footballer in footballers)
            _playerData.footballers.Remove(_playerData.footballers.Find(x=> x.id == footballer.Id));
        
        _teamPanel.RemoveFootballers(footballers);
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
}