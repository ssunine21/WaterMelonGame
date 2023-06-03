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

    private BannerView bannerView;
    private InterstitialAd interstitialAd;

    private RewardedAd coinRewardedAd;
    private RewardedAd rankUpItemRewardedAd;
    private RewardedAd destroyItemRewardedAd;
    private RewardedAd rerollItemRewardedAd;
    private RewardedAd restartGameRewardedAd;

    private bool isPrevSoundOn = false;

    public void Start() {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => {
        });

        RequestRewardedAd();
        RquestInterstitialAd();

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

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

        this.bannerView.LoadAd(CreateAdRequest());
        AboveAds();
    }

    private AdRequest CreateAdRequest() {
        return new AdRequest.Builder().Build();
    }

    private void RquestInterstitialAd() {
#if UNITY_ANDROID
        string adUnitId = AND_INTERSTITIAL_ID;
#elif UNITY_IPHONE
        string adUnitId = iOS_INTERSTITIAL_ID;
#else
        string adUnitId = "unexpected_platform";
#endif
        if (interstitialAd != null) {
            interstitialAd.Destroy();
        }

        this.interstitialAd = new InterstitialAd(adUnitId);

        this.interstitialAd.OnAdClosed += this.HandleOnAdClosed;
        this.interstitialAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;

        this.interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd() {
        if (DataManager.init.gameData.isPremium)
            return;

        if (interstitialAd.IsLoaded()) {
            interstitialAd.Show();
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args) {
        RquestInterstitialAd();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        Debug.Log($"{sender} loaded is fail : {args.Message}");
    }

    private void AboveAds() {

        Vector3 adsAbovePos = new Vector3(0, this.bannerView.GetHeightInPixels() / 300, 0);

        Camera.main.transform.position -= adsAbovePos;
    }

    private void RequestRewardedAd() {
        this.coinRewardedAd = CreateAndLoadRewardedAd();
        this.coinRewardedAd.OnUserEarnedReward += HandleUserCoinReward;

        this.rankUpItemRewardedAd = CreateAndLoadRewardedAd();
        this.rankUpItemRewardedAd.OnUserEarnedReward += HandleUserRankUpItemReward;

        this.destroyItemRewardedAd = CreateAndLoadRewardedAd();
        this.destroyItemRewardedAd.OnUserEarnedReward += HandleUserDestroyItemReward;

        this.rerollItemRewardedAd = CreateAndLoadRewardedAd();
        this.rerollItemRewardedAd.OnUserEarnedReward += HandleUserRerollItemReward;

        this.restartGameRewardedAd = CreateAndLoadRewardedAd();
        this.restartGameRewardedAd.OnUserEarnedReward += HandleUserRestartGameReward;
    }

    public RewardedAd CreateAndLoadRewardedAd() {
#if UNITY_ANDROID
        string adUnitId = AND_REWARD_ID;
#elif UNITY_IPHONE
        string adUnitId = iOS_REWARD_ID;
#else
        string adUnitId = "unexpected_platform";
#endif
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoaded;

        rewardedAd.LoadAd(CreateAdRequest());
        return rewardedAd;
    }

    public void ShowAdCoinRewarded() {
        if (this.coinRewardedAd.IsLoaded()) {
            this.coinRewardedAd.Show();
        }
    }

    public void ShowAdRankUpItem(Action callback) {
        if (this.rankUpItemRewardedAd.IsLoaded()) {
                this.rankUpItemRewardedAd.Show();
            callback?.Invoke();
        }
    }

    public void ShowAdDestroyItem(Action callback)
    {
        if (this.destroyItemRewardedAd.IsLoaded())
        {
            this.destroyItemRewardedAd.Show();
            callback?.Invoke();
        }
    }

    public void ShowAdRerollItem(Action callback)
    {
        if (this.rerollItemRewardedAd.IsLoaded())
        {
            this.rerollItemRewardedAd.Show();
            callback?.Invoke();
        }
    }

    public void ShowAdRestartGame()
    {
       if(this.restartGameRewardedAd.IsLoaded()) {
            this.restartGameRewardedAd.Show();
        }
    }

    public void HandleUserCoinReward(object sender, Reward args) {
        PlayerCoin.Earn(500);
        DataManager.init.gameData.currDailyCoinCount--;
        DataManager.init.Save();
        PlayerItem.OnChangeCurrDailyCoinCount?.Invoke();
    }

    public void HandleUserRankUpItemReward(object sender, Reward args)
    {
        ObjectManager.init.RankUpItem();
    }

    public void HandleUserRerollItemReward(object sender, Reward args)
    {
        ObjectManager.init.RerollItem();
    }

    public void HandleUserDestroyItemReward(object sender, Reward args)
    {
        ObjectManager.init.DestroyItem(4);
    }

    public void HandleUserRestartGameReward(object sender, Reward args)
    {
        ViewCanvas.Get<ViewCanvasGameOver>().SetActive(false);
        ObjectManager.init.DestroyHalf();
        GameManager.IsGamePause = false;
        DataManager.init.gameData.viewAdsCount = 1;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args) {
        AudioManager.Init.Mute(true);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        if (sender == coinRewardedAd) {
            coinRewardedAd = CreateAndLoadRewardedAd();
            this.coinRewardedAd.OnUserEarnedReward += HandleUserCoinReward;
        } else if (sender == rankUpItemRewardedAd) {
            rankUpItemRewardedAd = CreateAndLoadRewardedAd();
            this.rankUpItemRewardedAd.OnUserEarnedReward += HandleUserRankUpItemReward;
        }
        else if (sender == destroyItemRewardedAd)
        {
            destroyItemRewardedAd = CreateAndLoadRewardedAd();
            this.destroyItemRewardedAd.OnUserEarnedReward += HandleUserDestroyItemReward;
        }
        else if (sender == rerollItemRewardedAd)
        {
            rerollItemRewardedAd = CreateAndLoadRewardedAd();
            this.rerollItemRewardedAd.OnUserEarnedReward += HandleUserRerollItemReward;
        }
        else if (sender == restartGameRewardedAd){
            restartGameRewardedAd = CreateAndLoadRewardedAd();
            this.restartGameRewardedAd.OnUserEarnedReward += HandleUserRestartGameReward;
        }

        AudioManager.Init.Mute(false);
    }

    public void HandleRewardedAdFailedToLoaded(object sender, AdErrorEventArgs args) {
        Debug.Log($"{sender} loaded is fail : {args.Message}");
    }

    public void DestroyBannerAd() {
        if (this.bannerView != null)
            this.bannerView.Destroy();
    }
}

