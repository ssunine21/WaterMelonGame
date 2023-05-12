using System.Collections;
using UnityEngine;
using DG.Tweening;


public class MainObject : MonoBehaviour {
	private static readonly float GRAVITY_SCALE = 3.2f;

	public ObjectManager.ObjectKey mergeLevel;

	private float _radius;
	public float radius {
		get { return _radius; }
		set {
			_radius = value;
        }
    }

	private Sprite sprite;
	private SpriteRenderer _spriteRenderer;
	private Rigidbody2D _rigidbody;
	private Vector3 screenPos;

	public bool isDropped => gameObject.GetComponent<Rigidbody2D>().gravityScale > 0;
	public bool isReady;
	private bool isMerging = false;
	private bool isOver = false;


	private void Start() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rigidbody = GetComponent<Rigidbody2D>();
		radius = GetRadius();
		screenPos.x = Camera.main.ViewportToWorldPoint(Vector2.zero).x;
		screenPos.y = Camera.main.ViewportToWorldPoint(Vector2.one).x;
		StartCoroutine(CoAsyncPosition());
	}

	private IEnumerator CoAsyncPosition() {
		float waringLineTime = 0;
		float overLineTime = 0;

		while (true) {
			if (transform.position.x - radius < ObjectManager.init.backgroundLeft) {
				Vector2 temp = new Vector2(ObjectManager.init.backgroundLeft + radius, transform.position.y);
				transform.position = temp;
				_rigidbody.angularVelocity = 0;
			} else if (transform.position.x + radius > ObjectManager.init.backgroundRight) {
				Vector2 temp = new Vector2(ObjectManager.init.backgroundRight - radius, transform.position.y);
				transform.position = temp;
				_rigidbody.angularVelocity = 0;
			}

			if (isDropped && isReady) {
				if (waringLineTime > 0.5f) {
					waringLineTime = 0;
					MaxLine.init.WaringLine(GetYPosition() > MaxLine.init.WaringYPosition);
				}

				if(GetYPosition() > MaxLine.init.OverYPosition) 
					overLineTime += Time.deltaTime;
                else 
					overLineTime = 0;
			}

			if(overLineTime > 3f) {
				ObjectFlickerAnimation();
				overLineTime = 0;
			}

			waringLineTime += Time.deltaTime;
			yield return null;
		}
	}

	public void Setting() {
		isReady = false;

		try {
			radius = GetRadius();
			gameObject.AddComponent<CircleCollider2D>();
			gameObject.GetComponent<CircleCollider2D>().radius = radius;
			gameObject.GetComponent<Rigidbody2D>().gravityScale = GRAVITY_SCALE;

		} catch (System.NullReferenceException e) {
			Debug.Log(e.StackTrace);
		}

		_spriteRenderer.DOFade(0.0f, duraitionSeconds).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

		StartCoroutine(CoReady());
	}

	private float GetRadius() {
		sprite = GetComponent<SpriteRenderer>().sprite;
		return sprite.rect.width / (sprite.pixelsPerUnit * 0.01f) * 0.005f;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (IsEquals(collision.gameObject) && !isMerging) {
			isMerging = true;
			TargetPosCheckAndMerge(collision.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (IsEquals(collision.gameObject) && !isMerging) {
			isMerging = true;
			TargetPosCheckAndMerge(collision.gameObject);
		}
	}

	private bool IsEquals(GameObject collision) {
		if (collision.CompareTag("object") && CompareTag("object")) {
			if (this.mergeLevel == collision.GetComponent<MainObject>().mergeLevel)
				return true;
		}
		return false;
	}

	private void TargetPosCheckAndMerge(GameObject collision) {
		if(this.transform.position.y >= collision.transform.position.y) {
			Merge(collision);
		}
	}

	private void Merge(GameObject collision) {
		ObjectManager.init.MergeObject(collision.GetComponent<MainObject>(), this);
	}

	public void ObjectFlickerAnimation() {
		if(!GameManager.IsGamePause) {
			StartCoroutine(CoFlicker());
        }
	}

	public float GetYPosition() {
		return (this.transform.position.y + radius);
    }

	private float duraitionSeconds = 1f;
	private WaitForSeconds _wfs = new WaitForSeconds(3f);

	private IEnumerator CoFlicker() {
		yield return null;

		if (GameManager.IsGamePause)
			yield break;

		GameManager.IsGamePause = true;
		_spriteRenderer.DOFade(0.0f, duraitionSeconds).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

		yield return _wfs;

		GameManager.OnBindGameOver?.Invoke();
    }

	private IEnumerator CoReady() {
		yield return new WaitForSeconds(0.5f);
		isReady = true;
	}
}