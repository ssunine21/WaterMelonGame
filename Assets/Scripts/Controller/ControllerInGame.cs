using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ControllerInGame {
    private readonly ViewCanvasInGame _view;

    private bool _isHoldingBall = false;
    private bool _isGameStart = false;   

    public ControllerInGame(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasInGame>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "UI";

        _view.ButtonBack.onClick.AddListener(() =>
        {
            if (GameManager.IsGamePause == false)
                GameManager.OnBindGoHome?.Invoke();
        });
        
        _view.ButtonDestroyItem.onClick.AddListener(() => {
            var viewToastMessage = ViewCanvas.Get<ViewCanvasToast>();
            viewToastMessage.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.DestructionDesc), () => {
                var key = Definition.Item.Destruction;
                if (PlayerItem.GetCount(key) > 0) {
                    PlayerItem.Comsume(key);
                    ObjectManager.init.DestroyItem(4);
                }
                else if (_view.DestoryItemAdsPanel.activeSelf)
                {

                    if (DataManager.init.gameData.isPremium)
                    {
                        AdsManager.init.HandleUserDestroyItemReward();
                        DataManager.init.gameData.watchAdsDestroyItem = true;
                        DataManager.init.gameData.watchAdsDestroyItemCount--;
                        UpdateItemCount();
                    }
                    else
                        AdsManager.init.ShowAdDestroyItem(() =>
                        {
                            DataManager.init.gameData.watchAdsDestroyItem = true;
                            DataManager.init.gameData.watchAdsDestroyItemCount--;
                            UpdateItemCount();
                        });
                }
            });
        });
        
        _view.ButtonRankUpItem.onClick.AddListener(() => {
            var viewToastMessage = ViewCanvas.Get<ViewCanvasToast>();
            viewToastMessage.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RankUpDesc), () => {
                var key = Definition.Item.RankUp;
                if (PlayerItem.GetCount(key) > 0) {
                    PlayerItem.Comsume(key);
                    ObjectManager.init.RankUpItem();
                }
                else if (_view.RankUpItemAdsPanel.activeSelf)
                {
                    if (DataManager.init.gameData.isPremium)
                    {
                        AdsManager.init.HandleUserRankUpItemReward();
                        DataManager.init.gameData.watchAdsRankupItem = true;
                        DataManager.init.gameData.watchAdsRankupItemCount--;
                        UpdateItemCount();
                    }
                    else
                        AdsManager.init.ShowAdRankUpItem(() =>
                        {
                            DataManager.init.gameData.watchAdsRankupItem = true;
                            DataManager.init.gameData.watchAdsRankupItemCount--;
                            UpdateItemCount();
                        });
                }
            });
        });
        _view.ButtonRerollItem.onClick.AddListener(() => {
            var viewToastMessage = ViewCanvas.Get<ViewCanvasToast>();
            viewToastMessage.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.RerollDesc), () => {
                var key = Definition.Item.Reroll;
                if (PlayerItem.GetCount(key) > 0)
                {
                    PlayerItem.Comsume(key);
                    ObjectManager.init.RerollItem();
                }
                else if (_view.RerollItemAdsPanel.activeSelf)
                {
                    if (DataManager.init.gameData.isPremium)
                    {
                        AdsManager.init.HandleUserRerollItemReward();
                        DataManager.init.gameData.watchAdsRerollItem = true;
                        DataManager.init.gameData.watchAdsRerollItemCount--;
                        UpdateItemCount();
                    }
                    else
                        AdsManager.init.ShowAdRerollItem(() =>
                        {
                            DataManager.init.gameData.watchAdsRerollItem = true;
                            DataManager.init.gameData.watchAdsRerollItemCount--;
                            UpdateItemCount();
                        });
                }
            });
        });
        _view.ButtonShowObjBook.onClick.AddListener(() =>
        {
            var viewBook = ViewCanvas.Get<ViewCanvasObjectBook>();
            viewBook.Open();
        });
        
        GameManager.OnBindNewGame += InitNewStart;
        GameManager.OnBindStartGame += InitStartStart;
        GameManager.OnBindGoHome += GameEnd;

        CoCurrSocreFlow().Forget();

        PlayerItem.OnChangeItem += (key) => UpdateItemCount();


        //_view.Underground.position = new Vector2(0, Camera.main.ScreenToWorldPoint(Vector2.one * GameManager.GroundHeight).y);
    }

    private void InitNewStart()
    {
        DataManager.init.gameData.watchAdsDestroyItem = false;
        DataManager.init.gameData.watchAdsRankupItem = false;
        DataManager.init.gameData.watchAdsRerollItem = false;
        
        DataManager.init.gameData.watchAdsDestroyItemCount = 2;
        DataManager.init.gameData.watchAdsRankupItemCount = 2;
        DataManager.init.gameData.watchAdsRerollItemCount = 2;
        
        UpdateView();
        InitObject();
        Main().Forget();
    }

    private void InitStartStart() {
        UpdateView();
        if (DataManager.init.gameData.objectData == null
            || DataManager.init.gameData.objectData.Count == 0) {
            InitObject();
        }
        Main().Forget();
    }

    private void InitObject() {
        DataScore.SetCurrScore(0);
        _view.SetCurrScore(0);
        DataManager.init.gameData.viewAdsCount = 0;
        DataManager.init.gameData.currObjectKey = ObjectManager.ObjectKey.Zero;
        DataManager.init.gameData.nextObjectKey = ObjectManager.init.GetRandomKey();
        DataManager.init.gameData.objectData = new List<DataInfo.GameObjectData>();
    }

    private void UpdateView() {
        _view.SetActive(true);
        
        var topY = _view.Underground.anchoredPosition.y + (_view.Underground.rect.height / 2);
        var screenPos = new Vector2(_view.Underground.position.x, topY);
        if (Camera.main != null)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            MaxLine.init.SetUndergroundPositionY(worldPos.y - 1);
        }

        var bottomY = _view.Up.anchoredPosition.y - (_view.Up.rect.height / 2);
        var screenPos1 = new Vector2(_view.Up.position.x, bottomY);
        if (Camera.main != null)
        {
            var worldPos1 = Camera.main.ScreenToWorldPoint(screenPos1);
            MaxLine.init.SetMaxLinePositionY(_view.Up.position.y);
        }

        UpdateScore();
        UpdateItemCount();
    }

    private void GameEnd() {
        _isGameStart = false;
        _view.SetActive(false);

        if (!DataManager.init.gameData.isAttendanceFirstOpen)
        {
            ControllerMainMenu.OnBindMenu?.Invoke(1);
            DataManager.init.gameData.isAttendanceFirstOpen = true;
            DataManager.init.Save();
        }
    }

    private void UpdateItemCount()
    {
        int destroyCount = DataManager.init.gameData.destroyItemCount;
        int rankupCount = DataManager.init.gameData.rankupItemCount;
        int rerollCount = DataManager.init.gameData.rerollItemCount;

        _view.TextDestoryItemCount.text = destroyCount == 0 && DataManager.init.gameData.watchAdsDestroyItemCount > 0 ? "" : destroyCount.ToString();
        _view.TextRankUpItemCount.text = rankupCount == 0 && DataManager.init.gameData.watchAdsRankupItemCount > 0 ? "" : rankupCount.ToString();
        _view.TextRerollItemCount.text = rerollCount == 0 && DataManager.init.gameData.watchAdsRerollItemCount > 0 ? "" : rerollCount.ToString();

        _view.DestoryItemAdsPanel.SetActive(DataManager.init.gameData.watchAdsDestroyItemCount > 0 && destroyCount == 0);
        _view.RankUpItemAdsPanel.SetActive(DataManager.init.gameData.watchAdsRankupItemCount > 0 && rankupCount == 0);
        _view.RerollItemAdsPanel.SetActive(DataManager.init.gameData.watchAdsRerollItemCount > 0 && rerollCount == 0);

        DataManager.init.Save();
    }
    
    private void UpdateScore() {
        _view
            .SetCurrScore(DataScore.CurrScore)
            .SetBestScore(DataScore.BestScore);
    }

    async UniTaskVoid Main() {
        _isGameStart = true;
        GameManager.IsGamePause = false;
        foreach (var objInfo in DataManager.init.gameData.objectData) {
            var obj = ObjectManager.init.GetObject(objInfo.mergeLevel);
            obj.transform.position = new Vector2(objInfo.xPos, objInfo.yPos);
            obj.Setting();
        }

        var currBall = ObjectManager.init.GetObject(DataManager.init.gameData.currObjectKey);
        var nextBallKey = DataManager.init.gameData.nextObjectKey;
        _view.ViewNextObject.SetSprite(ObjectManager.init.ObjectSprites[(int)nextBallKey]);
        
        bool isButtonClick;
        Vector3 mousePosition;
        RaycastHit2D[] hits;

        _view.ViewNextObject.Show();

        while (_isGameStart) {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                GameManager.IsGamePause = true;
                GameManager.OnBindGameOver?.Invoke();
            }
#endif
            if (Input.GetMouseButtonDown(0) && !GameManager.IsGamePause) {
                isButtonClick = false;
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hits = Physics2D.RaycastAll(clickPosition, Vector2.zero);

                foreach (var hit in hits)
                {
                    ButtonExpansion button = hit.transform.GetComponent<ButtonExpansion>();
                    if (button != null)
                    {
                        isButtonClick = true;
                        break;
                    }
                }

                if (ViewCanvas.Get<ViewCanvasToast>().IsActiveSelf)
                    isButtonClick = true;

                if (ViewCanvas.Get<ViewCanvasObjectBook>().IsActiveSelf)
                    isButtonClick = true;

                while (!isButtonClick) {
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.y = currBall.transform.position.y;
                    currBall.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
                    _isHoldingBall = true;

                    if (Input.GetMouseButtonUp(0)) {
                        if (_isHoldingBall) {
                            DataScore.EarnCurrScore((int)(currBall.mergeLevel + 1) * 2);

                            currBall.Setting();
                            currBall = ObjectManager.init.GetObject(nextBallKey);
                            nextBallKey = ObjectManager.init.GetRandomKey();
                            _view.ViewNextObject.SetSprite(ObjectManager.init.ObjectSprites[(int)nextBallKey]);
                            
                            DataManager.init.gameData.currObjectKey = currBall.mergeLevel;
                            DataManager.init.gameData.nextObjectKey = nextBallKey;
                            await UniTask.Delay(300);

                            DataManager.init.Save();
                        }
                        _isHoldingBall = false;
                        break;
                    }
                    await UniTask.Yield();
                }
            }
            await UniTask.Yield();
        }
    }

    private async UniTaskVoid CoCurrSocreFlow() {
        int.TryParse(_view.TextCurrScore.text, out int curr);

        float elapsedTime = 0f;
        float duration = 2f;
        while (true) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            int currentValue = (int)Mathf.Lerp(curr, DataScore.CurrScore, t);
            if (currentValue >= DataScore.CurrScore)
            {
                elapsedTime = 0f;
                duration = 1f;
            }
            UpdateScore();
            await UniTask.Delay(10);
        }
    }
}