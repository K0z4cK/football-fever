using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopTab : MonoBehaviour
{
    private UnityAction<ShopTab> onSelect;

    [SerializeField]
    private ShopType _shopType;
    public ShopType ShopType => _shopType;

    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _selectedColor;

    private Button _selectButton;

    public void Init(UnityAction<ShopTab> select)
    {
        _selectButton = GetComponent<Button>();
        onSelect = select;
        _selectButton.onClick.AddListener(Select);
    }

    public void Select() 
    {
        onSelect?.Invoke(this);
        _selectButton.image.color = _selectedColor;
    }

    public void Deselect() => _selectButton.image.color = _normalColor;

}
