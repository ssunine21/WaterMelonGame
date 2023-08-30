using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class DataScore {
	public static UnityAction OnBindChangeCurrScore;
	public static UnityAction OnBindChangeBestScore;
	public static int CurrScore => DataManager.init.gameData.currScore;
	public static int BestScore => DataManager.init.gameData.bestScore;

	public static void SetCurrScore(int score) {
		DataManager.init.gameData.currScore = score;
	}

	public static void SetBestScore(int score) {
		if (score < DataManager.init.gameData.bestScore) return;

		DataManager.init.gameData.bestScore = score;
		GooglePlayGamesManager.UpdateLeaderboard();
		OnBindChangeBestScore?.Invoke();
	}

	public static void EarnCurrScore(int score) {
		DataManager.init.gameData.currScore += score;
		OnBindChangeCurrScore?.Invoke();
	}
}
