using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
    [SerializeField]
    private Button _closeButtoon;

    [SerializeField]
    private BattleCard _midCard;

    [SerializeField]
    private GameObject _sideCardsParent;
    [SerializeField]
    private List<BattleCard> _sideCards;

    public void Init(List<Footballer> footballers, UnityAction onClose)
    {
        _closeButtoon.onClick.RemoveAllListeners();
        _closeButtoon.onClick.AddListener(onClose);

        _sideCardsParent.SetActive(false);

        if (footballers.Count > 1)
        {
            _sideCardsParent.SetActive(true);
            Footballer mid = GetBestFootballer(footballers);
            footballers.Remove(mid);

            _midCard.SetFootballer(mid);

            foreach(var card in _sideCards)
            {
                card.SetFootballer(footballers[0]);
                footballers.RemoveAt(0);
            }
        }
        else
            _midCard.SetFootballer(footballers[0]);
    }

    private Footballer GetBestFootballer(List<Footballer> footballers)
    {
        Footballer best = footballers[0];
        foreach (var footballer in footballers)
        {
            if(footballer.Rating > best.Rating)
                best = footballer;
        }

        return best;
    }
}
