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
		public int viewAdsCount = 0;
		public int lastJoin = 0;

		public int currDailyCoinCount;
		public long initTimeDailyRewardTicks;

		public bool watchAdsDestroyItem;
		public bool watchAdsRankupItem;
		public bool watchAdsRerollItem;

		public string key;

		public bool[] styleProducts;
		public bool[] wallpaperProducts;
		public int styleNum;
		public int wallpaperNum;

		public bool isEffectVolum = true;
		public bool isBGMVolum = true;
		public bool isVibration = true;

		public bool isPremium = false;
		public bool isDoubleCoin = false;

		public List<GameObjectData> objectData;
		public ObjectManager.ObjectKey currObjectKey;
	}


	[System.Serializable]
	public class GameObjectData {
		public float xPos;
		public float yPos;
		public ObjectManager.ObjectKey mergeLevel;

		public GameObjectData(Vector2 pos, ObjectManager.ObjectKey key) {
			xPos = pos.x;
			yPos = pos.y;
			mergeLevel = key;
		}
	}
}
