using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ViewShopItem : ViewBase {
    public ShopPurchaseInfo PurchaseInfo => _purchaseInfo;

    [SerializeField] private Image[] _objectImages;
    [SerializeField] private ShopPurchaseInfo _purchaseInfo;
    [SerializeField] private GameObject _panelLock;

    public void SetObjectImage(int index) {
        Sprite[] sprites = Resources.LoadAll<Sprite>("obj/objects" + index);

        for(int i = 0; i < _objectImages.Length; ++i) {
            int num = int.Parse(Regex.Replace(_objectImages[i].name, @"\D", ""));
            _objectImages[i].sprite = Resources.Load<Sprite>($"obj/Obj{index}_{num}");
        }
    }

    public void StartAnimation() {
        foreach(var obj in _objectImages) {
            obj.DOKill();
            obj.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, Random.Range(0, 2) % 2 == 0 ? 360 : -360), 5f, RotateMode.FastBeyond360)
                     .SetEase(Ease.Linear)
                     .SetLoops(-1);
        }
    }

    public void StopAnimation() {
        foreach (var obj in _objectImages) {
            obj.GetComponent<RectTransform>().DOKill();
        }
    }

    public void SetLock(bool flag) {
        _panelLock.SetActive(flag);
    }
}