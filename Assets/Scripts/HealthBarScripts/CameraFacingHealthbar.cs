using UnityEngine;
using System.Collections;

public class CameraFacingHealthbar: MonoBehaviour {
	
	Camera m_Camera;
	
	public float curHealth;
	public float maxHealth;

	private string healthString;

	public float lastHealth;

	float lengthModifier = 1;

	Vector3 greenPos;

	Unit healthScript;


	// Use this for initialization
	void Start () {
		m_Camera = Camera.main;
	}

	void Awake () {
		//Loads unit health value from healthscript
		healthScript = transform.parent.gameObject.GetComponent<Unit>();

		curHealth = healthScript.Health;
		maxHealth = healthScript.MaxHealth;

		//stores two health values
		lastHealth = curHealth;

		greenPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		//Keeps health bar facing camera (Billboarding)
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.back, m_Camera.transform.rotation * Vector3.up);
		
		curHealth = healthScript.Health;
		maxHealth = healthScript.MaxHealth;

		//When unit takes damage, bar shifts to the left, making it appear that damage is coming off the right side of bar.  **Currently not working due to commented out code**
		if (lastHealth > curHealth){
			greenPos = transform.localPosition;
//			greenPos.z = (lastHealth - curHealth) / curHealth;
			lastHealth = curHealth;
			healthString = UnitInfo_Health.Instance.Label.text;
		}

		Vector3 greenScale = transform.localScale;
		greenScale.x = curHealth / maxHealth;
		transform.localPosition = greenPos;
		transform.localScale = greenScale;
	}
}
