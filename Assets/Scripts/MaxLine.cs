using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MaxLine : MonoBehaviour {
    public float LineHeight => transform.position.y;

    private static readonly int isWaringToHash = Animator.StringToHash("isWaring");
    private Transform _underGround;

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
            return this.transform.position.y - 5f;
        }
    }

    public float OverYPosition {
        get {
            return this.transform.position.y;
        }
    }

    private Animator animator;

    private void Start() {
        _underGround = GameObject.Find("UnderGround").GetComponent<Transform>();
        ObjectHeightAsync();
    }

    public void WaringLine() {
        animator.SetTrigger(isWaringToHash);
    }

    private void ObjectHeightAsync() {
        transform.position = new Vector2(0, Camera.main.ScreenToWorldPoint(Vector2.one * GameManager.MaxLineHeight).y);
        _underGround.position = new Vector2(0, Camera.main.ScreenToWorldPoint(Vector2.one * GameManager.GroundHeight).y);
    }
}
