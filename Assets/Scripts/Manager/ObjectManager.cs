using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectManager : MonoBehaviour {
	private static readonly int MAX_CREATE_OBJECT_NUMBER = 5;
	private List<Sprite> objectSprites = new List<Sprite>();

	public enum ObjectKey {
		None = -1,
		Zero,
		One,
		Two,
		Three,
		Four, 
		Five, 
		Six, 
		Seven,
		Eight, 
		Nine,
		Ten,
		Max
	}

	public static ObjectManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
		} else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public ParticleSystem Particle;
	public GameObject[] objects;
	public GameObject[] backgroundPrefabs;
	public BlockManager[] block;

	public List<MainObject> MainObjects => _mainObjecs;
	public float MinLeftX => GetPos(true);
	public float MaxRightX => GetPos(false);

	public int currBackgroundNum;
	public int currStyleNum;

	private GameObject _objParent;
	private List<MainObject> _mainObjecs = new List<MainObject>();
	private Queue<ParticleSystem> particleSystems = new Queue<ParticleSystem>();

	private float _initY;

	private void Start() {
		_objParent = new GameObject("objParent");

		GameManager.OnBindGoHome += InitObj;
		
		SetObjectSprite(DataManager.init.gameData.styleNum);
		ObjectsSizeAsync();
	}

	private void ObjectsSizeAsync() {
		float objRate = 1;// GameManager.Width / 720f;
		// screenRate / 0.5625f;

		_objParent.transform.localScale = Vector3.one * objRate;

		float hight = GameManager.ObjectHeight;
		_initY = Camera.main.ScreenToWorldPoint(Vector2.one * hight).y;
	}

	public MainObject GetRandomObject() {
		ObjectKey key = (ObjectKey)Random.Range(0, MAX_CREATE_OBJECT_NUMBER);
		return GetObject(key);
    }

	public MainObject GetObject(ObjectKey key, Vector3? pos = null) {
		if (!pos.HasValue) 
			pos = new Vector3(0, _initY, 0);

		int index = (int)key;
		var currObject = Instantiate(objects[index], _objParent.transform).GetComponent<MainObject>();
		currObject.transform.position = pos.Value;
		currObject.SpriteRenderer.sprite = objectSprites[(int)key];
		currObject.SpriteRenderer.transform.localPosition = Vector3.zero;

		if (_mainObjecs == null)
			_mainObjecs = new List<MainObject>();

		_mainObjecs.Add(currObject);

		return currObject;
	}

	public void MergeObject(MainObject target, MainObject curr) {
		if (target.mergeLevel == ObjectKey.Max) 
			return;
		
		curr.CircleCollider.enabled = false;

		var position = target.transform.position;

		DataScore.EarnCurrScore((int)(target.mergeLevel + 1) * 4);

		/*curr.transform.DOMove(position, 0.08f).SetEase(Ease.OutBack).OnComplete(() =>
		{*/
			var data = GetObject(target.mergeLevel + 1, position).GetComponent<MainObject>();
			data.Setting();
			
			DestroyObject(target);
			DestroyObject(curr);

			AudioManager.Init.Play(Definition.AudioType.Destroy);

			PlayerParticle(position).Forget();
			if (DataManager.init.gameData.isVibration)
				Vibratior.Vibrate();
		//});
	}

	private async UniTaskVoid PlayerParticle(Vector3 pos)
    {
		if (particleSystems.Count == 0)
			particleSystems.Enqueue(Instantiate(Particle));

		var particle = particleSystems.Dequeue();
		particle.transform.position = pos;
		particle.Play();

		await UniTask.Delay(1000);
		particleSystems.Enqueue(particle);
	}

	public void InitObj() {
		DataManager.init.gameData.objectData.Clear();

		foreach (var obj in _mainObjecs) {
			if (obj == null) continue;
			if (obj.gameObject.activeSelf && obj.isDropped)
				DataManager.init.gameData.objectData.Add(new DataInfo.GameObjectData(obj.transform.position, obj.mergeLevel));
		}
		Destroy(_objParent);
		_mainObjecs.Clear();
		_objParent = new GameObject("objParent");
	}

	public void SetObjectSprite(int objNum) {
		currStyleNum = objNum;

#if UNITY_IOS
		currStyleNum++;
		//if (currStyleNum == 0) currStyleNum = 1;
		//else if (currStyleNum == 1) currStyleNum = 0;
#endif

		if (objectSprites == null)
			objectSprites = new List<Sprite>();

		for(int i = 0; i < (int)ObjectKey.Max + 1; ++i ) {

			if (objectSprites.Count <= i)
				objectSprites.Add(Resources.Load<Sprite>($"obj/obj{currStyleNum}_{i}"));
			else
				objectSprites[i] = Resources.Load<Sprite>($"obj/obj{currStyleNum}_{i}");
		}
	}

	public void RankUpItem() {
		var randomObjects = GetRandomItems(2);
		foreach (var random in randomObjects) {
			MainObject mainObject = GetObject(random.mergeLevel + 1, random.transform.position);
			mainObject.Setting();

			DestroyObject(random);
		}
	}

	public void RerollItem()
	{
		var randomObjects = GetRandomItems(_mainObjecs.Count - 1);
		foreach (var random in randomObjects) {
			if (random.isDropped) {
				ObjectKey key = (ObjectKey)Random.Range(0, (int)ObjectKey.Nine);
				MainObject mainObject = GetObject(key, random.transform.position);
				mainObject.Setting();
				DestroyObject(random);
			}
		}
	}

	public void DestroyItem(int count) {
		var randomObjects = GetRandomItems(count);
		foreach(var random in randomObjects) {
			PlayerParticle(random.transform.position).Forget();
			DestroyObject(random);
        }
	}

	public void DestroyObject(MainObject obj) {
		_mainObjecs.Remove(obj);
		Destroy(obj.gameObject);
	}

	public void DestroyHalf() {
		int count = _mainObjecs.Count / 2;
		DestroyItem(count);
	}

	private List<MainObject> GetRandomItems(int count) {
		if (_mainObjecs.Count <= count)
			return _mainObjecs.Where(x => x.isDropped && x.isActiveAndEnabled).ToList();
		else {
			System.Random random = new System.Random();
			return _mainObjecs.Where(x => x.isDropped && x.isActiveAndEnabled).OrderBy(x => random.Next()).Take(count).ToList();
		}
    }

	private float GetPos(bool isLeft) {
		Vector2 pos;
		float maxWidth = GameManager.Width;
		float maxHeight = GameManager.Height;

		if (maxHeight < maxWidth) maxWidth = 1080;

		float realHalf = Screen.width * 0.5f;
		float ingameHalf = maxWidth * 0.5f;// > 540 ? 540 : realHalf;


		if (isLeft) {
			pos = Camera.main.ScreenToWorldPoint(new Vector2(realHalf - ingameHalf, 0));
			return pos.x;
		} else {
			pos = Camera.main.ScreenToWorldPoint(new Vector2(realHalf + ingameHalf, 0));
			return pos.x;
		}
	}
}