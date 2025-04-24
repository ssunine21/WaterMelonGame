using UnityEngine;
using UnityEngine.UI;

public class ViewObjectBook : MonoBehaviour
{
    [SerializeField] private Image _objectImage;

    public ViewObjectBook SetObjectImage(Sprite sprite)
    {
        _objectImage.sprite = sprite;
        return this;
    }

    public ViewObjectBook SetActiveObject(bool flag)
    {
        _objectImage.color = flag ? Color.white : new Color(0.3f, 0.3f, 0.3f);
        return this;
    }
}