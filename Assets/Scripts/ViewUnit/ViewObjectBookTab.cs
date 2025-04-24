using UnityEngine;
using UnityEngine.UI;

public class ViewObjectBookTab : MonoBehaviour
{
    public Button Button => _button;
    public ViewReddot ViewReddot => _viewReddot;
    
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private ViewReddot _viewReddot;

    public ViewObjectBookTab SetActiveView(bool flag)
    {
        _background.enabled = flag;
        return this;
    }
}