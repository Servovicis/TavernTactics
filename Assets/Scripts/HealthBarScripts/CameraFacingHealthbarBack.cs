using UnityEngine;
using System.Collections;

public class CameraFacingHealthbarBack : MonoBehaviour {

	Camera m_Camera;

	// Use this for initialization
	void Start () {
		m_Camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		//Keeps health bar facing camera (Billboarding)
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
	}
}
