using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ViewProducts : MonoBehaviour {
    public ButtonExpansion Button => _buttonPurchase;

    [SerializeField] private TMP_Text _textTitle;
    [SerializeField] private TMP_Text _textDesc;
    [SerializeField] private TMP_Text _textAmount;
    [SerializeField] private TMP_Text _textPrice;

    [SerializeField] private ButtonExpansion _buttonPurchase;

    public ViewProducts SetTitle(string text) {
        _textTitle.text = text;
        return this;
    }
    public ViewProducts SetDescription(string text) {
        _textDesc.text = text;
        return this;
    }
    public ViewProducts SetAmount(string text) {
        _textAmount.text = text;
        return this;
    }
    public ViewProducts SetPrice(string text) {
        _textPrice.text = text;
        return this;
    }
    public ViewProducts SetButtonAction(UnityAction action) {
        _buttonPurchase.onClick.AddListener(action);
        return this;
    }

    public void SetVisible(bool flag)
    {
        gameObject.SetActive(flag);
    }
}