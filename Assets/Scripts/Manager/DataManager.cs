using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Firebase.Database;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
	private const string Title = "RANK";
	private const string Coin = "coin";
	private const string LastJoin = "lastjoin";
	private const string BestScore = "bestscore";
	private const string Exp = "exp";

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

	public void Load() {

		try {
			BinaryFormatter binaryFormatter = new BinaryFormatter();

			using (var file = File.Open(dataPath, FileMode.OpenOrCreate))
			{
				if (file.Length <= 0) {
					gameData = new DataInfo.GameData();
					InitFirebaseData();
				} else {
					gameData = (DataInfo.GameData)binaryFormatter.Deserialize(file);
					LoadFirebaseDate();
				}

				LoadCloudData();
			}

		} catch (System.Exception e) {
			Debug.LogException(e);
		}
	}

	private void LoadFirebaseDate()
	{
		Debug.Log($"Load FirebaseKey : {gameData.key}");
		databaseReference.Child(gameData.key)
			.GetValueAsync().ContinueWith(task => {
				if(task.IsCompleted) {
					DataSnapshot snap = task.Result;
					gameData.coin = int.Parse(snap.Child(Coin).Value.ToString());
					gameData.exp = int.Parse(snap.Child(Exp).Value.ToString());
					LastTImeSync();
				}
			});

		if (gameData.lastJoin == 0) {
			databaseReference
				.Child(gameData.key)
				.Child("deviceModel").RemoveValueAsync();
			databaseReference
				.Child(gameData.key)
				.Child("flag").RemoveValueAsync();
			databaseReference
				.Child(gameData.key)
				.Child("score").RemoveValueAsync();
			databaseReference
				.Child(gameData.key)
				.Child("name").RemoveValueAsync();

			gameData.currScore = 0;
			gameData.bestScore = 0;
		}
	}

	public void InitFirebaseData() {
		var timespan = new System.TimeSpan(System.DateTime.Now.Ticks);
		gameData.key = databaseReference.Push().Key;
		gameData.lastJoin = timespan.Days;

		Debug.Log($"Init FirebaseKey : {gameData.key}");

		User user = new User(gameData.coin, gameData.lastJoin, gameData.exp);
		string json = JsonUtility.ToJson(user);
		databaseReference.Child(gameData.key).SetRawJsonValueAsync(json);
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

	public void LastTImeSync() {
		var timespan = new System.TimeSpan(System.DateTime.Now.Ticks);
		gameData.key = databaseReference.Push().Key;
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