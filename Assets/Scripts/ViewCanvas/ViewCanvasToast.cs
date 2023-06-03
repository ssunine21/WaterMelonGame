using System;
using TMPro;
using UnityEngine;
using DGExcepsion;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

public class ViewCanvasToast : ViewCanvas {
    [SerializeField] private GameObject _toast;
    [SerializeField] private GameObject _oneTimeToast;

    [SerializeField] private TMP_Text _textDesc;
    [SerializeField] private TMP_Text _textCheck;
    [SerializeField] private TMP_Text _textCancel;
    [SerializeField] private TMP_Text _textOneTimeMessage;

    [SerializeField] private ButtonExpansion _check;
    [SerializeField] private ButtonExpansion _cancel;
    [SerializeField] private ButtonExpansion _panel;

    private Vector3 _oneTimeMessagePosition = Vector3.zero;
    private Color _oneTimeMessageColor = Color.white;
    private Coroutine _coOneTimeMessage;

    public ViewCanvasToast SetDesc(string text) {
        _textDesc.text = text;
        return this;
    }

    public void SetLocalizeKey() {
        _textCheck.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Check);
        _textCancel.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Cancel);
    }

    public void Show(string message, Action OnCheckCallback = null, Action OnCancelCallback = null, bool isCancel = true) {
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

    public void ShowOneTimeMessage(string message) {
        if (_coOneTimeMessage != null)
            StopCoroutine(_coOneTimeMessage);

        _textOneTimeMessage.text = message;
        _coOneTimeMessage = StartCoroutine(CoOneTimeMessage());
    }

    private IEnumerator CoOneTimeMessage() {
        float time = 0.5f;

        _oneTimeToast.SetActive(true);

        if (_oneTimeMessagePosition == Vector3.zero) {
            _oneTimeMessagePosition = _textOneTimeMessage.transform.position;
            _oneTimeMessageColor = _textOneTimeMessage.color;
        }

        _textOneTimeMessage.transform.position = _oneTimeMessagePosition;
        _textOneTimeMessage.color = Color.clear;
        var rect = _textOneTimeMessage.GetComponent<RectTransform>();

        rect.DOKill();
        _textOneTimeMessage.DOKill();

        rect.DOMoveY(rect.position.y + 1, time).SetEase(Ease.OutCubic);
        _textOneTimeMessage.DOColor(_oneTimeMessageColor, time);

        yield return new WaitForSeconds(1f);

        rect.DOMoveY(rect.position.y + 0.5f, 0.5f).SetEase(Ease.OutCubic);
        _textOneTimeMessage.DOColor(Color.clear, 0.5f);
    }

    private void Close(ButtonExpansion button) {
        button.enabled = false;
        _wrapped.CloseToast(() => {
            _toast.SetActive(false);
            SetActive(false);
            button.enabled = true;
        });
    }
}
