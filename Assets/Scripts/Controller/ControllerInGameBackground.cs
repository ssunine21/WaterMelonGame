using UnityEngine;

public class ControllerInGameBackground {
    private readonly ViewCanvasInGameBackground _view;

    public ControllerInGameBackground(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasInGameBackground>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "Background";

        GameManager.OnBindNewGame += InitStartStart;
        GameManager.OnBindStartGame += InitStartStart;
        GameManager.OnBindGoHome += GameEnd;
    }
    
    private void InitStartStart() {
        UpdateView();
    }

    private void UpdateView() {
        _view.SetActive(true);
        UpdateWallpaper();
    }

    private void UpdateWallpaper() {
        int index = DataManager.init.gameData.wallpaperNum;
        _view.SetBackground(index);
    }

    private void GameEnd()
    {
        _view.SetActive(false);
    }
}