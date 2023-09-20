using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ViewCanvasGameOver : ViewCanvas {
    public ButtonExpansion ButtonCancel => _buttonCancel;
    public ButtonExpansion ButtonStartAds => _buttonAds;
    public Image Panel => _panel;
    public TMP_Text TextTotalScore => _textScore;
    public TMP_Text BestScoreText => _bestScoreText;
    public RectTransform ScoreRectTr => scoreRectTr;
    public RectTransform ContentRectTr => contentRectTr;
    public RectTransform HomeRectTr => homeRectTr;
    public RectTransform WatchAdsRectTr => watchAdsRectTr;
    public RectTransform CoinTr => _panelCoin;
    public RectTransform LevelTr => levelPanel;
    public RectTransform TagRectTr => tagRectTr;
    
    
    public TMP_Text TextCoin => _textCoin;
    public RectTransform PanelCoin => _panelCoin;

    [SerializeField] private RectTransform tagRectTr;
    [SerializeField] private RectTransform contentRectTr;
    [SerializeField] private RectTransform scoreRectTr;
    [SerializeField] private RectTransform homeRectTr;
    [SerializeField] private RectTransform watchAdsRectTr;
    [SerializeField] private RectTransform _panelCoin;
    [SerializeField] private RectTransform levelPanel;
    [SerializeField] private TMP_Text _textCoin;
    [SerializeField] private TMP_Text _lvText;
    [SerializeField] private Image _expAmount;

    [Space]
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private ButtonExpansion _buttonCancel;
    [SerializeField] private ButtonExpansion _buttonAds;
    [SerializeField] private Image _panel;

    [Space]
    [SerializeField] private TMP_Text _textWatchAds;

    public void SetCoin(int value)
    {
        _textCoin.text = value.ToString();
    }

    public ViewCanvasGameOver SetLevel(int value)
    {
        _lvText.text = value.ToString();
        return this;
    }

    public ViewCanvasGameOver SetFillAmount(float curr, float max)
    {
        _expAmount.fillAmount = curr / max;
        return this;
    }

    public ViewCanvasGameOver SetLocalize()
    {
        _textWatchAds.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.WatchAdsToContinuePlay);
        return this;
    }
}