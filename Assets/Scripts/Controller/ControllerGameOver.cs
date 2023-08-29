using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class ControllerGameOver {
    private readonly ViewCanvasGameOver _view;
    private bool isCancel = false;
    private float _textSocreYPos = 500f;

    public ControllerGameOver(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasGameOver>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "UI";

        _view.ButtonCancel.onClick.AddListener( () => {
            if (isCancel) {
                ObjectManager.init.MainObjects.Clear();
                GameManager.OnBindGoHome?.Invoke();
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
    }

    private void UpdateVisible(bool flag) {
        _view.SetActive(flag);
        _view.Panel.color = new Color(0, 0, 0, 0.8f);

        if (flag)
            StartTimer().Forget();
    }

    private async UniTaskVoid StartTimer()
    {
        _view.PanelCoin.SetActive(false);
        _view.PanelTimer.SetActive(true);
        _view.TextTotalScore.gameObject.SetActive(false);
        _view.TextTempTotalScore.gameObject.SetActive(false);

        if (DataManager.init.gameData.viewAdsCount == 0) {
            for (int i = 0; i <= 5; ++i) {
                _view.Timer.fillAmount = 1;
                _view.TextTimer.text = (5 - i).ToString();
                _view.Timer.DOFillAmount(0, 0.7f).SetEase(Ease.InOutExpo);

                float timer = 0;
                while (timer < 1) {
                    if (isCancel)
                        break;

                    timer += Time.deltaTime;
                    await UniTask.Yield();
                }
            }
        }

        if (!_view.IsActiveSelf)
            return;

        isCancel = true;

        _view.PanelTimer.SetActive(false);
        _view.TextTotalScore.gameObject.SetActive(true);

        int curr = 0;
        int score = DataScore.CurrScore;
        _view.Panel.DOColor(new Color(0, 0, 0, 0.9f), 0.5f);
        _view.RectTotalScore.DOAnchorPosY(_textSocreYPos, 0.5f);

        // after timer -> score setting
        float elapsedTime = 0f;
        float duration = 1f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime * 7f;
            float t = Mathf.Clamp01(elapsedTime / duration);
            int currentValue = (int)Mathf.Lerp(curr, score, t);
            _view.TextTotalScore.text = currentValue.ToString();
            await UniTask.Delay(10);
        }

        int finalScore = int.Parse(_view.TextTotalScore.text);
        finalScore += finalScore / 10;

        DataScore.SetCurrScore(finalScore);
        DataScore.SetBestScore(finalScore);

        int coin = Mathf.Clamp(Mathf.FloorToInt(finalScore * 0.013f), 0, 3500);
        coin *= DataManager.init.gameData.isDoubleCoin ? 2 : 1;

        _view.SetCoin(coin);
        PlayerCoin.Earn(coin);

        await UniTask.Delay(700);

        _view.PanelCoin.SetActive(true);
        _view.TextTempTotalScore.gameObject.SetActive(true);
        _view.TextTotalScore.text = finalScore.ToString();

        _view.TextTempTotalScore.text = _view.TextTotalScore.text;
        _view.TextTempTotalScore.GetComponent<RectTransform>().DOAnchorPosY(_textSocreYPos + 150, 0.3f);
        _view.TextTempTotalScore.DOFade(0, 0.5f);
    }
}