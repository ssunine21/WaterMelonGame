using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerShop {

    private static int _navIndex = 1;

    private readonly ViewCanvasShop _view;
    private UnityAction<int> OnSelectMenu;
    private UnityAction<int> OnSelectObjectItem;
    private UnityAction<int> OnSelectBackgroundItem;

    public ControllerShop(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasShop>(parent);
        _view.SetLocalizeText();

        OnSelectMenu += UpdateMenuView;
        OnSelectObjectItem += UpdateObjects;
        OnSelectBackgroundItem += UpdateBackgroundItems;

        ControllerMainNav.OnSelectMenu += SetVisible;
        SetButton();

        IAPManager.init.OnBindInitialized += () => {
            _view.ProductCoin0.SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_DUMMY));
            _view.ProductCoin1.SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_POKET));
            _view.ProductCoin2.SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_BOX));
        };

        _view.ProductItem0
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Destruction))
            .SetDescription(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.DestructionDesc))
            .SetPrice(PlayerItem.GetCost(Definition.Item.Destruction).ToString())
            .SetButtonAction(() => { if (PlayerItem.IsCanBuy(Definition.Item.Destruction)) { PlayerItem.Earn(Definition.Item.Destruction); PlayerCoin.Consume(PlayerItem.GetCost(Definition.Item.Destruction)); } });
        _view.ProductItem1
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RankUp))
            .SetDescription(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RankUpDesc))
            .SetPrice(PlayerItem.GetCost(Definition.Item.RankUp).ToString())
            .SetButtonAction(() => { if (PlayerItem.IsCanBuy(Definition.Item.RankUp)) { PlayerItem.Earn(Definition.Item.RankUp); PlayerCoin.Consume(PlayerItem.GetCost(Definition.Item.RankUp)); } });
        _view.ProductItem2
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Reroll))
            .SetDescription(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RerollDesc))
            .SetPrice(PlayerItem.GetCost(Definition.Item.Reroll).ToString())
            .SetButtonAction(() => { if (PlayerItem.IsCanBuy(Definition.Item.Reroll)) { PlayerItem.Earn(Definition.Item.Reroll); PlayerCoin.Consume(PlayerItem.GetCost(Definition.Item.Reroll)); } });

        _view.ProductCoin0
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.CoinDummy))
            .SetDescription($"1,500 + <#E38B29>1,500 {LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Bonus)}</color>")
            .SetAmount("3,000")
            .SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_DUMMY))
            .SetButtonAction(() => IAPManager.init.Purchase(IAPManager.COIN_DUMMY));
        _view.ProductCoin1
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.CoinPurse))
            .SetDescription($"4,000 + <#E38B29>4,000 {LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Bonus)}</color>")
            .SetAmount("8,000")
            .SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_POKET))
            .SetButtonAction(() => IAPManager.init.Purchase(IAPManager.COIN_POKET));
        _view.ProductCoin2
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.CoinBox))
            .SetDescription($"8,000 + <#E38B29>8,000 {LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Bonus)}</color>")
            .SetAmount("16,000")
            .SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.COIN_BOX))
            .SetButtonAction(() => IAPManager.init.Purchase(IAPManager.COIN_BOX));

        _view.ProductWatchAds
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.FreeCoin))
            .SetDescription($"{LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.FreeCoinDesc)} <#E38B29>{DataManager.init.gameData.currDailyCoinCount}/5</color>")
            .SetAmount("500")
            .SetButtonAction(() => {
                if (DataManager.init.gameData.currDailyCoinCount > 0)
                    AdsManager.init.ShowAdCoinRewarded();
            });
        _view.ProductDoubleCoin
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.DoubleCoin))
            .SetDescription(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.DoubleCoinDesc))
            .SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.DOUBLE_COIN))
            .SetButtonAction(() => IAPManager.init.Purchase(IAPManager.DOUBLE_COIN));
        _view.ProductPremium
            .SetTitle(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RemoveAds))
            .SetDescription(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RemoveAdsDesc))
            .SetPrice(IAPManager.init.GetLocalizedPriceString(IAPManager.PREMIUM))
            .SetButtonAction(() => IAPManager.init.Purchase(IAPManager.PREMIUM));

        _view.ButtonCoinPlus.onClick.AddListener(() => _view.ButtonMenus[2].onClick.Invoke());

        PlayerCoin.OnChangeValue += UpdateCoin;
        PlayerItem.OnChangeItem += (key) => UpdateItemCount();
        PlayerItem.OnChangeCurrDailyCoinCount += UpdateCurrDailyRewardCount;

        UpdateOnLockObject();
        UpdateOnLockWallpaper();
        UpdateItemCount();
        UpdateCoin();

        DailyRewardTimeUpdate().Forget();
    }

    private async UniTaskVoid DailyRewardTimeUpdate() {
        DataManager.init.gameData.initTimeDailyRewardTicks = DateTime.Today.AddDays(1).Ticks;


        var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        while (true) {
            if(DataManager.init.gameData.initTimeDailyRewardTicks < DateTime.Now.Ticks) {
                DataManager.init.gameData.currDailyCoinCount = 5;
                DataManager.init.gameData.initTimeDailyRewardTicks = DateTime.Now.AddMinutes(1).Ticks;//todayDate.AddDays(1).Ticks;
                PlayerItem.OnChangeCurrDailyCoinCount?.Invoke();
            }
            else {
                var time = new DateTime(DataManager.init.gameData.initTimeDailyRewardTicks - DateTime.Now.Ticks);
                _view.TextDailyRewardTime.text = $"{time.Hour:00}:{time.Minute:00}:{time.Second:00}";
            }

            _view.ProductWatchAds.Button.gameObject.SetActive(DataManager.init.gameData.currDailyCoinCount > 0);
            _view.TextDailyRewardTime.enabled = DataManager.init.gameData.currDailyCoinCount <= 0;

            await UniTask.Delay(1000);
        }
    }

    private void SetButton() {
        var viewToastMessage = ViewCanvas.Get<ViewCanvasToast>();

        int i = 0;
        foreach (var button in _view.ButtonMenus) {
            int index = i;
            button.onClick.AddListener(() => {
                OnSelectMenu?.Invoke(index);
            });

            i++;
        }

        i = 0;
        Array.Resize(ref DataManager.init.gameData.styleProducts, _view.ButtonObjectItems.Length);
        DataManager.init.gameData.styleProducts[0] = true;
        foreach (var button in _view.ButtonObjectItems) {
            int index = i;
            var item = button.GetComponent<ViewShopItem>();

            button.onClick.AddListener(() => OnSelectObjectItem(index));
            if (item != null) {
                item.SetObjectImage(index);
                item.PurchaseInfo.SetPrice().SetPurchaseButton(() => {
                    viewToastMessage.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.IsPurchase)
                        , () => {
                            if (PlayerCoin.Consume(item.PurchaseInfo.Price)) {
                                DataManager.init.gameData.styleProducts[index] = true;
                                UpdateOnLockObject();

                                OnSelectObjectItem?.Invoke(index);
                            }
                            else {
                                viewToastMessage.ShowOneTimeMessage("?????? ??????????.");
                            }
                        });
                });
            }
            i++;
        }

        i = 0;
        Array.Resize(ref DataManager.init.gameData.wallpaperProducts, _view.ButtonBackgroundItems.Length);
        DataManager.init.gameData.wallpaperProducts[0] = true;
        foreach (var button in _view.ButtonBackgroundItems) {
            int index = i;
            var item = button.GetComponent<ViewShopItem>();

            button.onClick.AddListener(() => OnSelectBackgroundItem(index));
            if(item != null) {
                item.PurchaseInfo.SetPrice().SetPurchaseButton(() => {
                    viewToastMessage.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.IsPurchase)
                        , () => {
                            if (PlayerCoin.Consume(item.PurchaseInfo.Price)) {
                                DataManager.init.gameData.wallpaperProducts[index] = true;
                                UpdateOnLockWallpaper();

                                OnSelectBackgroundItem?.Invoke(index);
                            } else {
                                viewToastMessage.ShowOneTimeMessage("?????? ??????????.");
                            }
                        });
                });
            }
            i++;
        }

        _view.ButtonMenus[0].onClick.Invoke();
        _view.ButtonObjectItems[DataManager.init.gameData.styleNum].onClick.Invoke();
        _view.ButtonBackgroundItems[DataManager.init.gameData.wallpaperNum].onClick.Invoke();
    }

    private void UpdateItemCount() {
        _view.TextDestruction.text = $"x{PlayerItem.GetCount(Definition.Item.Destruction)}";
        _view.TextRankUp.text = $"x{PlayerItem.GetCount(Definition.Item.RankUp)}";
        _view.TextReroll.text = $"x{PlayerItem.GetCount(Definition.Item.Reroll)}";
    }

    private void UpdateCurrDailyRewardCount() {
        _view.ProductWatchAds
            .SetDescription($"{LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.FreeCoinDesc)} <#E38B29>{DataManager.init.gameData.currDailyCoinCount}/5</color>");
    }

    private void UpdateMenuView(int index) {
        for (int i = 0; i < _view.ButtonMenus.Length; ++i) {
            _view.ShopMenus[i].SetActive(i == index);
            _view.UnLockMenu(i, i == index);
        }
    }

    private void UpdateObjects(int index) {
        if (DataManager.init.gameData.styleNum == index) {
            _view.ButtonObjectItems[index].SetLock(true);
            _view.ButtonObjectItems[index].GetComponent<ViewShopItem>().StartAnimation();
            return;
        }

        for (int i = 0; i < _view.ButtonObjectItems.Length; ++i) {
            _view.ButtonObjectItems[i].SetLock(i == index);
            var item = _view.ButtonObjectItems[i].GetComponent<ViewShopItem>();

            if (i == index)
                item.StartAnimation();
            else
                item.StopAnimation();
        }

        DataManager.init.gameData.styleNum = index;
        ObjectManager.init.SetObjectSprite(index);
        DataManager.init.Save();
    }

    private void UpdateOnLockObject() {
        int i = 0;
        foreach(var button in _view.ButtonObjectItems) {
            int index = i;
            button.enabled = DataManager.init.gameData.styleProducts[index];
            var item = button.GetComponent<ViewShopItem>();
            if(item != null) {
                item.SetLock(!DataManager.init.gameData.styleProducts[index]);
            }
            i++;
        }
    }

    private void UpdateBackgroundItems(int index) {
        for (int i = 0; i < _view.ButtonBackgroundItems.Length; ++i) {
            _view.ButtonBackgroundItems[i].SetLock(i == index);
        }

        if (DataManager.init.gameData.wallpaperNum == index)
            return;

        DataManager.init.gameData.wallpaperNum = index;
        DataManager.init.Save();
    }

    private void UpdateOnLockWallpaper() {
        int i = 0;
        foreach (var button in _view.ButtonBackgroundItems) {
            int index = i;
            button.enabled = DataManager.init.gameData.wallpaperProducts[index];
            var item = button.GetComponent<ViewShopItem>();
            if (item != null) {
                item.SetLock(!DataManager.init.gameData.wallpaperProducts[index]);
            }
            i++;
        }
    }

    private void UpdateCoin() {
        _view.CoinText.text = PlayerCoin.Coin.ToString();   
    }

    private void SetVisible(int index) {
        _view.SetActive(_navIndex == index);
    }
}