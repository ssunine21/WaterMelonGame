using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {
	static readonly private string TITLE = "RANK";
	static readonly private string COIN = "coin";

	public static DataManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
		}
		else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		dataPath = Application.persistentDataPath + "/gameData.dat";
		Load();
	}

	private string dataPath;
	public DataInfo.GameData gameData = new DataInfo.GameData();

	public void Save() {
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream file = File.Create(dataPath);


		/*gameData.isPremium = GameManager.init.isPremium;
		gameData.isDoubleCoin = GameManager.init.isDoubleCoin;

		gameData.isBGMVolum = CameraControl.init.audioSource.mute;
		gameData.isEffectVolum = UIManager.init.audioSource.mute;

		gameData.initTimer = UIManager.init.initTime;

		gameData.lastLanguageFileName = LocalizationManager.init.lastLanguageFileName;
		gameData.key = GameManager.init.key;*/

		binaryFormatter.Serialize(file, gameData);

		file.Close();

		//SetFirebaseData();
	}

	public void Load() {

		if (File.Exists(dataPath)) {

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream file = File.OpenRead(dataPath);

			if (file.Length <= 0) return;
			gameData = (DataInfo.GameData)binaryFormatter.Deserialize(file);

			//ShoppingManager.init.ApplyItem(ShoppingManager.init.style[gameData.styleNum]);
			//ShoppingManager.init.ApplyItem(ShoppingManager.init.wallpaper[gameData.wallpaperNum]);

			/*GameManager.init.isPremium = gameData.isPremium;
			GameManager.init.isDoubleCoin = gameData.isDoubleCoin;*/

			/*SettingManager.init.BGMOn(gameData.isBGMVolum);
			SettingManager.init.EffectOn(gameData.isEffectVolum);

			UIManager.init.initTime = gameData.initTimer;
			LocalizationManager.init.lastLanguageFileName = gameData.lastLanguageFileName ?? "";*/

			/*if (gameData.key.Equals("")) {
				InitFirebaseData();
			}
			else {
				GameManager.init.key = gameData.key;
				LoadFirebaseDate();
			}*/

			file.Close();

		}
		else {
			//InitFirebaseData();
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