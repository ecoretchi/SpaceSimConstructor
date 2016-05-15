using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    public GameObject ModuleInfoPanel;

    public Menu BuildingMenu;

    public Menu InfoMenu;    
    public Menu ModuleInfoMenu;

    protected SortedDictionary<MenuGroup, Menu> CurrentMenus;


    public enum MenuGroup {
        LEFT_PANEL,
        RIGHT_PANEL,
        MOUSE_FLOW_PANEL
    }
    // Use this for initialization
    void Start() {
        CurrentMenus = new SortedDictionary<MenuGroup, Menu>();
        CurrentMenus.Add(MenuGroup.LEFT_PANEL, null);
        CurrentMenus.Add(MenuGroup.RIGHT_PANEL, null);
        CurrentMenus.Add(MenuGroup.MOUSE_FLOW_PANEL, null);
        ShowMenu(InfoMenu);
        ShowMenu(BuildingMenu);
        ShowModuleInfoMenu(false);
    }

    public void ShowModuleInfoMenu(bool val) {
        RectTransform rc = ModuleInfoMenu.GetComponentInChildren<CanvasGroup>().GetComponent<RectTransform>();
        if (val == true) {

            Vector3 pos = Input.mousePosition;// Camera.main.WorldToScreenPoint(Input.mousePosition);     
            //print(pos);
            pos.z = 0;
            float w = Screen.width;
            float h = Screen.height;
            pos.x = pos.x-w/2;
            pos.y = pos.y-h/2;
            rc.localPosition = pos;
            rc.pivot = new Vector2(0.5f,0.5f);

        } else {
            rc.pivot = new Vector2(10, 0.5f);
        }
    }

    public void ShowMenu(Menu m_, bool show = true) {
        Menu CurrentMenu = CurrentMenus[m_.menuGroup];
        if (CurrentMenu != null) {
            if (CurrentMenu.parentMenu) //|| CurrentMenu.menuGroup == MenuGroup.MOUSE_FLOW_PANEL
                CurrentMenu.IsOpen = !show;
            if (m_)
                m_.IsOpenEx = CurrentMenu.IsOpenEx;
        }
        m_.IsOpen = show;
        CurrentMenus[m_.menuGroup] = m_;        
    }
    public void CloseExtendedMenu(Menu m_) {
        if (m_ != null) {
            m_.IsOpenEx = false;

            Menu CurrentMenu = CurrentMenus[m_.menuGroup];
            if (m_ != CurrentMenu && CurrentMenu) {
                if (CurrentMenu.parentMenu)
                    CurrentMenu.IsOpen = false;
                else
                    CurrentMenu.IsOpenEx = false;
            }
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

            if (m_ != CurrentMenu && CurrentMenu) {
                if (CurrentMenu.parentMenu)
                    CurrentMenu.IsOpen = false;
                else
                    CurrentMenu.IsOpenEx = false; 
                
            }

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
