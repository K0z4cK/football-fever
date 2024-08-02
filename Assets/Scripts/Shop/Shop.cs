using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ShopType {Footballers, Backgrounds, Music }
public class Shop : MonoBehaviour
{
    private UnityAction<int, ShopFootballerTemplate> onPurchaseFootballer;
    private UnityAction<int, ShopBackgroundTemplate> onPurchaseBackground;
    private UnityAction<int, ShopMusicTemplate> onPurchaseMusic;

    [SerializeField]
    private TMP_Text _coins;

    [Header("Scriptable Objects")]
    [SerializeField]
    private FootballersSO _footballersSO;
    [SerializeField]
    private BackgroundsSO _backgroundsSO;
    [SerializeField]
    private MusicsSO _musicSO;

    [Header("Prefabs")]
    [SerializeField]
    private ShopFootballerTemplate _shopFootballerPrefab;
    [SerializeField]
    private ShopBackgroundTemplate _shopBackgroundPrefab;
    [SerializeField]
    private ShopMusicTemplate _shopMusicPrefab;

    [Header("Contents")]
    [SerializeField]
    private Transform _footballersContent;
    [SerializeField]
    private Transform _backgroundsContent;
    [SerializeField]
    private Transform _musicContent;

    [Header("Tabs")]
    [SerializeField]
    private List<ShopTab> _shopTabs;
    private ShopTab _selectedTab;

    [Header("Popups")]
    [SerializeField]
    private GameObject _purchasePopup;
    [SerializeField]
    private TMP_Text _purchaseText;
    [SerializeField]
    private Image _purchaseImage;

    [SerializeField]
    private GameObject _moneyPopup;

    private PlayerData _playerData;

    private void Awake()
    {
        //Init();
    }

    public void Init(PlayerData playerData ,UnityAction<int, ShopFootballerTemplate> purchaseFootballer, UnityAction<int, ShopBackgroundTemplate> purchaseBackground, UnityAction<int, ShopMusicTemplate> purchaseMusic)
    {
        _playerData = playerData;

        onPurchaseFootballer = purchaseFootballer;
        onPurchaseBackground = purchaseBackground; 
        onPurchaseMusic = purchaseMusic;

        foreach (var tab in _shopTabs)
        {
            tab.Init(SelectTab);
        }
        if (_selectedTab == null)
            _shopTabs[0].Select();
    }

    private void SelectTab(ShopTab tab)
    {
        _selectedTab = tab;
        _shopTabs.ForEach(x => { 
            if (_selectedTab != x) 
                x.Deselect(); 
        });
        InitContent(_selectedTab.ShopType);
    }

    private void InitContent(ShopType type)
    {
        _footballersContent.gameObject.SetActive(false);
        _backgroundsContent.gameObject.SetActive(false);
        _musicContent.gameObject.SetActive(false);

        switch (type)
        {
            case ShopType.Footballers:
                SetFootballers();
                break;
            case ShopType.Backgrounds:
                SetBackgrounds();
                break;
            case ShopType.Music:
                SetMusic();
                break;
        }

    }

    private void SetFootballers()
    {
        _footballersContent.gameObject.SetActive(true);

        if (_footballersSO.footballerItems.Count != _footballersContent.childCount)
        {
            foreach (Transform child in _footballersContent)
                Destroy(child.gameObject); 
           
            foreach (var footballer in _footballersSO.footballerItems)
            {
                if(footballer.isUnique)
                {
                    if(_playerData.footballers.Exists(x=> x.id == footballer.id))
                        continue;
                }
                var newTemplate = Instantiate(_shopFootballerPrefab, _footballersContent);
                newTemplate.Init(footballer.price, footballer, OnBuyFootballer);
            }
        }
    }

    private void SetBackgrounds()
    {
        _backgroundsContent.gameObject.SetActive(true);

        if (_backgroundsSO.backgroundItems.Count != _backgroundsContent.childCount)
        {
            foreach (Transform child in _backgroundsContent)
                Destroy(child.gameObject);

            foreach (var background in _backgroundsSO.backgroundItems)
            {
                if (_playerData.backgrounds.Exists(x => x == background.id) || background.id == "base")
                    continue;
                var newTemplate = Instantiate(_shopBackgroundPrefab, _backgroundsContent);
                newTemplate.Init(background.price, background, OnBuyBackground);
            }
        }
    }

    private void SetMusic()
    {
        _musicContent.gameObject.SetActive(true);

        if (_musicSO.musicItems.Count != _musicContent.childCount)
        {
            foreach (Transform child in _musicContent)
                Destroy(child.gameObject);

            foreach (var music in _musicSO.musicItems)
            {
                if (_playerData.music.Exists(x => x == music.id) || music.id == "base")
                    continue;
                var newTemplate = Instantiate(_shopMusicPrefab, _musicContent);
                newTemplate.Init(music.price, music, OnBuyMusic);
            }
        }
    }

    private void OnBuyFootballer(int price, ShopFootballerTemplate template)
    {
        onPurchaseFootballer?.Invoke(price, template);
    }

    private void OnBuyBackground(int price, ShopBackgroundTemplate template)
    {
        onPurchaseBackground?.Invoke(price, template);
    }

    private void OnBuyMusic(int price, ShopMusicTemplate template)
    {
        onPurchaseMusic?.Invoke(price, template);
    }

    public void ShowPurchasePopup(string name, Sprite sprite)
    {
        _purchaseText.text = $"You have purchased new {name}";
        _purchaseImage.sprite = sprite;
        _purchasePopup.SetActive(true);
    }

    public void ShowMoneyPopup()
    {
        _moneyPopup.SetActive(true);
    }

    public void SetCoins(int coins) => _coins.text = coins.ToString();

    public void UpdateContent()
    {
        InitContent(_selectedTab.ShopType);
    }
}
