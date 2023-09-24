using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using UnityEngine.Events;
using FileMode = System.IO.FileMode;

public class DataManager : MonoBehaviour {
	private const string Title = "RANK";
	private const string Coin = "coin";
	private const string LastJoin = "lastjoin";
	private const string BestScore = "bestscore";
	private const string Exp = "exp";
	private const string IsRemoveAds = "isRemoveAds";

	public static readonly string FileName = "/gameData.dat";

	public DatabaseReference databaseReference;
	public bool IsReady = false;
	public static DataManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
		}
		else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		databaseReference = FirebaseDatabase.DefaultInstance.GetReference(Title);
		dataPath = Application.persistentDataPath + FileName;
		Load();
	}

    private void Start() {
		PlayerCoin.OnChangeValue += CoinFirebaseSync;
		DataScore.OnBindChangeBestScore += ScoreFirebaseSync;
		DataScore.OnBindChangeLevel += LevelFirebaseSync;
		GameManager.OnBindGoHome += Save;
    }

    private string dataPath;
	public DataInfo.GameData gameData;
	public CloudData cloudData;

	public void Save() {
		try {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (var file = File.Open(dataPath, FileMode.OpenOrCreate)) {
				binaryFormatter.Serialize(file, gameData);
			}
		} catch(System.Exception e) {
			Debug.LogException(e);
		}
	}

	public async void Load() {

		try {
			BinaryFormatter binaryFormatter = new BinaryFormatter();

			using (var file = File.Open(dataPath, FileMode.OpenOrCreate))
			{
				if (file.Length <= 0) {
					gameData = new DataInfo.GameData();
					InitFirebaseData();
				} else {
					gameData = (DataInfo.GameData)binaryFormatter.Deserialize(file);
					await LoadFirebaseDate();
				}

				LoadCloudData();
			}

		} catch (System.Exception e) {
			Debug.LogException(e);
		}
	}

	private async UniTask<bool> LoadFirebaseDate()
	{
		Debug.Log($"Load FirebaseKey : {gameData.key}");
		await databaseReference.Child(gameData.key)
			.GetValueAsync().ContinueWith(task => {
				if(task.IsCompleted) {
					DataSnapshot snap = task.Result;
					if (snap.Value == null)
					{
						InitFirebaseData(gameData.key);
						return;
					}
					gameData.coin = int.Parse(snap.Child(Coin).Value.ToString());
					gameData.exp = int.Parse(snap.Child(Exp).Value.ToString());

					if (snap.Child(BestScore).Value == null)
					{
						ScoreFirebaseSync();
					}
					else
					{
						gameData.bestScore = int.Parse(snap.Child(BestScore).Value.ToString());
					}

					if (snap.Child(IsRemoveAds).Value == null)
					{
						RemoveAdsFirebaseSync();
					}
					else
					{
						gameData.isPremium = bool.Parse(snap.Child(IsRemoveAds).Value.ToString());
					}
					LastTImeSync();
				}
			});
		return true;
	}

	public void InitFirebaseData(string authCode = "") {
		var timespan = new System.TimeSpan(System.DateTime.Now.Ticks);
		gameData.key = string.IsNullOrEmpty(authCode) ? databaseReference.Push().Key : authCode;
		gameData.lastJoin = timespan.Days;

		Debug.Log($"Init FirebaseKey : {gameData.key}");

		User user = new User(gameData.coin, gameData.lastJoin, gameData.exp, gameData.bestScore, gameData.isPremium);
		string json = JsonUtility.ToJson(user);
		databaseReference.Child(gameData.key).SetRawJsonValueAsync(json);
		
		Save();
	}

	public void CoinFirebaseSync() {
		databaseReference.Child(gameData.key).Child(Coin).SetValueAsync(gameData.coin);
	}
	public void ScoreFirebaseSync() {
		databaseReference.Child(gameData.key).Child(BestScore).SetValueAsync(gameData.bestScore);
	}
	public void LevelFirebaseSync() {
		databaseReference.Child(gameData.key).Child(Exp).SetValueAsync(gameData.exp);
	}

	public void RemoveAdsFirebaseSync()
	{
		databaseReference.Child(gameData.key).Child(IsRemoveAds).SetValueAsync(gameData.isPremium);
	}

	public void FirebaseLogin(UnityAction callback)
	{
		ViewCanvas.Get<ViewCanvasToast>().Loading(true);
		GooglePlayGamesManager.Login((success) =>
		{
			if (success)
			{
				TryFirebaseLogin(Social.localUser.id, callback);
			}
		});
	}

	private async void TryFirebaseLogin(string authCode, UnityAction callback)
	{
		var loadingView = ViewCanvas.Get<ViewCanvasToast>();
		await databaseReference.Child(authCode).GetValueAsync().ContinueWithOnMainThread( (task =>
		{
			if (task.IsCompleted)
			{
				gameData.isGameServiceLogin = true;
				DataSnapshot snapshot = task.Result;
				if (snapshot.Value == null)
				{
					InitFirebaseData(authCode);
					loadingView.ShowOneTimeMessage(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.SuccessLogin));
					callback?.Invoke();
				}
				else
				{
					string auth = authCode;
					var toast = ViewCanvas.Get<ViewCanvasToast>();
					toast.Show(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.LoginWarringMsg), async () =>
					{
						loadingView.Loading(true);
						gameData.key = auth;
						await LoadFirebaseDate();
						loadingView.ShowOneTimeMessage(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.SuccessLogin));
						loadingView.Loading(false);
						callback?.Invoke();
						
						Save();
					});
				}
			}
		}));
		
		ViewCanvas.Get<ViewCanvasToast>().Loading(false);
	}

	private IEnumerator IeShutDown()
	{
		yield return new WaitForSeconds(2);
		Application.Quit();
	}

	public void LastTImeSync() {
		var timespan = new System.TimeSpan(System.DateTime.Now.Ticks);
		gameData.lastJoin = timespan.Days;

		databaseReference.Child(gameData.key).Child(LastJoin).SetValueAsync(gameData.lastJoin);
	}

	public void LoadCloudData()
	{
		FirebaseDatabase.DefaultInstance.GetReference("CloudData")
			.GetValueAsync().ContinueWith(task =>
			{
				if (task.IsCompleted)
				{
					DataSnapshot dataSnapShot = task.Result;
					string json = dataSnapShot.GetRawJsonValue();
					cloudData = JsonUtility.FromJson<CloudData>(json);
					IsReady = true;
				}
			});
	}
}