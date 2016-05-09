using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    public Menu BuildingMenu;
    public Menu BuildConnectorsMenu;

    public Menu InfoMenu;    
    public Menu ModuleInfoMenu;

    protected SortedDictionary<MenuGroup, Menu> CurrentMenus;


    public enum MenuGroup {
        LEFT_PANEL,
        RIGHT_PANEL
    }
    // Use this for initialization
    void Start() {
        CurrentMenus = new SortedDictionary<MenuGroup, Menu>();
        CurrentMenus.Add(MenuGroup.LEFT_PANEL, null);
        CurrentMenus.Add(MenuGroup.RIGHT_PANEL, null);
        ShowMenu(InfoMenu);
        ShowMenu(BuildingMenu);
    }


    public void ShowModuleInfoMenu(bool val) {
        if (val == false) {
            ShowMenu(InfoMenu);
        } else {
            ShowMenu(ModuleInfoMenu);
        }
    }

    public void ShowMenu(Menu m_) {
        Menu CurrentMenu = CurrentMenus[m_.menuGroup];
        if (CurrentMenu != null) {
            CurrentMenu.IsOpen = false;
            if (m_)
                m_.IsOpenEx = CurrentMenu.IsOpenEx;
        }
        m_.IsOpen = true;
        CurrentMenus[m_.menuGroup] = m_;        
    }
    public void CloseExtendedMenu(Menu m_) {
        if (m_ != null) {
            m_.IsOpenEx = false;

            Menu CurrentMenu = CurrentMenus[m_.menuGroup];
            if (m_ != CurrentMenu && CurrentMenu)
                CurrentMenu.IsOpen = false;
        }
    }

    public void ShowExtendedMenu(Menu m_) {
        
        Menu CurrentMenu = CurrentMenus[m_.menuGroup];

        //if (m_ == CurrentMenu && m_.IsPlaying()) {
        //    print("ShowExtendedMenu - IsPlaying,  ... skip");
        //    return;
        //}
        //print("ShowExtendedMenu");

        if (m_!= null ) {
            m_.IsOpenEx = true;// !m_.IsOpenEx;

            if (m_ != CurrentMenu && CurrentMenu)
                CurrentMenu.IsOpen = false;

            if (m_.IsOpenEx && !m_.IsOpen)
                m_.IsOpen = true;
        
        }
        
        CurrentMenus[m_.menuGroup] = m_;
    }
    /// <summary>
    /// if current menu extended try open corresponded extended menu
    /// </summary>
    /// <param name="m_"></param>
    public void ShowAuto(Menu m_) {
        
        if(CurrentMenus[m_.menuGroup].IsOpenEx)
            ShowExtendedMenu(m_);
        else
            ShowMenu(m_);
    }
}
