using System;
using Cysharp.Threading.Tasks;
using DGExcepsion;
using UnityEngine;
using DG.Tweening;

public class ControllerAttendance
{
    private static int _navIndex = 1;
    private readonly ViewCanvasAttendance _view;

    public ControllerAttendance(Transform parent)
    {
        _view = ViewCanvas.Create<ViewCanvasAttendance>(parent);

        ControllerMainMenu.OnBindMenu += UpdateVisible;

        foreach (var closeButton in _view.CloseButtons)
        {
            closeButton.onClick.AddListener(() =>
            {
                closeButton.enabled = false;
                _view.Wrapped.CloseToast(() => {
                    closeButton.enabled = true;
                    _view.SetActive(false);
                });
            });
        }

        Init().Forget();
    }

    private async UniTaskVoid Init()
    {
        ControllerMainMenu.OnBindSetActiveMenu(_navIndex, false);
        await UniTask.WaitUntil(() => ServerTime.IsTimeReady && DataManager.init.IsReady);
        ControllerMainMenu.OnBindSetActiveMenu(_navIndex, true);
        _view.SetLocalized();
        for (int i = 0; i < DataManager.init.cloudData.cloudAttendanceDatas.Count; ++i)
        {
            int index = i;
            var data = DataManager.init.cloudData.cloudAttendanceDatas[i];
            _view.ViewAttendanceUnits[i]
                .SetDay(string.Format(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Day), data.day + 1))
                .SetRewardAmount(data.amount)
                .SetEarnedAttendance(DataManager.init.gameData.attendanceCount > i)
                .OnClickListeners(() => OnClickAttendance(index));
            
            _view.ViewAttendanceUnits[i].Enabled = DataManager.init.gameData.attendanceCount == index && IsTimeOver();
        }
    }

    private void UpdateVisible(int index)
    {
        _view.SetActive(index == _navIndex);
        if (index == _navIndex) {
            _view.Wrapped.ShowToast();
            DataManager.init.gameData.isAttendanceFirstOpen = true;
            DataManager.init.Save();
        }
    }

    private void OnClickAttendance(int index)
    {
        var cloudData = DataManager.init.cloudData.cloudAttendanceDatas.Find(x => x.index == index);
        if (cloudData == null) return;
        
        PlayerCoin.Earn(cloudData.amount);
        DataManager.init.gameData.attendanceCount += 1;
        DataManager.init.gameData.lastTimeEarnAttendanceReward = ServerTime.ToStringNextDay();
        DataManager.init.Save();
        EarnIconAnimation(index);
        Init();
    }

    private bool IsTimeOver()
    {
        if (DateTime.TryParse(DataManager.init.gameData.lastTimeEarnAttendanceReward, out var lastTime))
        {
            return lastTime.Ticks - ServerTime.NowTime.Ticks <= 0;
        }
        return true;
    }

    private void EarnIconAnimation(int index)
    {
        index = Mathf.Clamp(index, 0,_view.ViewAttendanceUnits.Length - 1);
        var image = _view.ViewAttendanceUnits[index].EarnIcon;

        if (image == null) return;

        Color currColor = image.color;
        Color nextColor = image.color;
        currColor.a = 0;
        image.color = currColor;
        image.transform.localScale = Vector3.one * 5;
        image.DOColor(nextColor, 0.3f);
        image.transform.DOScale(Vector3.one, 0.3f);

        _view.ViewAttendanceUnits[index].SetEarnedAttendance(true);
    }
}
