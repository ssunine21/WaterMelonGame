using System;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayGamesManager : MonoBehaviour {

    public static bool IsLogin { get; set; }

    public void Start() {
        PlayGamesPlatform.Activate();
        Login(success => IsLogin = success);
    }

    public static void Login(Action<bool> callback = null) {
        Social.localUser.Authenticate(callback);
    }

    public static void UpdateLeaderboard(Action<bool> callback = null) {
        if (IsLogin) {
            int score = DataManager.init.gameData.bestScore;
            Social.ReportScore(score, "CgkIz5PRr4YLEAIQAQ", callback);
        }
    }
}