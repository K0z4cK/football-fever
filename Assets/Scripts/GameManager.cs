using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {None, Menu, Game, Team }

public class GameManager : MonoBehaviour
{ 
    [SerializeField]
    private TeamPanel _teamPanel;
    [SerializeField]
    private Menu _menu;

    [SerializeField]
    private int _startTeamCount = 4;

    private GameState _state;


    private void Awake()
    {
        SetNewTeam();
    }

    private void SetNewTeam()
    {
        List<Footballer> footballers = new List<Footballer>();
        for (int i = 0; i < _startTeamCount; i ++)
        {
            Footballer newFootballer = new Footballer(NamesCollection.GetRandomName());
            newFootballer.SetStatsRandom();
            footballers.Add(newFootballer);
        }

        _teamPanel.Init(footballers);
    }
}
