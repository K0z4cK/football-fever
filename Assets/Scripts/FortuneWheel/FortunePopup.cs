using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FortunePopup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _description;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private GameObject _coinsParent;
    [SerializeField]
    private TMP_Text _coins;

    public void ShowPrize(int prize)
    {
        _image.gameObject.SetActive(false);
        _coinsParent.SetActive(true);

        _description.text = "You won";
        _coins.text = prize.ToString();
    }

    public void ShowPrize(FootballerItem prize)
    {
        _image.gameObject.SetActive(true);
        _coinsParent.SetActive(false);

        _description.text = "You won new Footballer";
        _image.sprite = prize.sprite;
    }
}
