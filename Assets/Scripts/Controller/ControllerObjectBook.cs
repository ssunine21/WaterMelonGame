using DGExcepsion;
using UnityEngine;

public class ControllerObjectBook
{
    private readonly ViewCanvasObjectBook _view;

    public ControllerObjectBook(Transform parent)
    {
        _view = ViewCanvas.Create<ViewCanvasObjectBook>(parent);
        _view.GetComponent<Canvas>().sortingLayerName = "UI";
        
        _view.SetActive(false);
        
        _view.CloseButton.onClick.AddListener(() =>
        {
            _view.CloseButton.enabled = false;
            _view.Wrapped.CloseToast(() => {
                _view.CloseButton.enabled = true;
                _view.SetActive(false);
            });
        });
    }
}