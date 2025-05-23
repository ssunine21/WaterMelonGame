using DataInfo.Controller;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    private void Start()
    {
        new ControllerLoading(transform);
        new ControllerMainNav(this.transform);
        new ControllerMainMenu(this.transform);
        new ControllerInGameBackground(this.transform);
        new ControllerInGame(this.transform);
        new ControllerToast(this.transform);
        new ControllerShop(this.transform);
        new ControllerOption(this.transform);
        new ControllerGameOver(this.transform);
        new ControllerAttendance(this.transform);
        new ControllerObjectBook(this.transform);
        new ControllerBook(this.transform);
        new ControllerFx(this.transform);
    }
}
