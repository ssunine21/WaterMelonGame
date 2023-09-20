using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ViewCanvasOption : ViewCanvas {
    public ButtonExpansion[] Close => _buttonClose;
    public Switch Music => _switchMusic;
    public Switch Effect => _switchEffect;
    public Switch Vibration => _switchVibration;

    [SerializeField] private ButtonExpansion[] _buttonClose;
    [SerializeField] private Switch _switchMusic;
    [SerializeField] private Switch _switchEffect;
    [SerializeField] private Switch _switchVibration;

    [SerializeField] private TMP_Text _textBGM;
    [SerializeField] private TMP_Text _textEffect;
    [SerializeField] private TMP_Text _textVibration;

    public void SetLocalizeText() {
        _textBGM.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.BGM);
        _textEffect.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Effect);
        _textVibration.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Vibration);
    }
}
