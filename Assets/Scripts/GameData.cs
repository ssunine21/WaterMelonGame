using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo {
	[System.Serializable]
	public class GameData {
		public int bestScore = 0;
		public int currScore = 0;
		public int coin = 0;
		public int adsCount = 0;
		public int rankupItemCount = 1;
		public int destroyItemCount = 1;
		public int rerollItemCount = 1;

		public int currDailyCoinCount;
		public long initTimeDailyRewardTicks;

		public string key;

		public bool[] styleProducts;
		public bool[] wallpaperProducts;
		public int styleNum;
		public int wallpaperNum;

		public bool isEffectVolum;
		public bool isBGMVolum;
		public bool isVibration;

		public bool isPremium = false;
		public bool isDoubleCoin = false;

		public string lastLanguageFileName;

		public DateTime initTimer;
		public List<GameObjectData> objectData;
		public ObjectManager.ObjectKey currObjectKey;
	}


	[System.Serializable]
	public class GameObjectData {
		public Vector2 position;
		public ObjectManager.ObjectKey mergeLevel;

		public GameObjectData(Vector2 pos, ObjectManager.ObjectKey key) {
			position = pos;
			mergeLevel = key;
		}
	}
}
