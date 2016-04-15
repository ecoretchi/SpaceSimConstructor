using UnityEngine;
using System.Collections;

public class Socket : MonoBehaviour {

    //using to shure that socket are compatible each other, only same type could be joined and connected
    public enum Type {
        Empty,
        Small,
        Medium,
        Large,
    };
    
    public enum State {
        Disabled,
        Enabled, // if socket enabled it can interact with other construction
        Sticked, // convergence with other socket (on other construction)
        Connected, // conected with other socket (on other construction)
        Welded // mean that user confirm the connection state for constructions
    }

    public Socket joined{ get; protected set; }

    public Type type;

    [System.ComponentModel.DefaultValue(State.Disabled)]
    public State state { get; set; }

    public bool IsCompatible(Socket s) {
        if (s.IsEnabled() && s.type!=Type.Empty)
            return this.type == s.type;
        return false;
    }
    public bool IsDisabled() {
        return state == State.Disabled;
    }
    public bool IsEnabled() {
        return state == State.Enabled;
    }
    public bool IsSticked() {
        return state == State.Sticked;
    }
    public bool IsConnected() {
        return state == State.Connected;
    }
    public bool IsWelded() {
        return state == State.Welded;
    }

    public void OnStick(Socket s) {
        joined = s;
        state = State.Sticked;
    }

    public bool OnConnect() {
        if (!IsSticked())
            return false;
        state = State.Connected;
        gameObject.layer = 0;// LayerMask.NameToLayer("Construction");
        return true;
    }
    public bool OnWeld() {
        if (!IsConnected())
            return false;
        state = State.Welded;
        return true;
    }
    
    public void OnRelease() {
        joined = null;
        state = State.Enabled;

        gameObject.layer =  LayerMask.NameToLayer("Construction");
    }

    public bool Enable(bool flag) {
        if (IsEnabled() || IsDisabled()) {
            state = flag ? State.Enabled : State.Disabled;
            return true;
        }           
        return false;
    }

}