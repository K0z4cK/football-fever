using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopBackgroundTemplate : ShopItemTemplate
{
    private UnityAction<int, ShopBackgroundTemplate> onBuy;

    private BackgroundItem _background;
    public BackgroundItem Background => _background;

    public void Init(int price, BackgroundItem background, UnityAction<int, ShopBackgroundTemplate> buy)
    {
        onBuy = buy;

        Init(price);

        _background = background;
        _image.sprite = _background.sprite;
        _name.text = _background.name;
        _description.text = _background.description;

        _buyButton.onClick.AddListener(OnBuyClick);
    }

    private void OnBuyClick() => onBuy?.Invoke(_price, this);
}
