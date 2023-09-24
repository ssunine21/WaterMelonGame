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
    public ButtonExpansion GoogleLoginButton => _googleLoginButton;
    public ButtonExpansion CopyButton => _copyButton;
    public Image CheckImage => _checkImage;

    [SerializeField] private ButtonExpansion[] _buttonClose;
    [SerializeField] private Switch _switchMusic;
    [SerializeField] private Switch _switchEffect;
    [SerializeField] private Switch _switchVibration;
    [SerializeField] private ButtonExpansion _googleLoginButton;
    [SerializeField] private ButtonExpansion _copyButton;
    [SerializeField] private Image _checkImage;

    [SerializeField] private TMP_Text _textBGM;
    [SerializeField] private TMP_Text _textEffect;
    [SerializeField] private TMP_Text _textVibration;
    [SerializeField] private TMP_Text _googleLogin;
    [SerializeField] private TMP_Text _loginDesc;
    [SerializeField] private TMP_Text _uuid;
    [SerializeField] private TMP_Text _copyText;

    public void SetLocalizeText() {
        _textBGM.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.BGM);
        _textEffect.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Effect);
        _textVibration.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Vibration);
        _googleLogin.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.GoogleLogin);
        _loginDesc.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.LoginDesc);
        _copyText.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Copy);

        _uuid.text = $"UUID : {DataManager.init.gameData.key}";
    }
}
