using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Tables;

public class ControllerGameOver {
    private readonly ViewCanvasGameOver _view;
    private bool isCancel = false;

    public ControllerGameOver(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasGameOver>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "UI";
        
        _view.ButtonCancel.onClick.AddListener( () => {
            if (isCancel) {
                DataManager.init.gameData.playTime += 1;
                ObjectManager.init.MainObjects.Clear();
                GameManager.OnBindGoHome?.Invoke();

                if (DataManager.init.gameData.playTime == 2)
                    GoogleReview.init.ShowStoreReview();
                else
                    AdsManager.init.ShowInterstitialAd();

                isCancel = false;
            } else
                isCancel = true;
        });

        _view.ButtonStartAds.onClick.AddListener(() =>
        {
            if (DataManager.init.gameData.isPremium)
                AdsManager.init.HandleUserRestartGameReward();
            else
                AdsManager.init.ShowAdRestartGame();
        });

        GameManager.OnBindGameOver += () => UpdateVisible(true);
        GameManager.OnBindGoHome += () => UpdateVisible(false);

        _view.SetLocalize();
        MainTask().Forget();
    }

    private Sequence _sequence;

    private async UniTaskVoid MainTask()
    {
        //0
        //130
        //-35
        //-140
        //315
        //-315
        _sequence = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(_view.ContentRectTr.DOAnchorPosY(0f, 0.8f).SetEase(Ease.OutBack))
            .Insert(0.5f, _view.ScoreRectTr.DOAnchorPosY(145f, 0.6f).SetEase(Ease.OutBack))
            .Insert(0.16f, _view.LevelTr.DOAnchorPosY(-125f, 0.8f).SetEase(Ease.OutBack))
            .Insert(0.24f, _view.HomeRectTr.DOAnchorPosY(-300f, 0.7f).SetEase(Ease.OutBack))
            .Insert(0.32f, _view.CoinTr.DOAnchorPosY(-20f, 0.6f).SetEase(Ease.OutBack))
            .Insert(0.36f, _view.WatchAdsRectTr.DOAnchorPosY(-300f, 0.8f).SetEase(Ease.OutBack));
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MovingTest();
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private void MovingTest()
    {
        _sequence.Restart();
    }

    private void UpdateVisible(bool flag) {
        _view.SetActive(flag);

        Vector3 anchoredPosition = _view.HomeRectTr.anchoredPosition;
        anchoredPosition.x = DataManager.init.gameData.viewAdsCount == 1 ? 0 : -185;
        _view.WatchAdsRectTr.gameObject.SetActive(DataManager.init.gameData.viewAdsCount != 1);
        _view.HomeRectTr.anchoredPosition = anchoredPosition;
        
        StartTimer().Forget();
    }

    private async UniTaskVoid StartTimer()
    {
        _sequence.Restart();
        if (!_view.IsActiveSelf)
            return;
        isCancel = true;
        _view.TextTotalScore.gameObject.SetActive(true);
        _view.BestScoreText.text = $"BEST : {DataScore.BestScore}";

        Vector3 tagPsotion = _view.TagRectTr.anchoredPosition;
        tagPsotion.y = -60;
        _view.TagRectTr.anchoredPosition = tagPsotion;

        int curr = 0;
        int score = DataScore.CurrScore;

        int coinCurr = 0;
        int coinScore = Mathf.Clamp(Mathf.FloorToInt(score * 0.013f), 0, 3500);
        
        SetExp(Mathf.Clamp(Mathf.FloorToInt(score * 0.013f), 0, 999)).Forget();
        
        // after timer -> score setting
        float elapsedTime = 0f;
        float duration = 8f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime * 7f;
            float t = Mathf.Clamp01(elapsedTime / duration);
            int currentValue = (int)Mathf.Lerp(curr, score, t);
            int currentCoinValue = (int)Mathf.Lerp(curr, coinScore, t);
            
            _view.TextTotalScore.text = currentValue.ToString();
            _view.SetCoin(currentCoinValue);
            
            await UniTask.Delay(10);
        }

        int finalScore = int.Parse(_view.TextTotalScore.text);
        finalScore += finalScore / 10;

        bool isBest = DataScore.BestScore < finalScore;
        
        DataScore.SetCurrScore(finalScore);
        DataScore.SetBestScore(finalScore);

        int coin = Mathf.Clamp(Mathf.FloorToInt(finalScore * 0.013f), 0, 3500);
        coin *= DataManager.init.gameData.isDoubleCoin ? 2 : 1;

        _view.SetCoin(coin);
        PlayerCoin.Earn(coin);
        await UniTask.Delay(1000);
        if (isBest)
        {
            _view.BestScoreText.text = $"NEW BEST : {DataScore.BestScore}";
            _view.TagRectTr.DOAnchorPosY(30f, 0.8f).SetEase(Ease.OutBack);
        }
        _view.TextTotalScore.text = finalScore.ToString();
    }

    private async UniTaskVoid SetExp(int earnExp)
    {
        float elapsedTime = 0f;
        float duration = 13f;

        int curr = DataScore.Exp;
        int next = curr + earnExp;
        DataScore.EarnExp(earnExp);
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime * 7f;
            float t = Mathf.Clamp01(elapsedTime / duration);
            
            tempExp = Mathf.Lerp(curr, next, t);
            ModularExpView(out int level, out float exp, out float maxExp);
            
            _view
                .SetLevel(level + 1)
                .SetFillAmount(exp, maxExp);
            
            await UniTask.Delay(10);

            earnExp--;
        }
    }

    private float tempExp;
    public void ModularExpView(out int level, out float currExp, out float maxExp)
    {
        float exp = tempExp;
        float needExp;
        int currLevel = 0;
        while (true)
        {
            needExp = Mathf.FloorToInt(currLevel * 5) + 50;
            if (needExp > exp)
                break;
            else
            {
                exp -= needExp;
                currLevel++;
            }
        }
        level = currLevel;
        currExp = exp;
        maxExp = needExp;
    }
}