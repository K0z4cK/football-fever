using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopMusicTemplate : ShopItemTemplate
{
    private UnityAction<int, ShopMusicTemplate> onBuy;

    private MusicItem _music;
    public MusicItem Music => _music;

    public void Init(int price, MusicItem music, UnityAction<int, ShopMusicTemplate> buy)
    {
        onBuy = buy;

        Init(price);

        _music = music;
        _image.sprite = _music.sprite;
        _name.text = _music.name;
        _description.text = _music.description;

        _buyButton.onClick.AddListener(OnBuyClick);
    }

    private void OnBuyClick() => onBuy?.Invoke(_price, this);
}
