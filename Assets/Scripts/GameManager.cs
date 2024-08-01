using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState {None, Menu, Game, Team }

public class GameManager : MonoBehaviour
{
    private int _coins = 0;

    [SerializeField]
    private TMP_Text _coinsCount;

    [SerializeField]
    private TeamPanel _teamPanel;
    [SerializeField]
    private CardBattle _battle;

    [SerializeField]
    private int _startTeamCount = 4;

    private GameState _state;


    private void Awake()
    {
        SetNewTeam();
        AddRandomTeam(5);
        _battle.Init(AddCoins, AddCoins, RemoveFootballers);
        _coinsCount.text = _coins.ToString();
    }

    private void AddCoins(int coins)
    {
        _coins += coins;
        _coinsCount.text = _coins.ToString();
    }

    private void RemoveFootballers(List<Footballer> footballers)
    {
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
            footballers.Add(newFootballer);
        }

        _teamPanel.AddFootballers(footballers);
    }
}
