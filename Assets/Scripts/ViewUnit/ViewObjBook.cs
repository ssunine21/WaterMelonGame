    using DGExcepsion;
    using UnityEngine;
    
    public class ViewObjBook : MonoBehaviour
    {
        [SerializeField] private RectTransform _wrap;
        [SerializeField] private Sprite[] _objSprites;
        [SerializeField] private Transform[] _objTransforms;

        public void Open()
        {
            gameObject.SetActive(true);
            _wrap.ShowToast();
        }

        public void Close()
        {
            _wrap.CloseToast(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private void ShowObject()
        {
            
        }
    }
