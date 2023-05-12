using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerItem {
    public static UnityAction<Definition.Item> OnChangeItem;
    public static UnityAction OnChangeCurrDailyCoinCount;

    public static bool IsCanBuy(Definition.Item key) {
        if(PlayerCoin.Coin < GetCost(key)) {
            var toastMessage = ViewCanvas.Get<ViewCanvasToast>();
            toastMessage.ShowOneTimeMessage("코인이 부족합니다.");
            return false;
        }

        return true;
    }

    public static void Earn(Definition.Item key, int count = 1) {
        switch (key) {
            case Definition.Item.Destruction:
                DataManager.init.gameData.destroyItemCount += count;
                break;
            case Definition.Item.RankUp:
                DataManager.init.gameData.rankupItemCount += count;
                break;
            case Definition.Item.Reroll:
                DataManager.init.gameData.rerollItemCount += count;
                break;
            default:
                break;
        }
        DataManager.init.Save();
        OnChangeItem?.Invoke(key);
    }
    public static void Comsume(Definition.Item key) {
        switch (key) {
            case Definition.Item.Destruction:
                DataManager.init.gameData.destroyItemCount -= 1;
                break;
            case Definition.Item.RankUp:
                DataManager.init.gameData.rankupItemCount -= 1;
                break;
            case Definition.Item.Reroll:
                DataManager.init.gameData.rerollItemCount -= 1;
                break;
            default:
                break;
        }
        DataManager.init.Save();
        OnChangeItem?.Invoke(key);
    }

    public static int GetCost(Definition.Item key) {
        switch (key) {
            case Definition.Item.Destruction:
                return 1000;
            case Definition.Item.RankUp:
                return 1000;
            case Definition.Item.Reroll:
                return 1000;
            default:
                return 0;
        }
    }

    public static int GetCount(Definition.Item key) {
        switch (key) {
            case Definition.Item.Destruction:
                return DataManager.init.gameData.destroyItemCount;
            case Definition.Item.RankUp:
                return DataManager.init.gameData.rankupItemCount;
            case Definition.Item.Reroll:
                return DataManager.init.gameData.rerollItemCount;
        }
        return 0;
    }
}