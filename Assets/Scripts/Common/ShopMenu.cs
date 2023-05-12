using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ShopMenu : MonoBehaviour
{
    private readonly float _lockDuration = 0.3f;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Image _underLine;

    public void Lock() {
        _title.DOColor(Definition.GrayColor, _lockDuration);
        _underLine.DOColor(Definition.OriginColorToAlpha, _lockDuration);
    }
    public void UnLock() {
        _title.DOColor(Definition.OriginColor, _lockDuration);
        _underLine.DOColor(Definition.OriginColor, _lockDuration);
    }
}
