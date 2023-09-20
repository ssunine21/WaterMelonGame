using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class DataScore {
	public static UnityAction OnBindChangeCurrScore;
	public static UnityAction OnBindChangeBestScore;
	public static UnityAction OnBindChangeLevel;
	
	public static int CurrScore => DataManager.init.gameData.currScore;
	public static int BestScore => DataManager.init.gameData.bestScore;
	public static int Exp => DataManager.init.gameData.exp;

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

	public static void EarnExp(int exp)
	{
		DataManager.init.gameData.exp += exp;
		GooglePlayGamesManager.UpdateLeaderboard();
		OnBindChangeLevel?.Invoke();
	}

	public static void ModularExp(out int level, out int currExp, out int maxExp)
	{
		int exp = DataManager.init.gameData.exp;
		int needExp;
		int currLevel = 0;
		while (true)
		{
			needExp = Mathf.FloorToInt(currLevel * 5) + 50;
			if (needExp > exp)
				break;
			else
			{
				exp -= needExp;
				currLevel++;
			}
		}
		level = currLevel;
		currExp = exp;
		maxExp = needExp;
	}

	public static int GetLevel()
	{
		ModularExp(out int level, out int exp, out int maxExp);
		return level + 1;
	}
}
