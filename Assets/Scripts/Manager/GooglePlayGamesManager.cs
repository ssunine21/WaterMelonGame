using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;

public class GooglePlayGamesManager : MonoBehaviour {

    public static bool IsLogin { get; set; }

    public void Start() {
#if UNITY_ANDROID
        PlayGamesPlatform.Activate();
#elif UNITY_IOS
#endif

        Login(success => IsLogin = success);
    }

    public static void Login(Action<bool> callback = null) {

        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success Login");
            callback?.Invoke(true);
        }
        else
        {
            Social.localUser.Authenticate(callback);
        }
    }

    public static void UpdateLeaderboard(Action<bool> callback = null) {

#if UNITY_ANDROID
        if (IsLogin) {
            int score = DataManager.init.gameData.bestScore;
            Social.ReportScore(score, "CgkIz5PRr4YLEAIQAQ", callback);
        }
        
#elif UNITY_IOS
        if (IsLogin)
        {
            int score = DataManager.init.gameData.bestScore;
            Social.ReportScore(score, "watermelongame.leaderboard.score", callback);
        }
#endif
    }
}