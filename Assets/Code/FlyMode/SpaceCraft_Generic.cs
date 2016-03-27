using System;
using UnityEngine;

namespace FlyMode {

    /// <summary>
    /// Generic spacecraft class
    /// </summary>
    public class SpaceCraft_Generic : MonoBehaviour, ISpaceEntity {

        public string GUID {
            get {
                throw new NotImplementedException();
            }
        }

        public string e_name {
            get {
                return gameObject.name;
            }
        }

    }
}
