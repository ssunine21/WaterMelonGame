using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public static class DataScore {
	public static UnityAction OnBindChangeCurrScore;
	public static int CurrScore => DataManager.init.gameData.currScore;
	public static int BestScore => DataManager.init.gameData.bestScore;

	public static void SetCurrScore(int score) {
		DataManager.init.gameData.currScore = score;
	}

	public static void SetBestScore(int score) {
		if (score < CurrScore) return;
		DataManager.init.gameData.bestScore = score;
    }

	public static void EarnBestScore(int score) {
		DataManager.init.gameData.bestScore += score;
    }

	public static void EarnCurrScore(int score) {
		DataManager.init.gameData.currScore += score;
		OnBindChangeCurrScore?.Invoke();
	}
	public static void ConsumeDestroyItem(int count) {
		DataManager.init.gameData.destroyItemCount -= count;
	}

	public static void EarnDestroyItem(int count) {
		DataManager.init.gameData.destroyItemCount += count;
	}

	public static void ConsumeRankUpItem(int count) {
		DataManager.init.gameData.rankupItemCount -= count;
	}

	public static void EarnRankUpItem(int count) {
		DataManager.init.gameData.rankupItemCount += count;
	}
}
