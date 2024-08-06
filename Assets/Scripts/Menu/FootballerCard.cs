using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FootballerCard : MonoBehaviour
{
    private UnityAction<Footballer> onShowFootballerPanel;
    private UnityAction<FootballerCard> onDestroy;


    private Footballer _footballer;
    public Footballer Footballer => _footballer;

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
    private Transform _dragParent;

    private Button _button;

    [Header("Dragger")]
    [SerializeField]
    private DragAndDropHandler _dragger;

    private List<Transform> _dragTargets = new List<Transform>();
    private float _dropDistance = 0;

    /*[SerializeField]
    private Dragger _dragger;*/

    //private bool _isMiniMode = true;
    private GameState _gameMode;

    private bool _canShowInfo = true;
    private bool _isShowFoolInfo = false;

    private float _screenWidth;

    public void Init(UnityAction<Footballer> showFootballerPanel, UnityAction<FootballerCard> destroy, Footballer footballer)
    {
        //_dragger = GetComponent<Dragger>();

        _dragger.OnReleasedObject += Dragger_OnReleasedObject;
        _dragger.OnGrabbedObject += Dragger_OnGrabbedObject;

       _screenWidth = Screen.width;

        _fullInfoLeft.SetActive(false);
        _fullInfoRight.SetActive(false);
        _button = GetComponent<Button>();
        _transform = transform;
        _parent = transform.parent;
        _dragParent = _parent.parent.parent.parent;

        _footballer = footballer;
        UpdateInfo();

        _button.onClick.AddListener(OnButtonClick);

        onShowFootballerPanel = showFootballerPanel;
        onDestroy = destroy;

        _gameMode = GameState.Menu;
    }

    public void UpdateInfo()
    {
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
    }

    private void OnButtonClick()
    {
        if (!_canShowInfo)
        {
            _canShowInfo = true;
            return;
        }

        if (_gameMode == GameState.Team)
        {
            onShowFootballerPanel?.Invoke(_footballer);
            return;
        }

        GameObject fullInfoPanel = _fullInfoLeft;

        if (_transform.localPosition.x > Screen.width / 1.2f)
            fullInfoPanel = _fullInfoRight;

        if (_faceInfo.activeSelf)
            fullInfoPanel.transform.SetParent(_parent.parent, true);
        else
            fullInfoPanel.transform.SetParent(_transform, true);

        _isShowFoolInfo = !_isShowFoolInfo;
        _faceInfo.SetActive(!_isShowFoolInfo);
        fullInfoPanel.SetActive(_isShowFoolInfo);

        StartCoroutine(InfoFolow(fullInfoPanel));
        
    }

    private IEnumerator InfoFolow(GameObject infoPanel)
    {
        while (_isShowFoolInfo)
        {
            infoPanel.transform.position = new Vector3(infoPanel.transform.position.x, _transform.position.y, infoPanel.transform.position.z);
            yield return null;
        }
    }

    public void HideInfo()
    {
        _isShowFoolInfo = false;
        _faceInfo.SetActive(true);
        _fullInfoLeft.SetActive(false);
        _fullInfoRight.SetActive(false);
    }

    public void SetMode(GameState mode)
    {
        _gameMode = mode;
        HideInfo();
        if (_gameMode == GameState.Game)
            _dragger.SetDraggable(true);
        else
            _dragger.SetDraggable(false);

    }

    public void SetDragTargets(List<Transform> targets, float dropDistance)
    {
        _dragTargets = targets;
        _dropDistance = dropDistance;
    }

    private void Dragger_OnReleasedObject(Vector3 releasedPosition)
    {
        SoundManager.Instance.MakeSound(SoundType.DragDrop);

        foreach (var target in _dragTargets)
        {
            if(Vector3.Distance(releasedPosition, target.transform.position) < _dropDistance)
            {
                if (target.TryGetComponent(out BattleCard bCard))
                {
                    bCard.SetFootballer(_footballer);
                    onDestroy?.Invoke(this);
                }
                else if (target.TryGetComponent(out TournamentCard tCard))
                {
                    tCard.SetFootballer(_footballer);
                    onDestroy?.Invoke(this);
                }
                else
                    transform.SetParent(target.transform);
                
                /*_transform.SetParent(target, false);
                _transform.position = target.position;
                _dragTargets.Remove(target);*/
                return;
            }
        }

        _transform.SetParent(_parent, false);
    }

    private void Dragger_OnGrabbedObject()
    {
        _canShowInfo = false;
        _transform.SetParent(_dragParent);
    }

    /*public void SetMiniMode(bool mode)
    {
        _isMiniMode = mode;
        if (!_isMiniMode)       
            HideInfo();
        
    }*/

    /*private void OnDestroy()
    {
        _dragger.OnReleasedObject -= Dragger_OnReleasedObject;
        _dragger.OnGrabbedObject -= Dragger_OnGrabbedObject;
    }*/
}
