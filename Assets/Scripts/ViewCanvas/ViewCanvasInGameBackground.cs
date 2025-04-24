using UnityEngine;

public class ViewCanvasInGameBackground : ViewCanvas {

    [SerializeField] private GameObject[] _background;
    public ViewCanvasInGameBackground SetBackground(int index) {
        for(int i = 0; i < _background.Length; i++) {
            _background[i].SetActive(i == index);
        }
        return this;
    }
}