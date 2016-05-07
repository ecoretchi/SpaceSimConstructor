using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public Menu InfoMenu;//root menu
    public Menu ModuleInfoMenu;
    public Menu CurrentMenu;
	// Use this for initialization
	void Start () {
        ShowMenu(CurrentMenu);
	}
    public void ShowMenu(bool val) {
        if (val == false) {
            ShowMenu(InfoMenu);
        } else {
            ShowMenu(ModuleInfoMenu);
        }
    }
    public void ShowMenu(Menu m) {
        if (CurrentMenu != null)
            CurrentMenu.IsOpen = false;
        CurrentMenu = m;
        CurrentMenu.IsOpen = true;
    }
}
