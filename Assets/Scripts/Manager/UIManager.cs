using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour {
	private static readonly int COIN_ABOUT_SCORE = 55;
	private static readonly double ADD_COIN_ADS_DELAY = 5;
	private static readonly float MAX_LEVEL_PANEL_OFF_DELAY = 2.5f;
	private static readonly float DESTROY_MAX_OBJECT_DELAY = 1.0f;

	public UnityAction OnShopClick;

	public static UIManager init = null;
	private void Awake() {
		if (init == null) {
			init = this;
			audioSource = GetComponent<AudioSource>();
		} else if (init != this) {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public AudioClip uiBtn;
	public AudioClip gameOver;
	public AudioClip maxlevel;
	public AudioClip effectSound;
	public AudioClip destroymaxlevel;
	public AudioClip addCoin;

	public AudioSource audioSource;

	public TextMeshProUGUI errorLogPanel;

	[SerializeField] private Button _shopButton;
	[SerializeField] private ViewCanvasShop _viewCanvasShop;

    private void Start() {
		OnShopClick += () => { _viewCanvasShop.SetActive(true); };
	}

}