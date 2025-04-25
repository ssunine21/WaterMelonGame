using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewCanvasInGame : ViewCanvas {
    public Transform BallParent => _ballParent;
    public ButtonExpansion ButtonBack => _buttonBack;
    public ButtonExpansion ButtonDestroyItem => _buttonDestroyItem;
    public ButtonExpansion ButtonRankUpItem => _buttonRankUpItem;
    public ButtonExpansion ButtonRerollItem => _buttonRerollItem;
    public ButtonExpansion ButtonShowObjBook => _buttonShowObjBook;

    public TMP_Text TextCurrScore => _textCurrScore;
    public Text TextDestoryItemCount => _textDestoryItemCount;
    public Text TextRankUpItemCount => _textRankUpItemCount;
    public Text TextRerollItemCount => _textRerollItemCount;

    public GameObject DestoryItemAdsPanel => _destoryItemAdsPanel;
    public GameObject RankUpItemAdsPanel => _rankUpItemAdsPanel;
    public GameObject RerollItemAdsPanel => _rerollItemAdsPanel;

    public ViewNextObjectUnit ViewNextObject => _viewNextObject;
    
    public RectTransform Underground => _underground;
    public RectTransform Up => _up;

    [SerializeField] private ButtonExpansion _buttonBack;
    [SerializeField] private Transform _ballParent;
    [SerializeField] private TMP_Text _textCurrScore;
    [SerializeField] private TMP_Text _textBestScore;

    [SerializeField] private ButtonExpansion _buttonDestroyItem;
    [SerializeField] private ButtonExpansion _buttonRankUpItem;
    [SerializeField] private ButtonExpansion _buttonRerollItem;
    [SerializeField] private ButtonExpansion _buttonShowObjBook;

    [SerializeField] private Text _textDestoryItemCount;
    [SerializeField] private Text _textRankUpItemCount;
    [SerializeField] private Text _textRerollItemCount;

    [SerializeField] private GameObject _destoryItemAdsPanel;
    [SerializeField] private GameObject _rankUpItemAdsPanel;
    [SerializeField] private GameObject _rerollItemAdsPanel;

    [SerializeField] private RectTransform _underground;
    [SerializeField] private RectTransform _up;
    [SerializeField] private ViewNextObjectUnit _viewNextObject;
    
    private WaitForSeconds _wfs = new WaitForSeconds(0.01f);

    public ViewCanvasInGame SetBestScore(int score) {
        _textBestScore.text = score.ToString();
        return this;
    }

    public ViewCanvasInGame SetCurrScore(int score) {
        _textCurrScore.text = score.ToString();
        return this;
    }
}