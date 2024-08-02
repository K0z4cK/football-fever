using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemTemplate : MonoBehaviour
{
    [SerializeField]
    protected Image _image;
    [SerializeField]
    protected TMP_Text _name;
    [SerializeField]
    protected TMP_Text _description;
    [SerializeField]
    protected TMP_Text _priceText;
    [SerializeField]
    protected Button _buyButton;

    protected int _price;

    public virtual void Init(int price)
    {
        _price = price;
        _priceText.text = price.ToString();
    }
}
