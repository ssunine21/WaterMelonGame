using TMPro;
using UltimateClean;
using UnityEngine;
using UnityEngine.UI;

public class ViewCanvasBook : ViewCanvas
{
    public ButtonExpansion[] CloseButtons => _closeButtons;
    public ViewObjectBookTab[] ViewObjectBookTabs => _viewObjectBookTabs;
    public ViewObjectBook[] ViewObjectBooks => _viewObjectBooks;
    public SliderAnimation AllSliderAnimation => _allSliderAnimation;
    public SliderAnimation TabSliderAnimation => _tabSliderAnimation;
    public Button GetRewardButton => _getRewardButton;
    public Button GetAdRewardButton => _getAdRewardButton;
    public GameObject GetRewardButtonLockPanel => _getRewardButtonLockPanel;
    public Image GetRewardIcon => _getRewardIcon;
    
    
    [SerializeField] private ButtonExpansion[] _closeButtons;
    [SerializeField] private ViewObjectBookTab[] _viewObjectBookTabs;
    [SerializeField] private ViewObjectBook[] _viewObjectBooks;

    [SerializeField] private SliderAnimation _allSliderAnimation;
    [SerializeField] private SliderAnimation _tabSliderAnimation;

    [SerializeField] private Button _getRewardButton;
    [SerializeField] private Button _getAdRewardButton;
    [SerializeField] private GameObject _getRewardButtonLockPanel;
    [SerializeField] private Image _getRewardIcon;

    [SerializeField] private TMP_Text _expText;
    [SerializeField] private TMP_Text _goldText;

    [Header("Localize")]
    [SerializeField] private TMP_Text _textTitle;
    [SerializeField] private TMP_Text _textDesc;
    [SerializeField] private TMP_Text _textDescResetTime;

    public void SetRewardData(int exp, int gold)
    {
        _expText.text = $"+{exp}";
        _goldText.text = $"+{gold}";
    }
    
    public ViewCanvasBook SetLocalizeText() {
        _textTitle.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Book);
        _textDesc.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.BookDesc);
        int baseTime = ServerTime.BaseTime();
        _textDescResetTime.text = string.Format(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.BaseTime), baseTime);
        return this;
    }
}