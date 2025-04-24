using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewAttendanceUnit : MonoBehaviour
{
    public bool Enabled
    {
        get => checkButton.enabled;
        set => checkButton.enabled = value;
    }

    public Image EarnIcon => earnIcon;
    
    [SerializeField] private Button checkButton;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text rewardAmount;
    [SerializeField] private Image earnIcon;
    [SerializeField] private Image backgroundImage;
    
    public ViewAttendanceUnit SetDay(string text)
    {
        dayText.text = text;
        return this;
    }

    public ViewAttendanceUnit SetRewardAmount(int amount)
    {
        rewardAmount.text = amount.ToString();
        return this;
    }

    public ViewAttendanceUnit OnClickListeners(UnityAction action)
    {
        checkButton.onClick.RemoveAllListeners();
        checkButton.onClick.AddListener(action);
        return this;
    }

    public ViewAttendanceUnit SetEarnedAttendance(bool isEarned)
    {
        // background color
        // get { 6230F5 } default { FDEEDC }
        // text color
        // get { white } default { 3D4D65 } 
        earnIcon.gameObject.SetActive(isEarned);
        string backgroundHexColor = isEarned ? "#6230F5" : "#FDEEDC";
        string textHexColor = isEarned ? "#FFFFFF" : "#3D4D65";
        Color color;
        if(ColorUtility.TryParseHtmlString(backgroundHexColor, out color))
        {
            backgroundImage.color = color;
        }

        if (ColorUtility.TryParseHtmlString(textHexColor, out color))
        {
            dayText.color = color;
            rewardAmount.color = color;
        }

        return this;
    }
}