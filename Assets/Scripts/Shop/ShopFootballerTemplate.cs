using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopFootballerTemplate : ShopItemTemplate
{
    private UnityAction<int, ShopFootballerTemplate> onBuy;

    private Footballer _footballer;
    public Footballer Footballer => _footballer;

    private bool _isRandom = false;
    private bool _isUnique;
    public bool IsUnique => _isUnique;

    private string _id;
    public string Id => _id;

    public void Init(int price, FootballerItem footballer, UnityAction<int, ShopFootballerTemplate> buy)
    {
        onBuy = buy;

        Init(price);

        _id = footballer.id;

        _footballer = new Footballer(footballer.name, footballer.stats, footballer.sprite, footballer.points);

        _image.sprite = _footballer.Sprite;
        _name.text = _footballer.Name;
        _description.text = $"Strength:\t{_footballer.Stats.strength}\n" +
            $"Speed:\t{_footballer.Stats.speed}\n" +
            $"Endurance:\t{_footballer.Stats.endurance}\n" +
            $"Accuracy:\t{_footballer.Stats.accuracy}\n" +
            $"Teaming:\t{_footballer.Stats.teaming}\n" +
            $"Points:\t{_footballer.Points}";

        if(_footballer.IsStatsZero())
        {
            _description.text = "Footballer with randomly distributed 10 points of characteristics ";
            _footballer.SetStatsRandom();
            _isRandom = true;
        }

        _isUnique = footballer.isUnique;
        if(_isUnique)
            _footballer.SetNewId(footballer.id);

        _buyButton.onClick.AddListener(OnBuyClick);
    }

    private void OnBuyClick() => onBuy?.Invoke(_price, this);

    public void Refresh()
    {
        _footballer.SetNewName();
        _footballer.SetNewId();
        _name.text = _footballer.Name;

        if (_isRandom)
            _footballer.SetStatsRandom();
    }
}
