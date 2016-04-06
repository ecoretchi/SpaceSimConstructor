using System;
using UnityEngine;


namespace Spacecraft {

	/// <summary>
	/// Generic spacecraft class
	/// </summary>
	public class Spacecraft_Generic : MonoBehaviour, ISpaceEntity, IShipControls {

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