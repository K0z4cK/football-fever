using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Wheel : MonoBehaviour
{
    private UnityAction onRotate;

    private UnityAction<int> onWinCoins;
    private UnityAction<FootballerItem> onWinFootballer;

    [SerializeField]
    private float _minRotatePower;
    [SerializeField]
    private float _maxRotatePower;
    [SerializeField]
    private float _stopPower;
    [SerializeField]
    private float _waitSecondsToStop;

    [SerializeField]
    private Rigidbody2D rbody;

    [SerializeField]
    private Transform _pointer;

    [SerializeField]
    private List<Transform> _prizes;

    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private TMP_Text _buttonText;
    [SerializeField]
    private TMP_Text _buttonTimer;

    [SerializeField]
    private FortunePopup _popup;

    private bool _isRotate = false;
    private bool _isStop = false;
    float t;

    private bool _canSpin = true;

    public void Init(UnityAction rotate, UnityAction<int> winCoins,UnityAction<FootballerItem> winFootballer)
    {
        onRotate = rotate;
        onWinCoins = winCoins;
        onWinFootballer = winFootballer;
        _playButton.onClick.AddListener(Rotete);
    }

    public void SetSpinAbility(bool canSpin) 
    { 
        _canSpin = canSpin; 
        _playButton.interactable = canSpin;

        _buttonText.gameObject.SetActive(_canSpin);
        _buttonTimer.gameObject.SetActive(!_canSpin);

    }

    private void Update()
    {
        if (rbody.angularVelocity > 0)
        {
            if(_isStop)
                rbody.angularVelocity -= _stopPower * Time.deltaTime;

            rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
        }

        if (rbody.angularVelocity == 0 && _isRotate)
        {
            t += 1 * Time.deltaTime;
            if (t >= 0.5f)
            {
                GetReward();

                _isRotate = false;
                _isStop = false;
                t = 0;
            }
        }
    }

    private IEnumerator WaitToStop()
    {
        yield return new WaitForSeconds(_waitSecondsToStop);
        _isStop = true;
    }

    private void Rotete()
    {
        if (!_isRotate && _canSpin)
        {
            rbody.AddTorque(UnityEngine.Random.Range(_minRotatePower, _maxRotatePower + 1));
            _isRotate = true;
            SetSpinAbility(false);
            StartCoroutine(WaitToStop());

            onRotate?.Invoke();
        }
    }



    private void GetReward()
    {
        Transform closestPrize = _prizes[0];
        foreach (Transform prize in _prizes)
        {
            if(Vector3.Distance(closestPrize.position, _pointer.position) > Vector3.Distance(prize.position, _pointer.position))
            {
                closestPrize = prize;
            }
        }
        print("prize " + closestPrize.name);
        Win(closestPrize);
    }


    private void Win(Transform prizeTransform)
    {
        _popup.gameObject.SetActive(true);

        if (prizeTransform.TryGetComponent(out CoinsPrize cPrize))
        {
            var prize = cPrize.GetPrize();
            _popup.ShowPrize(prize);
            onWinCoins?.Invoke(prize);
        }
        else if (prizeTransform.TryGetComponent(out FootballerPrize fPrize))
        {
            var prize = fPrize.GetPrize();
            _popup.ShowPrize(prize);
            onWinFootballer?.Invoke(prize);
        }
    }

    public void SetTimer(TimeSpan time)
    {
        //print(time.ToString());
        //print(time.TimeOfDay.ToString());
        _buttonTimer.text = $"{time.Hours}:{time.Minutes}:{time.Seconds}";
    }

}