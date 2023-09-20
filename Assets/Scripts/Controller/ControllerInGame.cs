using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class ControllerInGame {
    private readonly ViewCanvasInGame _view;

    private bool _isHoldingBall = false;
    private bool _isGameStart = false;   

    public ControllerInGame(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasInGame>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "Background";

        _view.ButtonBack.onClick.AddListener(() => {
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
                    AdsManager.init.ShowAdDestroyItem(() =>
                    {
                        DataManager.init.gameData.watchAdsDestroyItem = true;
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
                    AdsManager.init.ShowAdRankUpItem(() =>
                    {
                        DataManager.init.gameData.watchAdsRankupItem = true;
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
                    AdsManager.init.ShowAdRerollItem(() =>
                    {
                        DataManager.init.gameData.watchAdsRerollItem = true;
                        UpdateItemCount();
                    });
                }
            });
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
        DataManager.init.gameData.objectData = new List<DataInfo.GameObjectData>();
    }

    private void UpdateView() {
        _view.SetActive(true);

        UpdateWallpaper();
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

        _view.TextDestoryItemCount.text = destroyCount == 0 && !DataManager.init.gameData.watchAdsDestroyItem ? "" : destroyCount.ToString();
        _view.TextRankUpItemCount.text = rankupCount == 0 && !DataManager.init.gameData.watchAdsRankupItem ? "" : rankupCount.ToString();
        _view.TextRerollItemCount.text = rerollCount == 0 && !DataManager.init.gameData.watchAdsRerollItem ? "" : rerollCount.ToString();

        _view.DestoryItemAdsPanel.SetActive(!DataManager.init.gameData.watchAdsDestroyItem && destroyCount == 0);
        _view.RankUpItemAdsPanel.SetActive(!DataManager.init.gameData.watchAdsRankupItem && rankupCount == 0);
        _view.RerollItemAdsPanel.SetActive(!DataManager.init.gameData.watchAdsRerollItem && rerollCount == 0);

        DataManager.init.Save();
    }

    private void UpdateWallpaper() {
        int index = DataManager.init.gameData.wallpaperNum;
        _view.SetBackground(index);
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

        bool isButtonClick;
        Vector3 mousePosition;
        RaycastHit2D[] hits;

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

                while (!isButtonClick) {
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.y = currBall.transform.position.y;
                    currBall.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
                    _isHoldingBall = true;

                    if (Input.GetMouseButtonUp(0)) {
                        if (_isHoldingBall) {
                            DataScore.EarnCurrScore((int)(currBall.mergeLevel + 1) * 2);

                            currBall.Setting();
                            currBall = ObjectManager.init.GetRandomObject();
                            DataManager.init.gameData.currObjectKey = currBall.mergeLevel;
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