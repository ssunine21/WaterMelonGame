using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCanvas : MonoBehaviour
{
    private static Dictionary<string , ViewCanvas> _views = new Dictionary<string, ViewCanvas>();

    public RectTransform Wrapped => _wrapped;

    [SerializeField] protected RectTransform _wrapped;

    public static T Create<T> (Transform parent) where T : ViewCanvas {
        string typeName = typeof(T).Name;
        var view = Instantiate(Resources.Load<T>($"ViewCanvas/{typeName}"), parent);

        view.GetComponent<Canvas>().worldCamera = Camera.main;
        view.GetComponent<Canvas>().enabled = false;
        if (!_views.ContainsKey(typeName)) {
            _views.Add(view.name, view);
        }

        return view;
    }

    public static T Get<T> () where T : ViewCanvas {
        string name = $"{typeof(T).Name}(Clone)";
        return _views[name] as T;
    }

    public void SetActive(bool flag) {
        GetComponent<Canvas>().enabled = flag;
    }
}
