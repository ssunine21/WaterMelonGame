using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace DataInfo.Controller
{
    public class ControllerLoading
    {
        public static bool IsInit = false;
        private readonly ViewCanvasLoading _view;
        
        public ControllerLoading(Transform parent)
        {
            _view = ViewCanvas.Create<ViewCanvasLoading>(parent);
            _view.SetActive(true);

            ShowLoading().Forget();
        }

        private async UniTaskVoid ShowLoading()
        {
            await UniTask.Delay(500);

            foreach (var loadingImage in _view.LoadingSprites)
            {
                _view.SetLoadingImage(loadingImage);
                await UniTask.Delay(33);
            }
            _view.LoadingImage.enabled = false;
            
            await UniTask.Delay(500);

            IsInit = true;
            _view.CanvasGroup.DOFade(0, 0.3f)
                .OnComplete(() =>
                {         
                    _view.SetActive(false);
                });
        }
    }
}