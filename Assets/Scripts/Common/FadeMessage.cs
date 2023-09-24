using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FadeMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;

        public FadeMessage SetMessage(string message)
        {
            _title.text = message;
            return this;
        }

        public FadeMessage Show(UnityAction callback)
        {
            gameObject.SetActive(true);
            StartCoroutine(IeMessage(callback));
            return this;
        }

        private Vector3 _oneTimeMessagePosition = Vector3.zero;
        private IEnumerator IeMessage(UnityAction callback)
        {
            float time = 0.5f;
            
            if (_oneTimeMessagePosition == Vector3.zero) {
                _oneTimeMessagePosition = transform.position;
            }

            transform.position = _oneTimeMessagePosition;
            var rect = GetComponent<RectTransform>();

            rect.DOKill();
            this.DOKill();

            rect.DOMoveY(rect.position.y + 1, time).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(1f);

            rect.DOMoveY(rect.position.y + 0.5f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
            });
        }
    }
