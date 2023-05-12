using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ViewCanvasOption : ViewCanvas {
    public ButtonExpansion[] Close => _buttonClose;
    public UltimateClean.Switch Music => _switchMusic;
    public UltimateClean.Switch Effect => _switchEffect;
    public UltimateClean.Switch Vibration => _switchVibration;

    [SerializeField] private ButtonExpansion[] _buttonClose;
    [SerializeField] private UltimateClean.Switch _switchMusic;
    [SerializeField] private UltimateClean.Switch _switchEffect;
    [SerializeField] private UltimateClean.Switch _switchVibration;
}
