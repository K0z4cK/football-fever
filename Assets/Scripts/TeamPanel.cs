using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPanel : MonoBehaviour
{
    [SerializeField]
    private FootballerCard _cardPrefab;
    [SerializeField]
    private Transform _content;

    [SerializeField]
    private float _maxHeight;

    private RectTransform _rectTransform;
    private float _normalHeight;


    private List<Footballer> _footballers = new List<Footballer>();

    private void Awake()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _normalHeight = _rectTransform.rect.height;
    }

    public void Init(List<Footballer> footballers)
    {
        _footballers = footballers;
        foreach(var footballer in _footballers)
        {
            var newCard = Instantiate(_cardPrefab, _content);
            newCard.Init(footballer);
        }
    }

    public void ShowTeamPanel()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _maxHeight);
    }

    public void HideTeamPanel()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _normalHeight);
    }
}
