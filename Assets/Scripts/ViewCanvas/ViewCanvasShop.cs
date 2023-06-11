using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ViewCanvasShop : ViewCanvas {
    public ButtonExpansion[] ButtonMenus => _menuButtons;
    public ButtonExpansion[] ButtonObjectItems => _objectItems;
    public ButtonExpansion[] ButtonBackgroundItems => _backgroundItems;
    public ButtonExpansion ButtonCoinPlus => _buttonCoinPlus;
    public GameObject[] ShopMenus => _shopMenus;
    public TMP_Text CoinText => _textCoin;


    public GameObject PanelCoinProductParent => _panelCoinProductParent;
    public ViewProducts ProductItem0 => _productItem0;
    public ViewProducts ProductItem1 => _productItem1;
    public ViewProducts ProductItem2 => _productItem2;
    public ViewProducts ProductCoin0 => _productCoin0;
    public ViewProducts ProductCoin1 => _productCoin1;
    public ViewProducts ProductCoin2 => _productCoin2;
    public ViewProducts ProductWatchAds => _productCoinWatchAds;
    public ViewProducts ProductDoubleCoin => _productDoubleCoin;
    public ViewProducts ProductPremium => _productPremium;
    public TMP_Text TextDailyRewardTime => _textDailyRewardTime;

    public TMP_Text TextDestruction => _textDestruction;
    public TMP_Text TextRankUp => _textRankUp;
    public TMP_Text TextReroll => _textReroll;


    [SerializeField] ButtonExpansion[] _menuButtons;
    [SerializeField] ButtonExpansion[] _objectItems;
    [SerializeField] ButtonExpansion[] _backgroundItems;
    [SerializeField] GameObject[] _shopMenus;
    [SerializeField] TMP_Text _textCoin;

    [SerializeField] private ButtonExpansion _buttonCoinPlus;

    [Header("Products")]
    [SerializeField] private GameObject _panelCoinProductParent;
    [SerializeField] private ViewProducts _productItem0;
    [SerializeField] private ViewProducts _productItem1;
    [SerializeField] private ViewProducts _productItem2;
    [SerializeField] private ViewProducts _productCoin0;
    [SerializeField] private ViewProducts _productCoin1;
    [SerializeField] private ViewProducts _productCoin2;
    [SerializeField] private ViewProducts _productCoinWatchAds;
    [SerializeField] private ViewProducts _productDoubleCoin;
    [SerializeField] private ViewProducts _productPremium;
    [SerializeField] private TMP_Text _textDailyRewardTime;

    [Header("Item Count")]
    [SerializeField] private TMP_Text _textDestruction;
    [SerializeField] private TMP_Text _textRankUp;
    [SerializeField] private TMP_Text _textReroll;

    [Header("Localize Text")]
    [SerializeField] private TMP_Text _textObject;
    [SerializeField] private TMP_Text _textBackground;
    [SerializeField] private TMP_Text _textSpecial;

    public void UnLockMenu(int index, bool flag) {
        var menu = _menuButtons[index].GetComponent<ShopMenu>();
        if (menu != null) {
            if (flag)
                menu.UnLock();
            else
                menu.Lock();
        }
    }

    public void SetLocalizeText() {
        _textObject.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Object);
        _textBackground.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Background);
        _textSpecial.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Special);
    }
}
