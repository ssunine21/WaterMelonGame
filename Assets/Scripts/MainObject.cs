using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

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

	public SpriteRenderer SpriteRenderer => _spriteRenderer;
	public CircleCollider2D CircleTrigger => _circleTrigger;

	[SerializeField] private SpriteRenderer _spriteRenderer;

	private Sprite sprite;
	private Rigidbody2D _rigidbody;
	private CircleCollider2D _circleCollider;
	private CircleCollider2D _circleTrigger;

	private LayerMask _layerMask;

	public bool isDropped {
		get {
			try {
				return gameObject.GetComponent<Rigidbody2D>().gravityScale > 0;
			} catch(Exception e) {
				return false;
            }
		}
	}
	public bool isReady;
	private bool isMerging = true;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		radius = GetRadius();
		StartCoroutine(CoAsyncPosition());
	}

	private IEnumerator CoAsyncPosition() {
		float waringLineTime = 0;
		float overLineTime = 0;
		Vector2 tempPosition;
		_layerMask = LayerMask.GetMask("Unit");
		
		while (true) {
			_rigidbody.angularVelocity = Mathf.Lerp(_rigidbody.angularVelocity, 0, 1f);
			tempPosition = transform.position;
			if (transform.position.x - radius <= ObjectManager.init.MinLeftX)
			{
				_rigidbody.angularVelocity = 0;
				tempPosition.x = ObjectManager.init.MinLeftX + radius;
			}
			else if (transform.position.x + radius >= ObjectManager.init.MaxRightX)
			{
				_rigidbody.angularVelocity = 0;
				tempPosition.x = ObjectManager.init.MaxRightX - radius;
			}

			transform.position = tempPosition;

			if (isDropped && isReady) {
				if (waringLineTime > 1f
					&& GetYPosition() > MaxLine.init.WaringYPosition) {
					MaxLine.init.WaringLine();
				}

				if(GetYPosition() > MaxLine.init.OverYPosition) 
					overLineTime += Time.deltaTime;
                else 
					overLineTime = 0;
			}

			if(overLineTime > 2.3f)
			{
				if (!GameManager.IsGamePause)
					ObjectFlickerAnimation();
				overLineTime = 0;
			}

			if (isDropped && !GameManager.IsGamePause)
				waringLineTime += Time.deltaTime;

			if (GameManager.IsGamePause)
				overLineTime = 0;
			
			//collider
			Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius + 0.0001f);
			foreach (Collider2D collider in colliders)
			{
				if (collider.TryGetComponent<MainObject>(out var component))
				{
					if (component.gameObject == this.gameObject) continue;
					
					if (IsEquals(component.mergeLevel) 
					    && !isMerging && isReady
					    && component.isReady) {
						isMerging = true;
						component.isMerging = true;
						TargetPosCheckAndMerge(component);
						break;
					}
				}
			}
			yield return null;
		}
	}

	public void Setting() {
		//isReady = false;

		try {
			radius = GetRadius();
			if (_circleCollider == null)
				_circleCollider = gameObject.AddComponent<CircleCollider2D>();
			_circleCollider.radius = radius - 0.001f;
			if (_circleTrigger == null)
				_circleTrigger = gameObject.AddComponent<CircleCollider2D>();
			_circleTrigger.radius = radius;
			_circleTrigger.isTrigger = true;

			GetComponent<Rigidbody2D>().gravityScale = GRAVITY_SCALE;
		} catch (System.NullReferenceException e) {
			Debug.Log(e.StackTrace);
		}

		StartCoroutine(CoReady());
	}

	private float GetRadius() {
		sprite = _spriteRenderer.sprite;
		return sprite.rect.width / (sprite.pixelsPerUnit * 0.01f) * 0.005f;
	}

    /*private void OnTriggerEnter2D(Collider2D collision) {
		if (IsEquals(collision.gameObject) && !isMerging) {
			isMerging = true;
			TargetPosCheckAndMerge(collision.gameObject);
		}
	}

    private void OnTriggerStay2D(Collider2D collision) {
		if (IsEquals(collision.gameObject) && !isMerging) {
			isMerging = true;
			TargetPosCheckAndMerge(collision.gameObject);
		}
	}*/

	private bool IsEquals(ObjectManager.ObjectKey key)
	{
		return this.mergeLevel == key;
		// if (collision.CompareTag("object") && CompareTag("object")) {
		// 	if (this.mergeLevel == collision.GetComponent<MainObject>().mergeLevel)
		// 		return true;
		// }
		// return false;
	}

	private void TargetPosCheckAndMerge(MainObject collisionTr) {
		if(this.transform.position.y >= transform.position.y) {
			Merge(collisionTr);
		}
	}

	private void Merge(MainObject mainObject) {
		if (mainObject == null) 
			return;
		ObjectManager.init.MergeObject(mainObject, this);
	}

	public void ObjectFlickerAnimation() {
		if(!GameManager.IsGamePause) {
			StartCoroutine(CoFlicker());
        }
	}

	public float GetYPosition() {
		return (this.transform.position.y + radius);
    }

	private float duraitionSeconds = 0.5f;
	private WaitForSeconds _wfs = new WaitForSeconds(3f);

	private IEnumerator CoFlicker() {
		if (GameManager.IsGamePause)
			yield break;

		GameManager.IsGamePause = true;
		
		_spriteRenderer.DOFade(0.0f, duraitionSeconds).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
		
		yield return _wfs;
		GameManager.OnBindGameOver?.Invoke();
		ObjectManager.init.DestroyObject(this);
    }

	private WaitForSeconds wfs = new WaitForSeconds(0.2f);
	private WaitForSeconds wfs2 = new WaitForSeconds(0.2f);
	private IEnumerator CoReady() {
		yield return wfs2;
		isMerging = false;

		yield return wfs;
		isReady = true;
	}
}