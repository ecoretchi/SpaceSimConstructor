using UnityEngine;
using System.Collections;
namespace Stackables {
    public class Socket : MonoBehaviour {

        //using to shure that socket are compatible each other, only same type could be joined and connected
        public enum Type {
            Empty, //empty socket does not interact
            Small,
            Medium,
            Large,
        };

        public enum State {
            Disabled,
            Enabled, // if socket enabled it can interact with other construction
            Sticked, // convergence with other socket (on other construction)
            Connected, // connected with other socket (on other construction), mean that user release construction during it was sticked
            Welded // mean that user confirm the connection state for constructions
        }

        [HideInInspector]
        public Socket joined { get; protected set; }

        public Type type;

        [System.ComponentModel.DefaultValue(State.Disabled)]
        public State state { get; set; }

        public T GetComponentInParents<T>(int deep){
            Transform tr = transform;
            T t = tr.GetComponentInParent<T>();
            if(t == null)
            for (int i = 0; i < deep; ++i)
            {
                tr = tr.parent;
                if (tr == null)
                    break;
                t = tr.GetComponentInParent<T>();
                if (t != null)
                    break;
            }
            return t;
        }
        public bool IsCompatible(Socket s) {
            if (IsSticked())
                return true; //already sticked
            if( (s.IsEnabled() || s.IsSticked() ) && s.type != Type.Empty)
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

            gameObject.layer = LayerMask.NameToLayer("Construction");
        }
        public void OnDisable() {
            if (joined)
                joined.OnRelease();
            joined = null;
            state = State.Disabled;

            gameObject.layer = 0; //LayerMask.NameToLayer("Default");
        }
        public bool Enable(bool flag) {
            if (IsEnabled() || IsDisabled()) {
                state = flag ? State.Enabled : State.Disabled;
                return true;
            }
            return false;
        }

    }

} // namespace Stackables