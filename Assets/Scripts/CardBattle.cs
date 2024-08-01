using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardBattle : MonoBehaviour
{
    private UnityAction<int> onStart;

    private UnityAction<int> onWin;
    private UnityAction<List<Footballer>> onLose;

    [SerializeField]
    private GameObject _menuPanel;

    [SerializeField]
    private TeamPanel _teamPanel;

    [SerializeField]
    private Button _cancel;

    [Header("Duel Battle")]
    [SerializeField]
    private GameObject _duelBattlePanel;
    [SerializeField]
    private BattleCard _playerCard;
    [SerializeField]
    private BattleCard _enemyCard;
    [SerializeField]
    private Button _clearCard;
    [SerializeField]
    private Button _playDuel;

    [Header("Team Battle")]
    [SerializeField]
    private GameObject _teamBattlePanel;
    [SerializeField]
    private List<BattleCard> _playerCards;
    [SerializeField]
    private List<BattleCard> _enemyCards;

    [SerializeField]
    private GameObject _playerRatingObject;
    [SerializeField]
    private TMP_Text _playerRating;

    [SerializeField]
    private GameObject _enemyRatingObject;
    [SerializeField]
    private TMP_Text _enemyRating;

    [SerializeField]
    private Button _clearTeam;
    [SerializeField]
    private Button _playTeam;

    [Header("Popups")]
    [SerializeField]
    private WinPopup _winPopup;
    [SerializeField]
    private LosePopup _losePopup;
    [SerializeField]
    private GameObject _drawPopup;

    private List<BattleCard> _battleCards = new List<BattleCard>();

    public void Init(UnityAction<int> start, UnityAction<int> win, UnityAction<List<Footballer>> lose)
    {
        //SetDuelBattleCards();
        //SetTeamBattleCards();

        onStart = start;
        onWin = win;
        onLose = lose;

        _clearCard.onClick.AddListener(delegate
        {
            OnClearClick(delegate { SetDuelBattleCards(false); });
        });

        _clearTeam.onClick.AddListener(delegate
        {
            OnClearClick(delegate { SetTeamBattleCards(false); });
        });

        _cancel.onClick.AddListener(delegate
        {
            _teamPanel.RefreshCards();
        });

        _playDuel.onClick.AddListener(StartDuel);
        _playTeam.onClick.AddListener(StartTeamBattle);
    }

    public void SetDuelBattleCards(bool isNewMatch = true)
    {
        _enemyCard.Reset();

        _playDuel.gameObject.SetActive(true);
        _cancel.gameObject.SetActive(true);

        _clearCard.gameObject.SetActive(false);
        _duelBattlePanel.SetActive(true);

        _playDuel.interactable = false;

        _battleCards = new List<BattleCard>() { _playerCard };
        _battleCards.ForEach(battleCard => { battleCard.Init(RemoveCard); });
        RefreshCards();
        if (isNewMatch)
            SetEnemyDuelCard();
    }

    public void SetTeamBattleCards(bool isNewMatch = true)
    {
        _enemyCards.ForEach(enemyCard => { enemyCard.Reset(); });   

        _playTeam.gameObject.SetActive(true);
        _cancel.gameObject.SetActive(true);

        _clearTeam.gameObject.SetActive(false);
        _teamBattlePanel.SetActive(true);
        _playerRatingObject.SetActive(false);
        _enemyRatingObject.SetActive(false);

        _playTeam.interactable = false;

        _battleCards = new List<BattleCard>(_playerCards);
        _battleCards.ForEach(battleCard => { battleCard.Init(RemoveCard); });
        RefreshCards();

        if(isNewMatch)
            SetEnemyTeamCards();
    }

    private void RemoveCard(BattleCard card)
    {
        _battleCards.Remove(card);
        RefreshCards();

        if(_battleCards.Count == 0)
        {
            _playDuel.interactable = true;
            _playTeam.interactable = true;
        }

        _clearCard.gameObject.SetActive(true);
        _clearTeam.gameObject.SetActive(true);
    }

    private void RefreshCards()
    {
        _teamPanel.SetDragTargets(_battleCards);
    }

    private void OnClearClick(UnityAction action)
    {
        _clearCard.gameObject.SetActive(false);
        _clearTeam.gameObject.SetActive(false);

        _teamPanel.RefreshCards();

        action?.Invoke();
    }

    private void SetEnemyDuelCard()
    {
        Footballer enemy = new Footballer();
        enemy.SetStatsRandom();

        _enemyCard.SetFootballer(enemy);
    }

    private void SetEnemyTeamCards()
    {
        foreach (var card in _enemyCards)
        {
            Footballer enemy = new Footballer();
            enemy.SetStatsRandom();

            card.SetFootballer(enemy);
        }
    }

    private void StartDuel()
    {
        _playDuel.gameObject.SetActive(false);
        _clearCard.gameObject.SetActive(false);
        _cancel.gameObject.SetActive(false);

        float playerResult = _playerCard.GetDuelResult();
        float enemyResult = _enemyCard.GetDuelResult();

        _playerCard.ShowResult(playerResult);
        _enemyCard.ShowResult(enemyResult);

         onStart?.Invoke(_playerCard.Footballer.SumOfPoints);

        if (playerResult > enemyResult)
        {
            _winPopup.gameObject.SetActive(true);
            _winPopup.Init(_enemyCard.Footballer.SumOfPoints * 2,
                delegate
                {
                    OnWinPopupClose(_enemyCard.Footballer.SumOfPoints * 2);
                });
        }

        else if (playerResult < enemyResult)
        {
            var loseList = new List<Footballer>() { _playerCard.Footballer };

            _losePopup.gameObject.SetActive(true);

            onLose?.Invoke(loseList);

            _losePopup.Init(loseList, OnLosePopupClose);
        }
        else
           _drawPopup.SetActive(true);

    }

    private void StartTeamBattle()
    {
        _playTeam.gameObject.SetActive(false);
        _clearTeam.gameObject.SetActive(false);
        _cancel.gameObject.SetActive(false);

        float playerResult = 0;
        float enemyResult = 0;

        int playerCardsSumOfPoint = 0;
        int enemyCardsSumOfPoint = 0;

        foreach (var card in _playerCards)
        {
            float cardResult = card.GetTeamResult();
            playerResult += cardResult;
            card.ShowResult(cardResult);

            playerCardsSumOfPoint += card.Footballer.SumOfPoints;
        }

        foreach (var card in _enemyCards)
        {
            float cardResult = card.GetTeamResult();
            enemyResult += cardResult;
            card.ShowResult(cardResult);

            enemyCardsSumOfPoint += card.Footballer.SumOfPoints;
        }

        onStart?.Invoke(playerCardsSumOfPoint);

        _playerRatingObject.SetActive(true);
        _enemyRatingObject.SetActive(true);

        _playerRating.text = playerResult.ToString();
        _enemyRating.text = enemyResult.ToString();

        if (playerResult > enemyResult)
        {
            _winPopup.gameObject.SetActive(true);
            _winPopup.Init(enemyCardsSumOfPoint * 2,
                delegate
                {
                    OnWinPopupClose(enemyCardsSumOfPoint * 2);
                });
        }
        else if (playerResult < enemyResult)
        {
            List<Footballer> footballers = new List<Footballer>();
            _playerCards.ForEach(card => footballers.Add(card.Footballer));
            _losePopup.gameObject.SetActive(true);

            onLose?.Invoke(footballers);

            _losePopup.Init(footballers, OnLosePopupClose);
        }
        else
            _drawPopup.SetActive(true);

    }

    private void OnWinPopupClose(int coins)
    {
        _winPopup.gameObject.SetActive(false);
        _duelBattlePanel.gameObject.SetActive(false);
        _teamBattlePanel.gameObject.SetActive(false);
        _menuPanel.gameObject.SetActive(true);
        _teamPanel.SetMenuMode();
        _teamPanel.RefreshCards();

        onWin?.Invoke(coins);
    }

    private void OnLosePopupClose()
    {
        //onLose?.Invoke(footballers);

        _losePopup.gameObject.SetActive(false);
        _duelBattlePanel.gameObject.SetActive(false);
        _teamBattlePanel.gameObject.SetActive(false);
        _menuPanel.gameObject.SetActive(true);
        _teamPanel.SetMenuMode();

        
    }
}
