using UnityEngine;

public class ControllerToast {
    private readonly ViewCanvasToast _view;

    public ControllerToast(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasToast>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "UI";

        _view.SetActive(false);
        _view.SetLocalizeKey();

    }
}