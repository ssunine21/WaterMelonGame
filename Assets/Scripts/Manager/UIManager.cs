using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour {

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