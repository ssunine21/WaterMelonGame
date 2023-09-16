using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    private void Start() {
        new ControllerMainNav(this.transform);
        new ControllerMainMenu(this.transform);
        new ControllerInGame(this.transform);
        new ControllerToast(this.transform);
        new ControllerShop(this.transform);
        new ControllerOption(this.transform);
        new ControllerGameOver(this.transform);
        new ControllerAttendance(this.transform);
    }
}
