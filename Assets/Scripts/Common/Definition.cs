using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Definition {

	public enum LocalizeKey {
		None = -1,
		Title,
		StartGame,
		NewGame,
		Main,
		Store,
		IsPurchase,
		Check,
		Cancel,
		CoinDummy,
		CoinPurse,
		CoinBox,
		Bonus,
		Object,
		Background,
		Special,
		Destruction,
		RankUp,
		Reroll,
		FreeCoin,
		DoubleCoin,
		RemoveAds,
		DestructionDesc,
		RankUpDesc,
		RerollDesc,
		FreeCoinDesc,
		DoubleCoinDesc,
		RemoveAdsDesc,
	}


	[System.Serializable]
	public struct GameObjectData {
		public Vector2 position;
		public ObjectManager.ObjectKey mergeLevel;

		public GameObjectData(Vector2 pos, ObjectManager.ObjectKey key) {
			position = pos;
			mergeLevel = key;
        }
	}

	public enum Item {
		Destruction,
		RankUp,
		Reroll,
    }

	public static Color OriginColor = new Color(227f / 255f, 139f / 255f, 41f / 255f);
	public static Color GrayColor = new Color(174f / 255f, 174f / 255f, 174f / 255f);

	public static Color OriginColorToAlpha = new Color(227f / 255f, 139f / 255f, 41f / 255f, 0);
}
