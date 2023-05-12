using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerCoin {
    public static UnityAction OnChangeValue;
    public static int Coin => DataManager.init.gameData.coin;

    public static void Earn(int value) {
        DataManager.init.gameData.coin += value;
        DataManager.init.Save();
        OnChangeValue?.Invoke();
    }

    public static bool Consume(int value) {
        if (DataManager.init.gameData.coin >= value) {
            DataManager.init.gameData.coin -= value;
            OnChangeValue?.Invoke();
            DataManager.init.Save();
            return true;
        }

        return false;
    }
}