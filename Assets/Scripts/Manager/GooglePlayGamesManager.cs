using System;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayGamesManager : MonoBehaviour {
    public void Start() {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(bool isLogin) {

    }

    public static void UpdateLeaderboard(int score, Action<bool> callback) {
        Social.ReportScore(score, "CgkIz5PRr4YLEAIQAQ", callback);
    }
}