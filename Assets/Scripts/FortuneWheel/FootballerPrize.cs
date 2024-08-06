using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FootballerPrize : MonoBehaviour
{
    [SerializeField]
    private FootballerItem _prize;

    [SerializeField]
    private GameObject _coins;
    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _coins.SetActive(false);
        _image.sprite = _prize.sprite;
    }

    public FootballerItem GetPrize() => _prize;
}
