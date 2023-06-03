using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
	static readonly private string TITLE = "RANK";
	static readonly private string COIN = "coin";
	static readonly private string SCORE = "score";

	static readonly public string FileName = "/gameData.dat";

	public DatabaseReference databaseReference;

	public static DataManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
		}
		else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		//databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
		dataPath = Application.persistentDataPath + FileName;
		Load();
	}

    private void Start() {
		PlayerCoin.OnChangeValue += CoinFirebaseSync;
		GameManager.OnBindGoHome += Save;
    }

    private string dataPath;
	public DataInfo.GameData gameData;

	public void Save() {
		try {
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (var file = File.Open(dataPath, FileMode.OpenOrCreate)) {
				binaryFormatter.Serialize(file, gameData);
			}
		} catch(System.Exception e) {
			Debug.LogException(e);
		}

		//SetFirebaseData();
	}

	public void Load() {

		try {
			BinaryFormatter binaryFormatter = new BinaryFormatter();

			using (var file = File.Open(dataPath, FileMode.OpenOrCreate)) {
				if (file.Length <= 0)
				{
					gameData = new DataInfo.GameData();
					return;
				}

				gameData = (DataInfo.GameData)binaryFormatter.Deserialize(file);

				//if (gameData.key.Equals("")) {
				//	InitFirebaseData();
				//} else {
					LoadFirebaseDate();
				//}

			}

		} catch (System.Exception e) {
			Debug.LogException(e);
		}
	}

	private void LoadFirebaseDate() {
		//		FirebaseDatabase.DefaultInstance.GetReference(TITLE)
		//		.OrderByKey()
		//	.EqualTo(gameData.key)
		//.ChildAdded += HandleChildAddedUserData;
	}

	private void HandleChildAddedUserData(object sender, ChildChangedEventArgs arge) {
		if (arge.DatabaseError != null) {
			Debug.LogError(arge.DatabaseError.Message);
			return;
		};

		IDictionary data = (IDictionary)arge.Snapshot.Value;
		gameData.coin = int.Parse(data[COIN].ToString());
	}

	public void InitFirebaseData() {
		gameData.key = databaseReference.Child(TITLE).Push().Key;
		User user = new User(gameData.bestScore, gameData.coin);
		string json = JsonUtility.ToJson(user);
		//databaseReference.Child(TITLE).Child(gameData.key).SetRawJsonValueAsync(json);
	}

	public void CoinFirebaseSync() {
	//	databaseReference.Child(TITLE).Child(gameData.key).Child(COIN).SetValueAsync(gameData.coin);
	}

	public void ScoreFirebaseSync() {
		//databaseReference.Child(TITLE).Child(gameData.key).Child(SCORE).SetValueAsync(gameData.bestScore);
	}
}