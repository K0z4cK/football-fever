using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TournamentCard : MonoBehaviour
{
    UnityAction<TournamentCard> onInit;

    [SerializeField]
    private GameObject _placeholder;
    [SerializeField]
    private GameObject _clearPanel;
    [SerializeField]
    private GameObject _losePanel;
    [SerializeField]
    private GameObject _winPanel;

    [SerializeField]
    private TMP_Text _name;

    private Footballer _footballer;
    public Footballer Footballer => _footballer;


    public void Init(UnityAction<TournamentCard> init)
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        _clearPanel.SetActive(false);
        _placeholder.SetActive(true);
        onInit = init;
    }

    public void Init()
    {
        _winPanel.SetActive(false);
        _losePanel.SetActive(false);
        _placeholder.SetActive(false);
        _clearPanel.SetActive(true);
    }

    public void SetFootballer(Footballer footballer)
    {
        _placeholder.SetActive(false);
        _clearPanel.SetActive(false);
        _footballer = footballer;
        _name.text = _footballer.Name;
        onInit?.Invoke(this);
    }

    public void ShowEndPanel(bool isWin) 
    { 
        if (isWin) 
            _winPanel.SetActive(true);
        else
            _losePanel.SetActive(true);
    } 
}
