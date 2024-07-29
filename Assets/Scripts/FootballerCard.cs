using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FootballerCard : MonoBehaviour
{
    private Footballer _footballer;

    [Header("Face Info")]
    [SerializeField]
    private GameObject _faceInfo;
    [SerializeField]
    private Image _faceImage;
    [SerializeField]
    private TMP_Text _faceName;
    [SerializeField]
    private TMP_Text _ratingFace;   

    [Header("Full Info Left")]
    [SerializeField]
    private GameObject _fullInfoLeft;
    [SerializeField]
    private Image _fullImageLeft;
    [SerializeField]
    private TMP_Text _fullNameLeft;

    [SerializeField]
    private TMP_Text _strengthLeft;
    [SerializeField]
    private TMP_Text _speedLeft;
    [SerializeField]
    private TMP_Text _enduranceLeft;
    [SerializeField]
    private TMP_Text _accuracyLeft;
    [SerializeField]
    private TMP_Text _teamingLeft;

    [SerializeField]
    private TMP_Text _ratingHidenLeft;

    [Header("Full Info Right")]
    [SerializeField]
    private GameObject _fullInfoRight;
    [SerializeField]
    private Image _fullImageRight;
    [SerializeField]
    private TMP_Text _fullNameRight;

    [SerializeField]
    private TMP_Text _strengthRight;
    [SerializeField]
    private TMP_Text _speedRight;
    [SerializeField]
    private TMP_Text _enduranceRight;
    [SerializeField]
    private TMP_Text _accuracyRight;
    [SerializeField]
    private TMP_Text _teamingRight;

    [SerializeField]
    private TMP_Text _ratingHidenRight;

    private Transform _transform;
    private Transform _parent;
    private Button _button;

    private bool _isShowFoolInfo = false;

    private float _screenWidth;

    public void Init(Footballer footballer)
    {
        _screenWidth = Screen.width;

        _fullInfoLeft.SetActive(false);
        _fullInfoRight.SetActive(false);
        _button = GetComponent<Button>();
        _transform = transform;
        _parent = transform.parent;

        _footballer = footballer;
        _faceName.text = _footballer.Name;
        _fullNameLeft.text = _footballer.Name;
        _fullNameRight.text = _footballer.Name;
        _strengthLeft.text = _footballer.Stats.strength.ToString();
        _strengthRight.text = _footballer.Stats.strength.ToString();
        _speedLeft.text = _footballer.Stats.speed.ToString();
        _speedRight.text = _footballer.Stats.speed.ToString();
        _enduranceLeft.text = _footballer.Stats.endurance.ToString();
        _enduranceRight.text = _footballer.Stats.endurance.ToString();
        _accuracyLeft.text = _footballer.Stats.accuracy.ToString();
        _accuracyRight.text = _footballer.Stats.accuracy.ToString();
        _teamingLeft.text = _footballer.Stats.teaming.ToString();
        _teamingRight.text = _footballer.Stats.teaming.ToString();

        _ratingFace.text = _footballer.Rating.ToString();
        _ratingHidenLeft.text = _footballer.Rating.ToString();
        _ratingHidenRight.text = _footballer.Rating.ToString();

        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameObject fullInfoPanel = _fullInfoLeft;

        if (_transform.localPosition.x > Screen.width / 1.2f)
            fullInfoPanel = _fullInfoRight;

        if (_faceInfo.activeSelf)
            fullInfoPanel.transform.parent = _parent.parent;
        else
            fullInfoPanel.transform.parent = _transform;

        _isShowFoolInfo = !_isShowFoolInfo;
        _faceInfo.SetActive(!_isShowFoolInfo);
        fullInfoPanel.SetActive(_isShowFoolInfo);
    }

}
