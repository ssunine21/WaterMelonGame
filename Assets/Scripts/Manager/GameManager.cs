using Cysharp.Threading.Tasks;
using DataInfo.Controller;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {

	public static UnityAction OnBindNewGame;
	public static UnityAction OnBindStartGame;
	public static UnityAction OnBindGoHome;
	public static UnityAction OnBindGameOver;

	public static bool IsGameStart;
	public static bool IsGamePause;
	public static GameManager init;

	public static float Height => Screen.height;
	public static float Width => Screen.width;
	public static float MaxLineHeight => Height * 0.25f * 3.35f;
	public static float ObjectHeight => Height * 0.25f * 3.7f;

	public bool isEnterGame = false;

	[Header("Resources")] 
	public Sprite ExpIcon;

	public Sprite GoldIcon;

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

	private void Start()
	{
		PlayMusic().Forget();
	}

	private async UniTaskVoid PlayMusic()
	{
		await UniTask.WaitUntil(() => ControllerLoading.IsInit);
		AudioManager.Init.Play(Definition.AudioType.Background);
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
}