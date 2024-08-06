using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsPrize : MonoBehaviour
{
    [SerializeField]
    private int _prize;

    [SerializeField]
    private TMP_Text _coins;
    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _image.gameObject.SetActive(false);
        _coins.text = _prize.ToString();
    }

    public int GetPrize() => _prize;
}
