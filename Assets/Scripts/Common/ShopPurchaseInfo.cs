using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPurchaseInfo : MonoBehaviour {

    public int Price => _price;

    [SerializeField] private int _price;
    [SerializeField] private ButtonExpansion _buttonPurchase;
    [SerializeField] private TMP_Text _textPrice;

    public ShopPurchaseInfo SetPurchaseButton(System.Action action) {
        _buttonPurchase.onClick.RemoveAllListeners();
        _buttonPurchase.onClick.AddListener(action.Invoke);

        return this;
    }

    public ShopPurchaseInfo SetPrice() {
        _textPrice.text = _price.ToString();
        return this;
    }
}
