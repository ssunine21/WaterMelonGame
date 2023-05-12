using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static UnityAction OnBindNewGame;
	public static UnityAction OnBindStartGame;
	public static UnityAction OnBindGoHome;
	public static UnityAction OnBindGameOver;

	public static bool IsGameStart;
	public static bool IsGamePause;

	static readonly private string TITLE = "RANK";
	static readonly private string COIN = "coin";
	static readonly private string SCORE = "score";

	public DatabaseReference databaseReference;

	public string key = "";
	public static GameManager init = null;

	public GameObject premiumGround;

	public bool isEnterGame = false;

	private bool _isPremium;
	public bool isPremium {
        get { return _isPremium; }
		set {
			if (value) {
				_isPremium = true;
				BuyPremium();
			}
		}
	}

	public bool isDoubleCoin;

	private void Awake() {
		if (init == null) {
			init = this;
		} else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
		Application.targetFrameRate = 60;

		OnBindNewGame += GameStart;
		OnBindStartGame += GameStart;
		OnBindGoHome += GameEnd;
	}

    public void GameStart() {
		if (!isEnterGame) {
			isEnterGame = true;
			IsGameStart = true;
		}
	}

	public void GameEnd() {
		IsGameStart = false;
		IsGamePause = false;
    }

    public void BuyPremium() {
		AdsManager.init.DestroyBannerAd();
		premiumGround.SetActive(true);
	}

	public void SetFirebaseData(User user) {
		if (!key.Equals("")) {
			string json = JsonUtility.ToJson(user);
			databaseReference.Child(TITLE).Child(key).SetRawJsonValueAsync(json);
		}
	}

	public void CoinFirebaseSync(int num) {
		databaseReference.Child(TITLE).Child(key).Child(COIN).SetValueAsync(num);
	}

	public void ScoreFirebaseSync(int num) {
		databaseReference.Child(TITLE).Child(key).Child(SCORE).SetValueAsync(num);
	}
}