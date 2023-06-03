using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ButtonExpansion : Button {

    public float UpScale = 1.05f;
    public float DownScale = 0.95f;

    public GameObject LockPanel;
    public GameObject UnLockPanel;

    private bool isDoubleClick = false;
    private RectTransform rectTr;
    private CancellationTokenSource _cts;

    [SerializeField] private AudioClip _clickAudioClip;

    protected override void Start() {
        rectTr = GetComponent<RectTransform>();
        _cts = new CancellationTokenSource();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (!isDoubleClick)
            base.OnPointerClick(eventData);

        isDoubleClick = true;

        AudioManager.Init.Play(Definition.AudioType.UIButton);

        Scale();
    }

    private async void Scale() {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
        try {
            await RoutineDownScale(_cts);
            await RoutineUpScale(_cts);
            await RoutineOriginScale(_cts);
        } catch(System.Exception e) {
            Debug.LogException(e);
        }
        rectTr.localScale = Vector3.one;
        isDoubleClick = false;
    }

    private async UniTask RoutineDownScale(CancellationTokenSource cts) {
        var goal = Vector3.one * DownScale;

        while (Vector3.Distance(rectTr.localScale, goal) > 0.01) {
            rectTr.localScale = Vector3.Lerp(rectTr.localScale, goal, 0.5f);
            await UniTask.Yield(cts.Token);
        }
    }

    private async UniTask RoutineUpScale(CancellationTokenSource cts) {
        var goal = Vector3.one * UpScale;
        while (Vector3.Distance(rectTr.localScale, goal) > 0.01) {
            rectTr.localScale = Vector3.Lerp(rectTr.localScale, goal, 0.5f);
            await UniTask.Yield(cts.Token);
        }
    }

    private async UniTask RoutineOriginScale(CancellationTokenSource cts) {
        var goal = Vector3.one;
        while (Vector3.Distance(rectTr.localScale, goal) > 0.01) {
            rectTr.localScale = Vector3.Lerp(rectTr.localScale, goal, 0.5f);
            await UniTask.Yield(cts.Token);
        }
    }

    public void SetLock(bool flag) {
        if (LockPanel != null)
            LockPanel.SetActive(flag);

        if (UnLockPanel != null)
            UnLockPanel.SetActive(!flag);
    }
}