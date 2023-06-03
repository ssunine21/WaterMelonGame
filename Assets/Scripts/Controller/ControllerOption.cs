using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DGExcepsion;

public class ControllerOption {
    private static int _navIndex = 0;

    private readonly ViewCanvasOption _view;

    public ControllerOption(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasOption>(parent);

        foreach(var closeButton in _view.Close) {
            closeButton.onClick.AddListener(() => {
                closeButton.enabled = false;
                _view.Wrapped.CloseToast(() => {
                    closeButton.enabled = true;
                    _view.SetActive(false);
                });
            });
        }

        _view.Music.callback += () => {

            DataManager.init.gameData.isBGMVolum = _view.Music.IsToggled();
            AudioManager.Init.SetOption();
            DataManager.init.Save();
        };
        _view.Effect.callback += () => {

            DataManager.init.gameData.isEffectVolum = _view.Effect.IsToggled();
            AudioManager.Init.SetOption();
            DataManager.init.Save();
        };
        _view.Vibration.callback += () => {

            DataManager.init.gameData.isVibration = _view.Vibration.IsToggled();
            DataManager.init.Save();
        };

        _view.Music.Toggle(DataManager.init.gameData.isBGMVolum);
        _view.Effect.Toggle(DataManager.init.gameData.isEffectVolum);
        _view.Vibration.Toggle(DataManager.init.gameData.isVibration);

        ControllerMainMenu.OnBindMenu += UpdateVisible;
    }

    public void UpdateVisible(int index) {
        _view.SetActive(index == _navIndex);
        if (index == _navIndex) {
            _view.Wrapped.ShowToast();
        }
    }

}