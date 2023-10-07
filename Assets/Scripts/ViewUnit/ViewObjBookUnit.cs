using System;
using System.Collections;
using System.Collections.Generic;
using DGExcepsion;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ViewObjBookUnit : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTr;
    [SerializeField] private Image _icon;

    [SerializeField] private float _rotateSpeed = -130;

    private Coroutine _coRotate;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _rectTr.gameObject.SetActive(false);
    }

    public void SetSprite(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    public void Show()
    {
        _rectTr.gameObject.SetActive(true);
        _rectTr.ShowToast();
        if(_coRotate != null)
            StopCoroutine(_coRotate);
        _coRotate = StartCoroutine(IeRotate());
    }

    private IEnumerator IeRotate()
    {
        float zOffset = Random.Range(0f, 180f);
        _icon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zOffset));
        while (true)
        {
            _icon.transform.Rotate(new Vector3(0, 0, _rotateSpeed * Time.deltaTime));
            yield return null;
        }
    }
}