using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

namespace DGExcepsion {
    public static class DGExpansion {
        public static void ShowToast(this RectTransform rect, Action callback = null) {
            rect.DOKill();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack).OnComplete(() => callback?.Invoke()); ;
        }

        public static void CloseToast(this RectTransform rect, Action callback = null) {
            rect.DOKill();
            rect.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack).OnComplete(() => callback?.Invoke());
        }

        public static void CommonRotate(this RectTransform rect, Vector3 endValue, float duration) {
            rect.DORotate(endValue, duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
}
