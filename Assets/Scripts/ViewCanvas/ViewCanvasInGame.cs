using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ViewCanvasInGame : ViewCanvas {
    public Transform BallParent => _ballParent;
    public ButtonExpansion ButtonBack => _buttonBack;
    public ButtonExpansion ButtonDestroyItem => _buttonDestroyItem;
    public ButtonExpansion ButtonRankUpItem => _buttonRankUpItem;
    public ButtonExpansion ButtonRerollItem => _buttonRerollItem;

    public TMP_Text TextCurrScore => _textCurrScore;
    public TMP_Text TextDestoryItemCount => _textDestoryItemCount;
    public TMP_Text TextRankUpItemCount => _textRankUpItemCount;
    public TMP_Text TextRerollItemCount => _textRerollItemCount;

    public GameObject DestoryItemAdsPanel => _destoryItemAdsPanel;
    public GameObject RankUpItemAdsPanel => _rankUpItemAdsPanel;
    public GameObject RerollItemAdsPanel => _rerollItemAdsPanel;

    public Transform Underground => _underground;

    [SerializeField] private GameObject[] _background;
    [SerializeField] private ButtonExpansion _buttonBack;
    [SerializeField] private Transform _ballParent;
    [SerializeField] private TMP_Text _textCurrScore;
    [SerializeField] private TMP_Text _textBestScore;

    [SerializeField] private ButtonExpansion _buttonDestroyItem;
    [SerializeField] private ButtonExpansion _buttonRankUpItem;
    [SerializeField] private ButtonExpansion _buttonRerollItem;

    [SerializeField] private TMP_Text _textDestoryItemCount;
    [SerializeField] private TMP_Text _textRankUpItemCount;
    [SerializeField] private TMP_Text _textRerollItemCount;

    [SerializeField] private GameObject _destoryItemAdsPanel;
    [SerializeField] private GameObject _rankUpItemAdsPanel;
    [SerializeField] private GameObject _rerollItemAdsPanel;

    [SerializeField] private Transform _underground;

    private WaitForSeconds _wfs = new WaitForSeconds(0.01f);

    public ViewCanvasInGame SetBestScore(int score) {
        _textBestScore.text = score.ToString();
        return this;
    }

    public ViewCanvasInGame SetCurrScore(int score) {
        _textCurrScore.text = score.ToString();
        return this;
    }

    public ViewCanvasInGame SetBackground(int index) {
        for(int i = 0; i < _background.Length; i++) {
            _background[i].SetActive(i == index);
        }
        return this;
    }
}