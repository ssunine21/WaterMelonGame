using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
#elif UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class ControllerMainMenu {
    public static UnityAction<int> OnBindMenu;
    public static UnityAction<int, bool> OnBindSetActiveMenu;
    private static int _navIndex = 0;

    private readonly ViewCanvasMainMenu _view;

    public ControllerMainMenu(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasMainMenu>(parent);

        _view.SetActive(true);
        _view.SetLocalizeText();
        _view.NewGmae.onClick.AddListener(NewGame);
        _view.StartGame.onClick.AddListener(() => GameManager.OnBindStartGame?.Invoke());
        _view.Option.onClick.AddListener(() => OnBindMenu?.Invoke(0));
        _view.Leaderboard.onClick.AddListener(LeaderBoard);
        _view.Attendance.onClick.AddListener(() => OnBindMenu?.Invoke(1));

        OnBindSetActiveMenu += SetActiveMenus;
        
        ControllerMainNav.OnSelectMenu += SetVisible;
        GameManager.OnBindNewGame += () => _view.SetActive(false);
        GameManager.OnBindStartGame += () => _view.SetActive(false);
        GameManager.OnBindGoHome += () => _view.SetActive(true);
        DataScore.OnBindChangeBestScore += UpdateBestScore;

        UpdateBestScore();
    }

    private void SetActiveMenus(int index, bool flag)
    {
        switch (index)
        {
            case 0:
                _view.Option.gameObject.SetActive(flag);
                break;
            case 1:
                _view.Attendance.gameObject.SetActive(flag);
                break;
        }
    }

    private void UpdateBestScore() {
        _view.TextBestScore.text = DataScore.BestScore.ToString();
    }

    private void LeaderBoard() {
        if (Social.localUser.authenticated == false) {
            Social.localUser.Authenticate((bool success) => {
                if (success) {
                    Social.ShowLeaderboardUI();
                    return;
                } else {
                    return;
                }
            });
        }
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI("watermelongame.leaderboard.score", TimeScope.AllTime);
#endif
    }

    private void NewGame() {
        if (DataManager.init.gameData.objectData != null
            || DataManager.init.gameData.objectData.Count >= 0) {
            ViewCanvas.Get<ViewCanvasToast>().Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.WaringNewStart),
                GameManager.OnBindNewGame.Invoke);
        } else
            GameManager.OnBindNewGame?.Invoke();
    }

    private void SetVisible(int index) {
        _view.SetActive(_navIndex == index);
    }
}