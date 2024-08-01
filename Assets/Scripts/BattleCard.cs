using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleCard : MonoBehaviour
{
    UnityAction<BattleCard> onInit;

    [SerializeField]
    private GameObject _placeholder;

    [SerializeField]
    private Image _image;
    [SerializeField]
    private TMP_Text _name;
    [SerializeField]
    private TMP_Text _ratig;

    private Footballer _footballer;
    public Footballer Footballer => _footballer;

    public void Init(UnityAction<BattleCard> init)
    {
        _placeholder.SetActive(true);
        onInit = init;
    }

    public void SetFootballer(Footballer footballer)
    {
        _footballer = footballer;

        if(_placeholder != null)
            _placeholder.SetActive(false);

        _name.text = footballer.Name;
        _ratig.text = footballer.Rating.ToString();
        onInit?.Invoke(this);
    }

    public void ShowResult(float result)
    {
        _ratig.transform.parent.gameObject.SetActive(true);
        _ratig.text = result.ToString();
    }

    public float GetDuelResult() => _footballer.GetStatsResult();

    public float GetTeamResult() => _footballer.GetStatsTeamResult();

    public void Reset()
    {
        _ratig.transform.parent.gameObject.SetActive(false);
    }
}
