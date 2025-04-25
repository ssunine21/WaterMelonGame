using System;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;

#elif UNITY_IOS
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;
#endif

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
            int level = DataScore.GetLevel();
            
            PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_best_score, callback);
            PlayGamesPlatform.Instance.ReportScore(level, GPGSIds.leaderboard_best_level, callback);
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