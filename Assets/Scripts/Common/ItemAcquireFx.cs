using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemAcquireFx : MonoBehaviour
{
    public  Image GoodImage => _image;
    
    [SerializeField] private Image _image;
    
    public void Explosion(Vector2 from, Vector2 to, float range, UnityAction callback = null)
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * range, 1.0f).SetEase(Ease.OutCubic));
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { callback?.Invoke();});
    }
}