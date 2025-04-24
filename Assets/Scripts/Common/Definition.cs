using UnityEngine;

[System.Serializable]
public static class Definition {

	public enum LocalizeKey {
		None = -1,
		AppName,
		Title,
		StartGame,
		NewGame,
		Main,
		Store,
		IsPurchase,
		IsNotCoin,
		Check,
		Cancel,
		CoinPack,
		CoinBundle,
		CoinChest,
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
		WaringNewStart,
		BGM,
		Effect,
		Vibration,
		Attendance,
		AttendanceDesc,
		BaseTime,
		Day,
		WatchAdsToContinuePlay,
		GoogleLogin,
		LoginDesc,
		Copy,
		CopyDesc,
		LoginWarringMsg,
		SuccessLogin,
		NextObject,
		Book,
		BookDesc
	}

	public enum Item {
		Destruction,
		RankUp,
		Reroll,
    }

	public enum AudioType
    {
		Background,
		UIButton,
		Coin,
		Destroy
    }

	public enum RedDotType
	{
		Book0,
		Book1,
		Book2,
		Book3,
		Book4,
		Book5,
	}

	public static Color OriginColor = new Color(227f / 255f, 139f / 255f, 41f / 255f);
	public static Color GrayColor = new Color(174f / 255f, 174f / 255f, 174f / 255f);

	public static Color OriginColorToAlpha = new Color(227f / 255f, 139f / 255f, 41f / 255f, 0);

	public static int ObjectThemeCount = 6;
}
