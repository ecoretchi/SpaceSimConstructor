using UnityEngine;
using System.Collections;
namespace Stackables {
    public class Socket : MonoBehaviour {

        //using to sure that socket are compatible each other, only same type could be joined and connected
        public enum DimensionType {
            Empty, //empty socket does not interact
            Small,
            Medium,
            Large,
        };

        public enum OrientationType {
            Hybrid, //could be any connected to any
            Straight, //horizontal oriented, module could connected over horizontals plane
            Up, //vertical oriented
            Down //vertical oriented
        };

        //public enum Type {

        public enum State {
            Disabled,
            Enabled, // if socket enabled it can interact with other construction
			Rejected, // if parent socket could be conencted in case collision his stackable owner 
            Sticked, // convergence with other socket (on other construction)
            Connected, // connected with other socket (on other construction), mean that user release construction during it was sticked
            Welded // mean that user confirm the connection state for constructions
        }

        public Socket collision { get; protected set; }
        public Socket joined { get; protected set; }
        public DimensionType dimType;
        public OrientationType orientedType;
        public State state { get; set; }

        void Awake() {

            if (dimType == DimensionType.Empty) {
                if (name == "socket_S")
                    dimType = DimensionType.Small;
                else if (name == "socket_M")
                    dimType = DimensionType.Medium;
                else if (name == "socket_L")
                    dimType = DimensionType.Large;
            }
        }

        public T GetComponentInParents<T>(int deep = 2){
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
        public bool IsCompatible(Socket mother, Socket father = null) {
            if(father == null)
                father = this;
            if (father.IsSticked())
                return true; //already sticked
            if ((mother.IsEnabled() || mother.IsSticked()) && mother.dimType != DimensionType.Empty) {

                return father.dimType == mother.dimType && IsOrientedEachOther(mother,father);
            }
            return false;
        }
        public bool IsOrientedEachOther(Socket mother, Socket father = null) {
            if (father == null)
                father = this;
            if (mother.orientedType == OrientationType.Hybrid)
                return true;
            if (father.orientedType == OrientationType.Hybrid)
                return true;
            if (mother.orientedType == OrientationType.Straight)
                return father.orientedType == OrientationType.Straight;
            if (mother.orientedType == OrientationType.Up)
                return father.orientedType == OrientationType.Down;
            if (mother.orientedType == OrientationType.Down)
                return father.orientedType == OrientationType.Up;
            return false;
        }

        public bool IsDisabled() {
            return state == State.Disabled;
        }
        public bool IsEnabled() {
            return state == State.Enabled;
        }
        public bool IsRejected() {
            return state == State.Rejected;
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
		public void OnCollision (Socket s) {
			collision = s;
			state = State.Rejected;
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
			collision = null;
            state = State.Enabled;

            gameObject.layer = LayerMask.NameToLayer("Construction");
        }
        public void OnDisable() {
            if (joined)
                joined.OnRelease();
            joined = null;
			collision = null;
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