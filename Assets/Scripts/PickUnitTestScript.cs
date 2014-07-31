using UnityEngine;
using System.Collections;

public class PickUnitTestScript : MonoBehaviour {
	
	bool buttonIsPressed = false;
	public SpriteManager HealthbarSpriteManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () 
	{
		if(Input.GetButton ("press"))
			{
				if (!buttonIsPressed) {
					//UpdateHealthbarToUnit(GameObject.Find ("SkeletonArcher"), -10f);
					SkeletonArcherKind tempUnitScript = GameObject.Find ("SkeletonArcher(Clone)").GetComponent<SkeletonArcherKind>(); 
					if (tempUnitScript != null)
						tempUnitScript.Health -= 1;
					buttonIsPressed = true;
				}
			}
			else if (buttonIsPressed)
				buttonIsPressed = false;
				
		//rotate the sprite so its facing the camera
		 	transform.eulerAngles = new Vector3 
			(
				Camera.main.transform.eulerAngles.x,
				Camera.main.transform.parent.gameObject.transform.eulerAngles.y,
				transform.eulerAngles.z
				);
	}

	//Update Healthbar when unit health changed
	public void UpdateHealthbarToUnit(GameObject unit, float HealthDiff)
	{
		float maxHealth = (float) unit.GetComponent<Unit>().MaxHealth;
		float currentHealth = (float) unit.GetComponent<Unit>().Health;
		float newHealth = currentHealth + HealthDiff;
		
		//apply the new Health
		//unit.GetComponent<Unit>().Health = newHealth;
		
		if(newHealth > 0f)
		{
			//get healthbar client
			GameObject healthbarClient = unit.transform.FindChild("HealthBar").gameObject;
			
			float OnePercent = maxHealth / 100f;
			float HealthPercentage = newHealth / OnePercent;
			
			Debug.Log (HealthPercentage);
		}
	}
}