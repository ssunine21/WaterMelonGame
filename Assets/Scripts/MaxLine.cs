using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaxLine : MonoBehaviour {
    public float LineHeight => transform.position.y;

    private static readonly float DELAY_GAMEOVER = 1.5f;
    private static readonly int isWaringToHash = Animator.StringToHash("isWaring");
    private bool isWaring = false;

    public static MaxLine init;
    private void Awake() {
		if (init == null) {
			init = this;
		} else if (init != this) {
			Destroy(this.gameObject);
		}

        animator = GetComponent<Animator>();
    }

    public float WaringYPosition {
        get {
            return this.transform.position.y - 100f;
        }
    }

    public float OverYPosition {
        get {
            return this.transform.position.y;
        }
    }

    private Animator animator;

    private void Start() {
        ObjectHeightAsync();
    }

    public void WaringLine(bool flag) {
        animator.SetBool(isWaringToHash, flag);
    }

    public void StopFlickerAnim() {
        animator.SetBool(isWaringToHash, false);
    }

    private void ObjectHeightAsync() {
        float hight = GameManager.Height / 4 * 3.1f;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Vector2.one * hight);
        pos.x = 0;
        this.transform.position = pos;
    }
}
