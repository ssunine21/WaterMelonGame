using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ViewCanvasLoading : ViewCanvas
{
    public CanvasGroup CanvasGroup => canvasGroup;
    public Sprite[] LoadingSprites => loadingSprites;
    public Image LoadingImage => loadingImage;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Sprite[] loadingSprites;
    [SerializeField] private Image loadingImage;
    
    public ViewCanvasLoading SetLoadingImage(Sprite sprite)
    {
        loadingImage.sprite = sprite;
        return this;
    }
}
