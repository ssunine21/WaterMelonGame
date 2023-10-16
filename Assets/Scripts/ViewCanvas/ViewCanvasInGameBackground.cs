using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ViewCanvasInGameBackground : ViewCanvas {

    [SerializeField] private GameObject[] _background;
    public ViewCanvasInGameBackground SetBackground(int index) {
        for(int i = 0; i < _background.Length; i++) {
            _background[i].SetActive(i == index);
        }
        return this;
    }
}