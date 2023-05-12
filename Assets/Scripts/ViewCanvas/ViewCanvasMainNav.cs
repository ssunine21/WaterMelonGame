using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewCanvasMainNav : ViewCanvas {

    public Button[] ButtonMenus => _menus;

    [SerializeField] private Button[] _menus;

    [Header("Localize")]
    [SerializeField] private TMP_Text _textMain;
    [SerializeField] private TMP_Text _textStore;

    public ViewCanvasMainNav SetLocalizeText() {
        _textMain.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Main);
        _textStore.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Store);
        return this;
    }
}