using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBase : MonoBehaviour {
    private static Dictionary<string, ViewBase> _views = new Dictionary<string, ViewBase>();
    public static T Create<T>(Transform parent) where T : ViewBase {
        string typeName = typeof(T).Name;
        var view = Instantiate(Resources.Load<T>($"ViewBase/{typeName}"), parent);

        if (!_views.ContainsKey(typeName)) {
            _views.Add(typeName, view);
        }
        return view;
    }

    public static T Get<T>(T type) where T : ViewBase {
        return _views[type.name] as T;
    }

    public void SetActive(bool flag) {
        gameObject.SetActive(flag);
    }
}
