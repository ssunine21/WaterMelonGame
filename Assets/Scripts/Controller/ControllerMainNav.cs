using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class ControllerMainNav {
    public static UnityAction<int> OnSelectMenu;

    private readonly ViewCanvasMainNav _view;
    private readonly float _lockDuration = 0.3f;

    public ControllerMainNav(Transform parent) {
        _view = ViewCanvas.Create<ViewCanvasMainNav>(parent);

        _view.SetActive(true);
        _view.SetLocalizeText();

        int i = 0;
        foreach(var buttonMenu in _view.ButtonMenus) {
            int index = i;
            buttonMenu.onClick.AddListener(() => {
                OnSelectMenu?.Invoke(index);
                UpdateButtonView(index);
            });
            i++;
        }
    }

    private void UpdateButtonView(int index) {
        for(int i = 0; i < _view.ButtonMenus.Length; ++i) {
            _view.ButtonMenus[i].GetComponentInChildren<Image>().DOColor(index == i ? Definition.OriginColor : Definition.GrayColor, _lockDuration);
            _view.ButtonMenus[i].GetComponentInChildren<TMP_Text>().DOColor(index == i ? Definition.OriginColor : Definition.GrayColor, _lockDuration);
        }
    }
}