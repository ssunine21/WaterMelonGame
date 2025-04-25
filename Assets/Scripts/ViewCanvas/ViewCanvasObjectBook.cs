    using System.Collections;
    using DG.Tweening;
    using DGExcepsion;
    using UnityEngine;
    using UnityEngine.UI;
    
    public class ViewCanvasObjectBook : ViewCanvas
    {
        public ButtonExpansion CloseButton => _close;
        
        public int ShowSpeed = 1000;
        [SerializeField] private ViewObjBookUnit[] _viewObjBookUnits;
        [SerializeField] private Transform _arrowTr;
        [SerializeField] private Image _aroundFillAmount;
        [SerializeField] private ButtonExpansion _close;
        public void Open()
        {
            SetActive(true);
            _wrapped.ShowToast();
            StartCoroutine(ShowObject());
        }

        public void Close()
        {
            _wrapped.CloseToast(() =>
            {
                SetActive(false);
            });
        }

        private WaitForSeconds _wfs;
        private IEnumerator ShowObject()
        {
            float speedForSec = ShowSpeed / 1000f;
            float speedForPiece = speedForSec / 15f;
            _wfs = new WaitForSeconds(speedForPiece);
            AnimationInit();
            _aroundFillAmount.DOFillAmount(1, speedForSec).SetEase(Ease.InOutQuart);
            _arrowTr.DORotate(new Vector3(0, 0, -330f), speedForSec, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart);
            
            for (int i = 0; i < _viewObjBookUnits.Length; ++i)
            {
                var bookUnit = _viewObjBookUnits[i];
                bookUnit.Init();
                bookUnit.SetSprite(ObjectManager.init.ObjectSprites[i]);
                yield return _wfs;
                bookUnit.Show();
            }
        }

        private void AnimationInit()
        {
            foreach (var bookUnit in _viewObjBookUnits)
            {
                bookUnit.Init();
            }

            _aroundFillAmount.fillAmount = 0;
            _arrowTr.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
