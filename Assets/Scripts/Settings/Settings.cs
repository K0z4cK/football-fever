using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _coins;

    [SerializeField]
    private Toggle _music;
    [SerializeField]
    private Toggle _sound;

    [SerializeField]
    private Image _background;

    //current music track

    [SerializeField]
    private BackgroundCard _backgroundCardPrefab;
    [SerializeField]
    private MusicCard _musicCardPrefab;

    [SerializeField]
    private Transform _backgroundsContent;
    [SerializeField]
    private Transform _musicContent;

    private BackgroundCard _selectedBackgroundCard;
    private MusicCard _selectedMusicCard;

    private List<BackgroundCard> _backgroundCards = new List<BackgroundCard>();
    private List<MusicCard> _musicCards = new List<MusicCard>();

    private List<BackgroundItem> _backgroundItems;
    private List<MusicItem> _musicItems;

    private PlayerData _playerData;

    public void Init(PlayerData playerData, List<BackgroundItem> backgroundItems, List<MusicItem> musicItems)
    {
        _playerData = playerData;
        _backgroundItems = backgroundItems;
        _musicItems = musicItems;

        SetBackgroundCards();
        SetMusicCards();
    }

    private void SetBackgroundCards()
    {
        if (_backgroundItems.Count == _backgroundsContent.childCount)
            return;

        foreach(Transform child in _backgroundsContent)
            Destroy(child.gameObject);

        _backgroundCards.Clear();

        foreach (var item in _backgroundItems)
        {
            var newCard = Instantiate(_backgroundCardPrefab, _backgroundsContent);
            newCard.Init(item, UpdateBackground);
            _backgroundCards.Add(newCard);
        }
        _backgroundCards.Find(x => x.Item.id == _playerData.selectedBackground).Select();
    }

    private void SetMusicCards()
    {
        if (_musicItems.Count == _musicContent.childCount)
            return;

        foreach (Transform child in _musicContent)
            Destroy(child.gameObject);

        _musicCards.Clear();

        foreach (var item in _musicItems)
        {
            var newCard = Instantiate(_musicCardPrefab, _musicContent);
            newCard.Init(item, UpdateMusic);
            _musicCards.Add(newCard);
        }
        _musicCards.Find(x => x.Item.id == _playerData.selectedMusic).Select();
    }

    public void UpdateCards()
    {
        SetBackgroundCards();
        SetMusicCards();
    }


    private void UpdateBackground(BackgroundCard item)
    {
        _playerData.selectedBackground = item.Item.id;
        _background.sprite = item.Item.sprite;
        _selectedBackgroundCard = item;

        foreach (var card in _backgroundCards)
        {
            if (card != _selectedBackgroundCard)
                card.Deselect();
        }

    }

    private void UpdateMusic(MusicCard item)
    {
        //todo
    }

    public void SetCoins(int coins)
    {
        _coins.text = coins.ToString();
    }
}
