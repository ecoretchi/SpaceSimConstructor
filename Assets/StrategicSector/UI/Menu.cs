using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public MenuManager.MenuGroup menuGroup;
    private Animator _animator;
    //private CanvasGroup _canvasGroup;
    //private Animation _animation;

    public Menu parentMenu;

    public bool IsOpen {
        get { return _animator.GetBool("IsOpen"); }
        set { _animator.SetBool("IsOpen", value); }
    }

    public bool IsOpenEx {
        get { return _animator.GetBool("IsOpenEx"); }
        set { _animator.SetBool("IsOpenEx", value); }
    }

	// Use this for initialization
	void Awake() {
        _animator = GetComponent<Animator>();
        //_canvasGroup = GetComponent<CanvasGroup>();
        //_animation = GetComponent<Animation>(); 

        var rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = Vector2.zero;

        IsOpen = false;
        IsOpenEx = false;
    }
    //public bool IsPlaying() {
    //    if (_animation != null) 
    //        return _animation.IsPlaying("BuildExtended");
    //    return false;
    //}	
	// Update is called once per frame
	void Update () {
        //if (_animation != null) {
        //    if (_animation.isPlaying) {
        //        _canvasGroup.blocksRaycasts = _canvasGroup.interactable = false;
        //    } else {
        //        _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        //    }
        //}

        //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
        //    
        //} else {
        //    _canvasGroup.blocksRaycasts = _canvasGroup.interactable = true;
        //}
	}
}
