using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class Switch : MonoBehaviour
{
    public UnityAction callback;

    private Button button;
    private Animator animator;

    private Image bgEnabledImage;
    private Image bgDisabledImage;

    private Image handleEnabledImage;
    private Image handleDisabledImage;

    private bool switchEnabled;

    private void Awake()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();

        bgEnabledImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        bgDisabledImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        handleEnabledImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        handleDisabledImage = transform.GetChild(1).GetChild(1).GetComponent<Image>();

        switchEnabled = true;

    }

    private void OnEnable()
    {
        button.onClick.AddListener(Toggle);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Toggle);
    }

    public void Toggle()
    {
        switchEnabled = !switchEnabled;
        if (switchEnabled)
        {
            bgDisabledImage.gameObject.SetActive(false);
            bgEnabledImage.gameObject.SetActive(true);
            handleDisabledImage.gameObject.SetActive(false);
            handleEnabledImage.gameObject.SetActive(true);
        }
        else
        {
            bgEnabledImage.gameObject.SetActive(false);
            bgDisabledImage.gameObject.SetActive(true);
            handleEnabledImage.gameObject.SetActive(false);
            handleDisabledImage.gameObject.SetActive(true);
        }

        animator.SetTrigger(switchEnabled ? "Enable" : "Disable");
        callback?.Invoke();
    }

    public void Toggle(bool flag)
    {
        switchEnabled = !flag;
        Toggle();
    }

    public bool IsToggled()
    {
        return switchEnabled;
    }
}