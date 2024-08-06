using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Tournament : MonoBehaviour
{
    private UnityAction<int> onStartTournament;

    private UnityAction<int> onStart;

    private UnityAction<int> onWin;
    private UnityAction<List<Footballer>> onLose;

    [Header("Footballers Pool")]
    [SerializeField]
    private GameObject _poolPanel;
    [SerializeField]
    private Transform _footballersPool;
    private List<Footballer> _playerFootballers;
    private List<Footballer> _allFootballers;

    [Header("Tournament Panel")]
    [SerializeField]
    private TeamPanel _teamPanel;

    [SerializeField]
    private List<TournamentCard> _firstTourCards;
    [SerializeField]
    private List<TournamentCard> _secondTourCards;
    [SerializeField]
    private List<TournamentCard> _lastTourCards;

    [SerializeField]
    private Button _play;
    [SerializeField]
    private Button _clear;
    [SerializeField]
    private Button _cancel;

    [SerializeField]
    private TournamentCard _winnerCard;

    [SerializeField]
    private GameObject _tournamentNet;

    [Header("Duel Battle")]
    [SerializeField]
    private GameObject _duelBattlePanel;
    [SerializeField]
    private BattleCard _firstCard;
    [SerializeField]
    private BattleCard _secondCard;

    [Header("Popup")]
    [SerializeField]
    private GameObject _popup;
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private Image _image;

    private List<TournamentCard> _cards;
    private List<Footballer> _footballersToRemove = new List<Footballer>();

    private TournamentCard _firstTourCard;
    private TournamentCard _secondTourCard;

    private TournamentCard _nextTourCard;

    private bool _canGoNext = false;
    private int _winMultiplicator = 1;


    public void Init(UnityAction<int> startTournament, UnityAction<int> start, UnityAction<int> win, UnityAction<List<Footballer>> lose)
    {
        //SetDuelBattleCards();
        //SetTeamBattleCards();
        onStartTournament = startTournament;
        onStart = start;
        onWin = win;
        onLose = lose;
    }

    public void SetPlayersPool()
    {
        _tournamentNet.SetActive(false);
        _poolPanel.gameObject.SetActive(true);

        _clear.onClick.RemoveAllListeners();
        _cancel.onClick.RemoveAllListeners();
        _play.onClick.RemoveAllListeners();

        _clear.onClick.AddListener(delegate
          {
              OnClearClick(delegate { SetPlayersPool(); }, false);
          });

        _cancel.onClick.AddListener(delegate
        {
            _teamPanel.RefreshCards();
        });

        _play.onClick.AddListener(SetNewTournament);

        _play.gameObject.SetActive(true);
        _cancel.gameObject.SetActive(true);

        _clear.gameObject.SetActive(false);
        _play.interactable = false;

        _teamPanel.SetDragTargets(new List<Transform>() { _footballersPool }, 1000f);

        StartCoroutine(PoolCoroutine());
    }

    private IEnumerator PoolCoroutine()
    {
        while(!_play.interactable)
        {
            yield return null;
            if (_footballersPool.childCount != 0)
            {
                _play.interactable = true;
                _clear.gameObject.SetActive(true);
            }
        }
    }

    public void SetNewTournament()
    {
        _tournamentNet.SetActive(true);
        _poolPanel.gameObject.SetActive(false);

        _clear.onClick.RemoveAllListeners();
        _cancel.onClick.RemoveAllListeners();
        _play.onClick.RemoveAllListeners();

        _clear.onClick.AddListener(delegate
        {
            OnClearClick(delegate { SetNewTournament(); }, true);
        });

        _cancel.onClick.AddListener(delegate
        {
            _teamPanel.RefreshCards();
        });

        _play.onClick.AddListener(StartTournament);

        _playerFootballers = new List<Footballer>();
        foreach (Transform child in _footballersPool)
            _playerFootballers.Add(child.GetComponent<FootballerCard>().Footballer);

        _allFootballers = new List<Footballer>();

        for(int i = 0; i < 8 -_playerFootballers.Count; i++)
        {
            var footballer = new Footballer();
            footballer.SetStatsRandom();
            _allFootballers.Add(footballer);
        }

        _allFootballers.AddRange(_playerFootballers);

        _teamPanel.SetTeam(new List<Footballer>(_allFootballers));

        _footballersToRemove = new List<Footballer>();

        _secondTourCards.ForEach(card => card.Init());
        _lastTourCards.ForEach(card => card.Init());
        _winnerCard.Init();

        _cards = new List<TournamentCard>(_firstTourCards);
        _cards.ForEach(battleCard => { battleCard.Init(RemoveCard); });

        _play.gameObject.SetActive(true);
        _cancel.gameObject.SetActive(true);

        _clear.gameObject.SetActive(false);
        _play.interactable = false;


        RefreshCards();

    }

    private void StartTournament()
    {
        _play.gameObject.SetActive(false);
        _clear.gameObject.SetActive(false);
        _cancel.gameObject.SetActive(false);

        StartCoroutine(TournamentCoroutine());

        onStartTournament?.Invoke(_playerFootballers.Count * 11);
    }

    private IEnumerator TournamentCoroutine()
    {
        yield return null;

        int nextTourId = 0;

        for (int i = 1; i < _firstTourCards.Count; i += 2)
        {
            yield return new WaitForSeconds(1f);

            _nextTourCard = _secondTourCards[nextTourId];
            _firstTourCard = _firstTourCards[i - 1];
            _secondTourCard = _firstTourCards[i];

            _canGoNext = true;

            StartDuel(_firstTourCard, _secondTourCard);

            yield return new WaitUntil(() => _canGoNext);
            nextTourId++;
        }

        nextTourId = 0;
        _winMultiplicator = 5;

        yield return new WaitForSeconds(1f);

        _nextTourCard = _lastTourCards[nextTourId];
        _firstTourCard = _secondTourCards[0];
        _secondTourCard = _secondTourCards[2];

        _canGoNext = true;

        StartDuel(_firstTourCard, _secondTourCard);

        yield return new WaitUntil(() => _canGoNext);

        nextTourId++;

        yield return new WaitForSeconds(1f);

        _nextTourCard = _lastTourCards[nextTourId];
        _firstTourCard = _secondTourCards[1];
        _secondTourCard = _secondTourCards[3];

        _canGoNext = true;

        StartDuel(_firstTourCard, _secondTourCard);

        yield return new WaitUntil(() => _canGoNext);

        nextTourId = 0;

        _winMultiplicator = 10;

        yield return new WaitForSeconds(1f);

        _nextTourCard = _winnerCard;
        _firstTourCard = _lastTourCards[0];
        _secondTourCard = _lastTourCards[1];

        _canGoNext = true;

        StartDuel(_firstTourCard, _secondTourCard);

        yield return new WaitUntil(() => _canGoNext);
        yield return new WaitForSeconds(1f);

        _popup.SetActive(true);
        _name.text = _winnerCard.Footballer.Name;
        _image.sprite = _winnerCard.Footballer.Sprite;

        _winMultiplicator = 1;
        onLose?.Invoke(_footballersToRemove);

        SoundManager.Instance.MakeSound(SoundType.Win);
    }

    private void StartDuel(TournamentCard firstTourCard, TournamentCard secondTourCard)
    {
        _duelBattlePanel.SetActive(true);
        _tournamentNet.SetActive(false);

        _firstCard.SetFootballer(firstTourCard.Footballer);
        _secondCard.SetFootballer(secondTourCard.Footballer);

        StartCoroutine(DuelCoroutine());
    }

    private IEnumerator DuelCoroutine()
    {
        _canGoNext = false;

        float firstResult = _firstCard.GetDuelResult();
        float secondResult = _secondCard.GetDuelResult();
        
        if(_playerFootballers.Contains(_firstCard.Footballer) || _playerFootballers.Contains(_secondCard.Footballer))
            onStart?.Invoke(_firstCard.Footballer.SumOfPoints);

        yield return new WaitForSeconds(1f);
        _firstCard.ShowResult(firstResult);
        yield return new WaitForSeconds(2f);
        _secondCard.ShowResult(secondResult);
        yield return new WaitForSeconds(2f);

        var loseList = new List<Footballer>();
        float procentage = 0;
        int win = 0;

        if (firstResult >= secondResult)
        {
            _firstTourCard.ShowEndPanel(true);
            _secondTourCard.ShowEndPanel(false);            

            loseList = new List<Footballer>() { _secondCard.Footballer };
            _nextTourCard.SetFootballer(_firstCard.Footballer);
            procentage = secondResult / firstResult;
            win = _secondCard.Footballer.SumOfPoints * 2;
            float countedWin = (win * procentage) * _winMultiplicator;
            if (_playerFootballers.Contains(_firstCard.Footballer))
                onWin?.Invoke((int)countedWin);
        }
        else if (firstResult < secondResult)
        {
            _firstTourCard.ShowEndPanel(false);
            _secondTourCard.ShowEndPanel(true);

            loseList = new List<Footballer>() { _firstCard.Footballer };
            _nextTourCard.SetFootballer(_secondCard.Footballer);
            procentage = firstResult / secondResult;
            win = _firstCard.Footballer.SumOfPoints * 2;
            float countedWin = (win * procentage) * _winMultiplicator;
            if (_playerFootballers.Contains(_secondCard.Footballer))
                onWin?.Invoke((int)countedWin);
        }
       
        _footballersToRemove.AddRange(loseList);       

        _duelBattlePanel.SetActive(false);
        _tournamentNet.SetActive(true);

        _canGoNext = true;
    }

    private void RemoveCard(TournamentCard card)
    {
        _cards.Remove(card);
        RefreshCards();

        if (_cards.Count == 0)
        {
            _play.interactable = true;
        }

        _clear.gameObject.SetActive(true);
    }

    private void RefreshCards()
    {
        List<Transform> targets = new List<Transform>();
        _cards.ForEach(x => targets.Add(x.transform));
        _teamPanel.SetDragTargets(targets, 500f);
    }

    private void OnClearClick(UnityAction action, bool isNet)
    {
        _clear.gameObject.SetActive(false);
        if(isNet)
            _teamPanel.SetTeam(_allFootballers);
        else
            _teamPanel.RefreshCards();

        action?.Invoke();
    }

}
