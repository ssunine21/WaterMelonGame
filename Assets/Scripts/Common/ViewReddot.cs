using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewReddot : MonoBehaviour
{
    private static Dictionary<Definition.RedDotType, bool> _reddotDic;
    private static UnityAction _onChangedReddot;
    
    [SerializeField] private GameObject _reddot;
    [SerializeField] private List<Definition.RedDotType> _reddotTypes;

    private void Awake()
    {
        _reddotDic ??= new Dictionary<Definition.RedDotType, bool>();

        if (_reddotTypes == null) return;
        foreach (var type in _reddotTypes)
        {
            if(!_reddotDic.ContainsKey(type))
                _reddotDic.Add(type, true);
        }

        _onChangedReddot += ChangeReddot;
    }

    private void ChangeReddot()
    {
        foreach (var earnType in _reddotTypes)
        {
            if (!_reddotDic.ContainsKey(earnType)) continue;
            if (!_reddotDic[earnType]) continue;
            _reddot.SetActive(true);
            return;
        }
        
        _reddot.SetActive(false);
    }

    public void SetReddot(Definition.RedDotType reddotType, bool isOn)
    {
        if (_reddotTypes == null) return;
        foreach (var type in _reddotTypes)
        {
            if (type == reddotType)
            {
                if (_reddotDic.ContainsKey(type))
                {
                    _reddotDic[type] = isOn;
                    _onChangedReddot?.Invoke();
                }
            }
        }
    }

    public void SetReddot(bool isOn)
    {
        if (_reddotTypes == null) return;
        SetReddot(_reddotTypes[0], isOn);
    }
}