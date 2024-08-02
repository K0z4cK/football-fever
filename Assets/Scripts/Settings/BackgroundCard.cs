using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackgroundCard : MonoBehaviour
{
    private UnityAction<BackgroundCard> onSelect;

    [SerializeField]
    private Image _image;
    private Image _frame;
    private Button _button;

    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _selectedColor;

    private BackgroundItem _item;
    public BackgroundItem Item => _item;

    public void Init(BackgroundItem item, UnityAction<BackgroundCard> select)
    {
        _frame = GetComponent<Image>();
        _button = GetComponent<Button>();

        _item = item;
        _image.sprite = _item.sprite;
        onSelect = select;
        _button.onClick.AddListener(Select);
    }

    public void Select()
    {
        _frame.color = _selectedColor;
        onSelect?.Invoke(this);
    }

    public void Deselect()
    {
        _frame.color = _normalColor;
    }
}
