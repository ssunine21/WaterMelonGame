using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataInfo.Manager;
using DG.Tweening;
using DGExcepsion;
using UnityEngine;

public class ControllerBook
{
    private static int _navIndex = 2;
    private readonly ViewCanvasBook _view;

    private int _totalDiscoverCount;
    private int _currIndex;

    public ControllerBook(Transform parent)
    {
        _view = ViewCanvas.Create<ViewCanvasBook>(parent);
        _view.SetLocalizeText();
        
        foreach (var closeButton in _view.CloseButtons)
        {
            closeButton.onClick.AddListener(() =>
            {
                closeButton.enabled = false;
                _view.Wrapped.CloseToast(() => {
                    closeButton.enabled = true;
                    _view.SetActive(false);
                });
            });
        }
        
        ControllerMainMenu.OnBindMenu += UpdateVisible;
        
        Init().Forget();
    }

    private async UniTaskVoid Init()
    {
        ControllerMainMenu.OnBindSetActiveMenu(_navIndex, false);
        await UniTask.WaitUntil(() => ServerTime.IsTimeReady && DataManager.init.IsReady);
        ControllerMainMenu.OnBindSetActiveMenu(_navIndex, true);
        
        var i = 0;
        foreach (var bookTab in _view.ViewObjectBookTabs)
        {
            var index = i;
            bookTab.Button.onClick.AddListener(() =>
            {
                ChangeTab(index);
            });
            ++i;
        }
        
        _view.GetRewardButton.onClick.AddListener(() =>
        {
            if(!DataManager.init.gameData.isReceivedBookRewards[_currIndex])
            {
                PlayerBook.OnBindGetBookReward?.Invoke(false);
            }
        });
        _view.GetAdRewardButton.onClick.AddListener(() =>
        {
            if (DataManager.init.gameData.isPremium)
                PlayerBook.OnBindGetBookReward?.Invoke(true);
            else
                AdsManager.init.ShowRewardedInterstitialAd();
        });
        
        UpdateAllReddot();

        PlayerBook.OnFindBookObject += UpdateReddot;
        PlayerBook.OnBindGetBookReward += GetReward;
    }

    private void GetReward(bool isAds)
    {
        var rewardAmount = GetRewardAmount(_currIndex);
        DataScore.EarnExp(rewardAmount.Key);
        PlayerCoin.Earn(rewardAmount.Value);
        
        if(isAds)
        {
            EarnIconAnimation();
            DataManager.init.gameData.getBookAdRewardDateToStrings[_currIndex] = ServerTime.ToStringNextDay();
            DataManager.init.Save();
            _view.GetAdRewardButton.enabled = false;
        }
        else
        {
            DataManager.init.gameData.isReceivedBookRewards[_currIndex] = true;
            DataManager.init.Save();
        } 
        
        var fxCanvas = ViewCanvas.Get<ViewCanvasFx>();
        var menuCanvas = ViewCanvas.Get<ViewCanvasMainMenu>();
        fxCanvas.ItemExplosion(Vector2.zero, menuCanvas.Level.transform.position, 1f, GameManager.init.ExpIcon, 10);
        fxCanvas.ItemExplosion(Vector2.zero, menuCanvas.Option.transform.position, 1f, GameManager.init.GoldIcon, 10);

        ChangeTab(_currIndex);
        UpdateReddot(_currIndex);
    }

    private KeyValuePair<int, int> GetRewardAmount(int index)
    {
        var rewardData = DataManager.init.cloudData.cloudBookRewardDatas[index];
        var adRewardData = DataManager.init.cloudData.cloudBookAdRewardDatas[index];
        var isReceivedReward = DataManager.init.gameData.isReceivedBookRewards[index];

        return new KeyValuePair<int, int>(
            isReceivedReward ? adRewardData.exp : rewardData.exp,
            isReceivedReward ? adRewardData.gold : rewardData.gold);
    }

    private void ChangeTab(int index)
    {
        _currIndex = index;
        for (var i = 0; i < _view.ViewObjectBookTabs.Length; ++i)
        {
            _view.ViewObjectBookTabs[i].SetActiveView(i == index);
        }

        var rewardAmount = GetRewardAmount(index);
        _view.SetRewardData(rewardAmount.Key, rewardAmount.Value);
        
        UpdateObject(index);
    }

    private void UpdateAllReddot()
    {
        for (var i = 0; i < Definition.ObjectThemeCount; ++i)
        {
            UpdateReddot(i);
        }
    }

