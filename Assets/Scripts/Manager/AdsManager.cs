using System;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour {
    // Android
    // 배너 광고   ca-app-pub-3940256099942544/6300978111
    // 전면 광고   ca-app-pub-3940256099942544/1033173712
    // 보상형 광고  ca-app-pub-3940256099942544/5224354917

    // iOS
    // 배너 광고   ca-app-pub-3940256099942544/2934735716
    // 전면 광고   ca-app-pub-3940256099942544/4411468910
    // 보상형 광고  ca-app-pub-3940256099942544/1712485313

    private static readonly string AND_BANNER_ID = "ca-app-pub-7832687788012663/9321714808";
    private static readonly string AND_INTERSTITIAL_ID = "ca-app-pub-7832687788012663/1332806899";
    private static readonly string AND_REWARD_ID = "ca-app-pub-7832687788012663/3605180232";

    private static readonly string iOS_BANNER_ID = "ca-app-pub-7832687788012663/6504877560";
    private static readonly string iOS_INTERSTITIAL_ID = "ca-app-pub-7832687788012663/1679720432";
    private static readonly string iOS_REWARD_ID = "ca-app-pub-7832687788012663/8965994142";

    //private static readonly string iOS_BANNER_ID = "ca-app-pub-3940256099942544/2934735716";
    //private static readonly string iOS_INTERSTITIAL_ID = "ca-app-pub-3940256099942544/4411468910";
    //private static readonly string iOS_REWARD_ID = "ca-app-pub-3940256099942544/1712485313";

    public static AdsManager init = null;
    List<string> deviceIds = new List<string>();

    private void Awake() {
        if (init == null) {
            init = this;
        } else if (init != this) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;

    private RewardedAd _coinRewardedAd;
    private RewardedAd _rankUpItemRewardedAd;
    private RewardedAd _destroyItemRewardedAd;
    private RewardedAd _rerollItemRewardedAd;
    private RewardedAd _restartGameRewardedAd;

    public void Start() {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => {
        });

        LoadCoinRewardedAd();
        LoadDestroyRewardedAd();
        LoadRankUpRewardedAd();
        LoadRerollRewardedAd();
        LoadRestartGameRewardedAd();

        LoadInterstitialAd();

        GameManager.OnBindNewGame += RequestBannerAd;
        GameManager.OnBindStartGame += RequestBannerAd;
        GameManager.OnBindGoHome += DestroyBannerAd;
    }

    private void RequestBannerAd()
    {
        if (DataManager.init.gameData.isPremium)
            return;

#if UNITY_ANDROID
        string adUnitId = AND_BANNER_ID;
#elif UNITY_IPHONE
        string adUnitId = iOS_BANNER_ID;
#else
            string adUnitId = "unexpected_platform";
#endif

        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        var adRequest = new AdRequest();
        
        _bannerView.LoadAd(adRequest);
        AboveAds();
    }

    private void LoadInterstitialAd() {
#if UNITY_ANDROID
        string adUnitId = AND_INTERSTITIAL_ID;
#elif UNITY_IPHONE
        string adUnitId = iOS_INTERSTITIAL_ID;
#else
        string adUnitId = "unexpected_platform";
#endif

        InterstitialAd.Load(adUnitId, new AdRequest(),
            (InterstitialAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null)
                {
                    Debug.Log("Interstitial ad failed to load with error: " +
                               loadAdError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    Debug.Log("Interstitial ad failed to load.");
                    return;
                }

                Debug.Log("Interstitial ad loaded.");
                _interstitialAd = ad;
            });
    }

    public void ShowInterstitialAd() {
        if (DataManager.init.gameData.isPremium)
            return;

        if(_interstitialAd != null & _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad cannot be shown.");
        }
    }

    private void AboveAds() {
        
        Vector3 adsAbovePos = new Vector3(0, this._bannerView.GetHeightInPixels() / 300, 0);

      //  Camera.main.transform.position -= adsAbovePos;
    }

    private string GetRewardId()
    {
#if UNITY_ANDROID
        return AND_REWARD_ID;
#elif UNITY_IPHONE
        return iOS_REWARD_ID;
#else
        return "unexpected_platform";
#endif
    }

    private void LoadCoinRewardedAd()
    {
        string adUnitId = GetRewardId();

        if(_coinRewardedAd != null)
        {
            DestroyAd(_coinRewardedAd);
        }

        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null) return;
                else if (ad == null) return;
                _coinRewardedAd = ad;
                OnAdFullScreenContentClosed(_coinRewardedAd, LoadCoinRewardedAd);
            });
    }

    private void LoadRankUpRewardedAd()
    {
        string adUnitId = GetRewardId();
        if (_rankUpItemRewardedAd != null)
        {
            DestroyAd(_rankUpItemRewardedAd);
        }
        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null) return;
                else if (ad == null) return;
                _rankUpItemRewardedAd = ad;
                OnAdFullScreenContentClosed(_rankUpItemRewardedAd, LoadRankUpRewardedAd);
            });
    }

    private void LoadDestroyRewardedAd()
    {
        string adUnitId = GetRewardId();
        if (_destroyItemRewardedAd != null)
        {
            DestroyAd(_destroyItemRewardedAd);
        }
        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null) return;
                else if (ad == null) return;
                _destroyItemRewardedAd = ad;
                OnAdFullScreenContentClosed(_destroyItemRewardedAd, LoadDestroyRewardedAd);
            });
    }

    private void LoadRerollRewardedAd()
    {
        string adUnitId = GetRewardId();
        if (_rerollItemRewardedAd != null)
        {
            DestroyAd(_rerollItemRewardedAd);
        }
        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null) return;
                else if (ad == null) return;
                _rerollItemRewardedAd = ad;
                OnAdFullScreenContentClosed(_rerollItemRewardedAd, LoadRerollRewardedAd);
            });
    }

    private void LoadRestartGameRewardedAd()
    {
        string adUnitId = GetRewardId();
        if (_restartGameRewardedAd != null)
        {
            DestroyAd(_restartGameRewardedAd);
        }
        RewardedAd.Load(adUnitId, new AdRequest(),
            (RewardedAd ad, LoadAdError loadAdError) =>
            {
                if (loadAdError != null) return;
                else if (ad == null) return;
                _restartGameRewardedAd = ad;
                OnAdFullScreenContentClosed(_restartGameRewardedAd, LoadRestartGameRewardedAd);
            });
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    private void DestroyAd(RewardedAd ad)
    {
        if(ad != null)
        {
            ad.Destroy();
            ad = null;
        }
    }

    private void OnAdFullScreenContentClosed(RewardedAd ad, Action action)
    {
        ad.OnAdFullScreenContentClosed += action;
    }

    public void ShowAdCoinRewarded() {
        if (_coinRewardedAd != null && _coinRewardedAd.CanShowAd()) {
            _coinRewardedAd.Show((Reward reward) =>
            {
                HandleUserCoinReward();
            });
        }
    }

    public void ShowAdRankUpItem(Action callback) {
        if (_rankUpItemRewardedAd != null && _rankUpItemRewardedAd.CanShowAd())
        {
            _rankUpItemRewardedAd.Show((Reward reward) =>
            {
                HandleUserRankUpItemReward();
                callback.Invoke();
            });
        }
    }

    public void ShowAdDestroyItem(Action callback)
    {
        if (_destroyItemRewardedAd != null && _destroyItemRewardedAd.CanShowAd())
        {
            _destroyItemRewardedAd.Show((Reward reward) =>
            {
                HandleUserDestroyItemReward();
                callback.Invoke();
            });
        }
    }

    public void ShowAdRerollItem(Action callback)
    {
        if (_rerollItemRewardedAd != null && _rerollItemRewardedAd.CanShowAd())
        {
            _rerollItemRewardedAd.Show((Reward reward) =>
            {
                HandleUserRerollItemReward();
                callback.Invoke();
            });
        }
    }

    public void ShowAdRestartGame()
    {
        if (_restartGameRewardedAd != null && _restartGameRewardedAd.CanShowAd())
        {
            _restartGameRewardedAd.Show((Reward reward) =>
            {
                HandleUserRestartGameReward();
            });
        }
    }

    public void HandleUserCoinReward() {
        PlayerCoin.Earn(300);
        DataManager.init.gameData.currDailyCoinCount--;
        PlayerItem.OnChangeCurrDailyCoinCount?.Invoke();

        DataManager.init.Save();
    }

    public void HandleUserRankUpItemReward()
    {
        ObjectManager.init.RankUpItem();
    }

    public void HandleUserRerollItemReward()
    {
        ObjectManager.init.RerollItem();
    }

    public void HandleUserDestroyItemReward()
    {
        ObjectManager.init.DestroyItem(4);
    }

    public void HandleUserRestartGameReward()
    {
        ViewCanvas.Get<ViewCanvasGameOver>().SetActive(false);
        ObjectManager.init.DestroyHalf();
        GameManager.IsGamePause = false;
        DataManager.init.gameData.viewAdsCount = 1;
    }

    public void DestroyBannerAd() {
        if (this._bannerView != null)
            this._bannerView.Destroy();
    }
}

