using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ViewCanvasGameOver : ViewCanvas {
    public GameObject PanelTimer => _panelTimer;
    public Image Timer => _imageTimer;
    public TMP_Text TextTimer => _textTimer;
    public ButtonExpansion ButtonCancel => _buttonCancel;
    public ButtonExpansion ButtonStartAds => _buttonAds;
    public Image Panel => _panel;
    public TMP_Text TextTotalScore => _textScore;
    public TMP_Text TextTempTotalScore => _textTempScore;
    public RectTransform RectTotalScore => _textScore.GetComponent<RectTransform>();


    [Header("Timer")]
    [SerializeField] private GameObject _panelTimer;
    [SerializeField] private Image _imageTimer;
    [SerializeField] private TMP_Text _textTimer;

    [Space]
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textTempScore;
    [SerializeField] private ButtonExpansion _buttonCancel;
    [SerializeField] private ButtonExpansion _buttonAds;
    [SerializeField] private Image _panel;

    private Coroutine _timer;
    private WaitForSeconds _wfs = new WaitForSeconds(1);

    public void StartTimer() {
        _textTimer.text = "5";

        if (_timer != null) {
            StopCoroutine(_timer);
        }

        _timer = StartCoroutine(CoTimer());
    }

    private IEnumerator CoTimer() {
        for(int i = 0; i <= 5; ++i) {
            _imageTimer.fillAmount = 1;
            _textTimer.text = (5 - i).ToString();
            _imageTimer.DOFillAmount(0, 0.7f).SetEase(Ease.InOutExpo);
            yield return _wfs;
        }
    }
}