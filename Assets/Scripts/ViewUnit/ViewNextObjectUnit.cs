using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;

public class ViewNextObjectUnit : MonoBehaviour
{
    [SerializeField] private Transform _textTr;
    [SerializeField] private float _rotateSpeed = -130;
    [SerializeField] private TMP_Text[] _texts;
    [SerializeField] private Image _icon;
    
    private Coroutine _coRotate;
    
    public void Show()
    {
        var localizedText = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.NextObject);

        int i = 0;
        foreach (var localizedTextChar in localizedText)
        {
            _texts[i].text = localizedTextChar.ToString();
            _texts[i].gameObject.SetActive(true);
            i++;
        }

        for (; i < _texts.Length; ++i)
        {
            _texts[i].gameObject.SetActive(false);
        }
        
        if(_coRotate != null)
            StopCoroutine(_coRotate);
        _coRotate = StartCoroutine(IeRotate());
    }

    public void SetSprite(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    private IEnumerator IeRotate()
    {
        float zOffset = Random.Range(0f, 180f);
        _textTr.rotation = Quaternion.Euler(new Vector3(0, 0, zOffset));
        while (true)
        {
            _textTr.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
            _icon.transform.Rotate(new Vector3(0, 0, ((-_rotateSpeed) + 20) * Time.deltaTime));
            yield return null;
        }
    }
}
