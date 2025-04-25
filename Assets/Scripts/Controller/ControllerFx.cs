using DataInfo.Manager;
using UnityEngine;

namespace DataInfo.Controller
{
    public class ControllerFx
    {
        private readonly ViewCanvasFx _view;

        public ControllerFx(Transform prent)
        {
            _view = ViewCanvas.Create<ViewCanvasFx>(prent);
            _view.SetActive(true);
        }
    }
}