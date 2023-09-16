using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEditor.UI;

public class ViewCanvasMainMenu : ViewCanvas {
    public ButtonExpansion Option => _buttonOption;
    public ButtonExpansion NewGmae => _buttonNewGame;
    public ButtonExpansion StartGame => _buttonStartGame;
    public ButtonExpansion Leaderboard => _buttonLeaderboard;
    public ButtonExpansion Attendance => _buttonAttendance;
    public TMP_Text TextBestScore => _textBestScore;


    [SerializeField] private ButtonExpansion _buttonNewGame;
    [SerializeField] private ButtonExpansion _buttonStartGame;
    [SerializeField] private ButtonExpansion _buttonOption;
    [SerializeField] private ButtonExpansion _buttonLeaderboard;
    [SerializeField] private ButtonExpansion _buttonAttendance;
    [SerializeField] private TMP_Text _textBestScore;

    [Header("Localize")]
    [SerializeField] private TMP_Text _textTitle;
    [SerializeField] private TMP_Text _textStartGame;
    [SerializeField] private TMP_Text _textNewGame;

    public ViewCanvasMainMenu SetLocalizeText() {
#if UNITY_ANDROID
        _textTitle.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Title);
#elif UNITY_IOS
        _textTitle.text = "Candy Pang";
#endif
        _textStartGame.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.StartGame);
        _textNewGame.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.NewGame);
        return this;
    }
}