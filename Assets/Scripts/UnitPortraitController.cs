using UnityEngine;
using System.Collections;

public class UnitPortraitController : MonoBehaviour {

	#region Singleton
	public static UnitPortraitController Instance { get; set; }
	#endregion

	GameObject Unit;
	GameObject myPrefab;

	// Use this for initialization
	void Awake () {
		Unit = transform.Find ("Unit").gameObject;
		Instance = this;
	}

	public void LoadUnit (GameObject prefab) {
		unloadUnit ();
		myPrefab = GameObject.Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
		myPrefab.transform.parent = Unit.transform;
		myPrefab.transform.position = Unit.transform.position;
		myPrefab.transform.rotation = Unit.transform.rotation;
	}

	public void unloadUnit () {
		GameObject.Destroy (myPrefab);
	}

	public GameObject getUnit () {
		return myPrefab;
	}
}