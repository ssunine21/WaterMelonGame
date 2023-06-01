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

	public static float Height;
	public static float Width;

	public static float GroundHeight => Height * 0.03f;
	public static float MaxLineHeight => Height * 0.25f * 3.1f;
	public static float MaxLineWaringHeight => Height * 0.25f * 2.1f;
	public static float ObjectHeight => Height * 0.25f * 3.7f;

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

		float width = Screen.width;
		float defaultValue = width / 9 * 16;
		if (Screen.height < defaultValue) {
			Height = Screen.height;
			Width = Height / 16 * 9;
		} else {
			Height = defaultValue;
			Width = width;
		}

		OnBindNewGame += GameStart;
		OnBindStartGame += GameStart;
		OnBindGoHome += GameEnd;
	}

	private void Start() {
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
		isPremium = true;
	}
}