    private void UpdateReddot(int index)
    {

        if (!DataManager.init.gameData.isReceivedBookRewards[index])
        {
            _view.ViewObjectBookTabs[index].ViewReddot.SetReddot(
                DataManager.init.gameData.discoveredObjects[index].Count(x => x) >= 11);
            return;
        }
        
        DateTime.TryParse(DataManager.init.gameData.getBookAdRewardDateToStrings[index], out var lastTime);
        bool isReceivedReward = lastTime.Ticks - ServerTime.NowTime.Ticks > 0;
        
        _view.ViewObjectBookTabs[index].ViewReddot.SetReddot(!isReceivedReward);
    }

    private void UpdateObject(int index)
    {
        var currDiscoverCount = 0;
        var isDiscover = DataManager.init.gameData.discoveredObjects.Count > index;
        for (var i = 0; i < _view.ViewObjectBooks.Length; ++i)
        {
            if (isDiscover)
            {
                isDiscover = DataManager.init.gameData.discoveredObjects[index][i];
            }

            if (isDiscover)
                currDiscoverCount++;
                
            _view.ViewObjectBooks[i]
                .SetObjectImage(Resources.Load<Sprite>($"obj/Obj{index}_{i}"))
                .SetActiveObject(isDiscover);
        }
        _view.TabSliderAnimation.StartAnimate(currDiscoverCount, 11, 1f);

        DataManager.init.gameData.isReceivedBookRewards ??= new List<bool>();
        if (DataManager.init.gameData.isReceivedBookRewards.Count < Definition.ObjectThemeCount)
        {
            for (var i = DataManager.init.gameData.isReceivedBookRewards.Count; i < Definition.ObjectThemeCount; ++i)
            {
                DataManager.init.gameData.isReceivedBookRewards.Add(false);
            }
        }
        
        UpdateButtonView(index, currDiscoverCount);
    }

    private void UpdateButtonView(int index, int currDiscoverCount)
    {
        bool isAllDiscover = currDiscoverCount >= 11;
        
        _view.GetRewardButtonLockPanel.SetActive(!isAllDiscover);
        _view.GetRewardButton.enabled = isAllDiscover;
        
        if (!DataManager.init.gameData.isReceivedBookRewards[index])
        {
            _view.GetRewardButton.gameObject.SetActive(true);
            _view.GetAdRewardButton.gameObject.SetActive(false);
            _view.GetRewardIcon.gameObject.SetActive(false);
        }
        else
        {
            _view.GetRewardButton.gameObject.SetActive(false);
            _view.GetAdRewardButton.gameObject.SetActive(true);

            if (DataManager.init.gameData.getBookAdRewardDateToStrings == null)
            {
                DataManager.init.gameData.getBookAdRewardDateToStrings = new List<string>();
                for (var i = 0; i < Definition.ObjectThemeCount; ++i)
                    DataManager.init.gameData.getBookAdRewardDateToStrings.Add(ServerTime.ToStringYesterday());
                
                DataManager.init.Save();
            }

            if (DateTime.TryParse(DataManager.init.gameData.getBookAdRewardDateToStrings[index], out var lastTime))
            {
                bool isReceivedReward = lastTime.Ticks - ServerTime.NowTime.Ticks > 0;
                _view.GetAdRewardButton.enabled = !isReceivedReward;
                _view.GetRewardIcon.gameObject.SetActive(isReceivedReward);
            }
        }
    }

    private void UpdateVisible(int index)
    {
        _view.SetActive(index == _navIndex);
        if (index == _navIndex) {
            _view.Wrapped.ShowToast();
            ChangeTab(0);

            _totalDiscoverCount = 0;
            foreach (var discoveredObjects in DataManager.init.gameData.discoveredObjects)
            {
                foreach (var isDiscover in discoveredObjects)
                {
                    if (isDiscover)
                        _totalDiscoverCount++;
                }
            }
            
            _view.AllSliderAnimation.StartAnimate(_totalDiscoverCount, 11 * Definition.ObjectThemeCount, 1.5f);
        }
    }
    
    private void EarnIconAnimation()
    {
        var image = _view.GetRewardIcon;

        if (image == null) return;

        image.gameObject.SetActive(true);
        Color currColor = image.color;
        Color nextColor = image.color;
        currColor.a = 0;
        image.color = currColor;
        image.transform.localScale = Vector3.one * 5;
        image.DOColor(nextColor, 0.3f);
        image.transform.DOScale(Vector3.one, 0.3f);
    }
}