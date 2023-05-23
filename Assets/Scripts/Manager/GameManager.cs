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
	public static GameManager init;

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
}