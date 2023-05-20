using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
	static readonly private string TITLE = "RANK";
	static readonly private string COIN = "coin";

	static readonly public string FileName = "/gameData.dat";

	public static DataManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
		}
		else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		dataPath = Application.persistentDataPath + FileName;
		Load();
	}

	private string dataPath;
	public DataInfo.GameData gameData = new DataInfo.GameData();

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
				if (file.Length <= 0) return;

				gameData = (DataInfo.GameData)binaryFormatter.Deserialize(file);
			}

		} catch (System.Exception e) {
			Debug.LogException(e);
		}
	}

	private void LoadFirebaseDate() {
		FirebaseDatabase.DefaultInstance.GetReference(TITLE)
			.OrderByKey()
			.EqualTo(gameData.key)
			.ChildAdded += HandleChildAddedUserData;
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
		GameManager.init.key = GameManager.init.databaseReference.Child(TITLE).Push().Key;
		User user = new User(SystemInfo.deviceModel, gameData.coin);
		string json = JsonUtility.ToJson(user);
		GameManager.init.databaseReference.Child(TITLE).Child(GameManager.init.key).SetRawJsonValueAsync(json);
	}

	public void SetFirebaseData() {
		User user = new User(
			SystemInfo.deviceModel,
			gameData.coin
			);
		GameManager.init.SetFirebaseData(user);
	}
}