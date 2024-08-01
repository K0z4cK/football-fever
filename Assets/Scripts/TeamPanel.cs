using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TeamPanel : MonoBehaviour
{
    private UnityAction onTeamFullShow;
    private UnityAction onTeamFullHide;

    [SerializeField]
    private FootballerCard _cardPrefab;
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private float _maxHeight;

    [SerializeField]
    private RectTransform _rectTransform;
    private float _normalHeight;

    [Header("Footballer Card Panel")]
    [SerializeField]
    private GameObject _cardPanel;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMP_Text _name;

    [SerializeField]
    private TMP_Text _strength;
    [SerializeField]
    private TMP_Text _speed;
    [SerializeField]
    private TMP_Text _endurance;
    [SerializeField]
    private TMP_Text _accuracy;
    [SerializeField]
    private TMP_Text _teaming;
    [SerializeField]
    private TMP_Text _points;

    [SerializeField]
    private TMP_Text _rating;

    [SerializeField]
    private List<StructDictionary<StatType, Button>> _statButtons = new();

    [SerializeField]
    private Button _closeCard;

    private List<Footballer> _footballers = new List<Footballer>();
    private List<FootballerCard> _footballerCards = new List<FootballerCard>();

    private Footballer _selectedFootballer = null;

    private void Awake()
    {
        _normalHeight = _rectTransform.rect.height;
        _closeCard.onClick.AddListener(OnHideCardPanel);
    }

    public void Init(List<Footballer> footballers)
    {
        _footballers = footballers;
        foreach(var footballer in _footballers)
        {
            var newCard = Instantiate(_cardPrefab, _content);
            newCard.Init(OnShowCardPanel, OnCardDestroy, footballer);
            _footballerCards.Add(newCard);
        }
    }

    public void ShowTeamPanel()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _maxHeight);
        _footballerCards.ForEach(card => {card.SetMode(GameState.Team); });
        onTeamFullShow?.Invoke();
    }

    public void HideTeamPanel()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _normalHeight);
        _footballerCards.ForEach(card => { card.SetMode(GameState.Menu); });
        onTeamFullHide?.Invoke();
    }

    private void OnShowCardPanel(Footballer footballer)
    {
        _selectedFootballer = footballer;

        foreach(var button in _statButtons)
        {
            button.Value.onClick.RemoveAllListeners();
            button.Value.onClick.AddListener(delegate 
            { 
                footballer.IncreaseStat(button.Key);
                UpdateCardInfo();
            });
        }

        UpdateCardInfo();
        _cardPanel.SetActive(true);
    }

    private void OnHideCardPanel()
    {
        _footballerCards.ForEach(card => { card.UpdateInfo(); });
        _cardPanel.SetActive(false);
    }

    public void UpdateCardInfo()
    {
        if(_selectedFootballer ==  null)
            return;

        //image todo

        _name.text = _selectedFootballer.Name;
        _strength.text = _selectedFootballer.Stats.strength.ToString();
        _speed.text = _selectedFootballer.Stats.speed.ToString();
        _endurance.text = _selectedFootballer.Stats.endurance.ToString();
        _accuracy.text = _selectedFootballer.Stats.accuracy.ToString();
        _teaming.text = _selectedFootballer.Stats.teaming.ToString();

        _rating.text = _selectedFootballer.Rating.ToString();
        _points.text = _selectedFootballer.Points.ToString();
    }

    public void SetBattleMode()
    {
        _footballerCards.ForEach(card => { card.SetMode(GameState.Game); });
    }

    public void SetMenuMode()
    {
        _footballerCards.ForEach(card => { card.SetMode(GameState.Menu); });
    }

    public void AddFootballer(Footballer footballer)
    {
        _footballers.Add(footballer);
        RefreshCards();
    }

    public void AddFootballers(List<Footballer> footballers)
    {
        _footballers.AddRange(footballers);
        RefreshCards();
    }

    public void RemoveFootballers(List<Footballer> footballers)
    {
        footballers.ForEach(f => print("to remove: " + f.Name));
        footballers.ForEach(f => _footballers.Remove(f));
        RefreshCards();
    }

    public void SetDragTargets(List<BattleCard> cards)
    {
        _footballerCards.ForEach((card) => {
            card.SetDragTargets(cards);
        });
        SetBattleMode();
    }

    public void RefreshCards()
    {
        _footballerCards.ForEach(card => { 
            if(card != null)
                Destroy(card.gameObject); 
        });
        _footballerCards.Clear();

        foreach (var footballer in _footballers)
        {
            var newCard = Instantiate(_cardPrefab, _content);
            newCard.Init(OnShowCardPanel, OnCardDestroy, footballer);
            _footballerCards.Add(newCard);
        }
    }

    private void OnCardDestroy(FootballerCard card)
    {
        _footballerCards.Remove(card);
        Destroy(card.gameObject);
    }
}
