using System;
using TMPro;
using UnityEngine;
using DGExcepsion;
using System.Collections.Generic;

public class ViewCanvasToast : ViewCanvas {
    [SerializeField] private GameObject _toast;
    [SerializeField] private FadeMessage _oneTimeToast;
    [SerializeField] private GameObject _loadingPanel;

    [SerializeField] private TMP_Text _textDesc;
    [SerializeField] private TMP_Text _textCheck;
    [SerializeField] private TMP_Text _textCancel;

    [SerializeField] private ButtonExpansion _check;
    [SerializeField] private ButtonExpansion _cancel;
    [SerializeField] private ButtonExpansion _panel;

    private Vector3 _oneTimeMessagePosition = Vector3.zero;
    private Color _oneTimeMessageColor = Color.white;
    private Coroutine _coOneTimeMessage;

    private List<FadeMessage> _fadeMessages = new ();

    public ViewCanvasToast SetDesc(string text) {
        _textDesc.text = text;
        return this;
    }

    public void SetLocalizeKey() {
        _textCheck.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Check);
        _textCancel.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Cancel);
    }

    public void Loading(bool isOn)
    {
        _loadingPanel.SetActive(isOn);
    }

    public void ShowNoButton(string message, Action action)
    {
        _check.gameObject.SetActive(false);
        _cancel.gameObject.SetActive(false);
        _panel.enabled = false;
        
        SetDesc(message);
        action?.Invoke();
    }

    private bool _isPopupMessage;
    public void Show(string message, Action OnCheckCallback = null, Action OnCancelCallback = null, bool isCancel = true)
    {
        _isPopupMessage = true;
        SetActive(true);
        _toast.SetActive(true);
        _wrapped.ShowToast();

        _check.onClick.RemoveAllListeners();
        _cancel.onClick.RemoveAllListeners();
        _panel.onClick.RemoveAllListeners();

        SetDesc(message);
        _check.onClick.AddListener(() => OnCheckCallback?.Invoke());
        _cancel.onClick.AddListener(() => OnCancelCallback?.Invoke());
        _panel.onClick.AddListener(() => OnCancelCallback?.Invoke());

        _check.onClick.AddListener(() => Close(_check));
        _cancel.onClick.AddListener(() => Close(_cancel));
        _panel.onClick.AddListener(() => Close(_panel));

        _cancel.gameObject.SetActive(isCancel);
    }

    private bool _isShowingMessage;
    public void ShowOneTimeMessage(string message)
    {
        SetActive(true);
        _isShowingMessage = true;
        var fadeMessage = _fadeMessages.Find(x => !x.gameObject.activeSelf);
        if (fadeMessage == null)
        {
            fadeMessage = Instantiate(_oneTimeToast, this.transform);
            _fadeMessages.Add(fadeMessage);
        }

        fadeMessage
            .SetMessage(message)
            .Show(() =>
            {
                _isShowingMessage = false;
                if (!_isPopupMessage)
                    SetActive(false);
            });
    }
    
    private void Close(ButtonExpansion button) {
        button.enabled = false;
        _wrapped.CloseToast(() => {
            _toast.SetActive(false);
            if (!_isShowingMessage)
                SetActive(false);
            button.enabled = true;
            _isPopupMessage = false;
        });
    }
}
