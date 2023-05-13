using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerToast {
    private readonly ViewCanvasToast _view;

    public ControllerToast(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasToast>(parent);
        _view.SetActive(false);
        _view.SetLocalizeKey();

    }
}