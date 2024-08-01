using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [SerializeField]
    private Button _closeButtoon;

    [SerializeField]
    private TMP_Text _coins;

    public void Init(int coins, UnityAction onClose)
    {
        _coins.text = coins.ToString();
        _closeButtoon.onClick.RemoveAllListeners();
        _closeButtoon.onClick.AddListener(onClose);       
    }
}
