using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class ControllerMainMenu {
    public static UnityAction<int> OnBindMenu;
    private static int _navIndex = 0;

    private readonly ViewCanvasMainMenu _view;

    public ControllerMainMenu(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasMainMenu>(parent);

        _view.SetActive(true);
        _view.SetLocalizeText();
        _view.NewGmae.onClick.AddListener(() => GameManager.OnBindNewGame?.Invoke());
        _view.StartGame.onClick.AddListener(() => GameManager.OnBindStartGame?.Invoke());
        _view.Option.onClick.AddListener(() => OnBindMenu?.Invoke(0));
        _view.Leaderboard.onClick.AddListener(() => Social.ShowLeaderboardUI());

        ControllerMainNav.OnSelectMenu += SetVisible;
        GameManager.OnBindNewGame += () => _view.SetActive(false);
        GameManager.OnBindStartGame += () => _view.SetActive(false);
        GameManager.OnBindGoHome += () => _view.SetActive(true);
    }

    private void SetVisible(int index) {
        _view.SetActive(_navIndex == index);
    }
}