using UnityEngine;
using System.Collections;

public class CameraPivot : MonoBehaviour {

	#region Singleton
	
	public static CameraPivot Instance { get; set; } //The singleton for this script. It can be called from any other script by typing CameraPivot.Instance
	//as long as the GameManager has been initialized.

	#endregion

	public float RotateSpeed;
	public float rotationDirection = 0f;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			rotationDirection = 0f;

		if (Input.GetKey(KeyCode.LeftArrow))
			rotationDirection = 1f;
		else if (Input.GetKey(KeyCode.RightArrow))
			rotationDirection = -1f;

		transform.Rotate (transform.up, rotationDirection * RotateSpeed * Time.deltaTime);	
	}


}
