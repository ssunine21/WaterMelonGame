    using TMPro;
    using UnityEngine;

    public class ViewCanvasAttendance : ViewCanvas
    {
        public ViewAttendanceUnit[] ViewAttendanceUnits => units;
        public ButtonExpansion[] CloseButtons => _closeButtons;
        
        [SerializeField] private ViewAttendanceUnit[] units;
        [SerializeField] private ButtonExpansion[] _closeButtons;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private TMP_Text timeText;

        public void SetLocalized()
        {
            titleText.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.Attendance);
            descText.text = LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.AttendanceDesc);

            int baseTime = ServerTime.BaseTime();
            timeText.text = string.Format(LocalizationManager.init.GetLocalizedValue(Definition.LocalizeKey.BaseTime), baseTime);
        }
    }
