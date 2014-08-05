using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0414 // variable is assigned but its value is never used.
public class HealthManager : MonoBehaviour {

	public GameObject HealthBarClient;
	#region Singleton
	
	private int unitLayer = 12;
	public static HealthManager Instance{ get; set;}

	#endregion
	
	public SpriteManager HealthbarSpriteManager;
	public static int countstuff = 0;

	void Start()
	{
		Instance = this;
		
		UnitController.onUnitSpawn += AddHealthbarToUnitSpawned;

		UnitController.onUnitDead += RemoveHealthBarToUnitDead;

		AddHealthBarToUnits();
	}

	//When a unit is spawned, add the healthbar
	public void AddHealthbarToUnitSpawned(GameObject unit)
	{
		GameObject healthBarClient = unit.transform.Find("HealthBar").gameObject;
		Debug.Log ("count: "+countstuff);
		countstuff++;
		
		
//		Sprite healthbarSprite = HealthbarSpriteManager.AddSprite(healthBarClient, 1f, .35f, new Vector2(0f, 0.9f), new Vector2(0.1f, 0.1f), Vector3.zero, false); 

//		healthBarClient.GetComponent<HealthBarClient>().myHealthBarSprite = healthbarSprite;
		HealthbarSpriteManager.transform.position = new Vector3(HealthbarSpriteManager.transform.position.x,HealthbarSpriteManager.transform.position.y+3.0f,HealthbarSpriteManager.transform.position.z);

	}

	//Remove healthbar when unit is dead
	public void RemoveHealthBarToUnitDead(GameObject unit)
	{
		GameObject healthBarClient = unit.transform.Find("HealthBar").gameObject;
//	
//		Sprite healthbarSprite = healthBarClient.GetComponent<HealthBarClient>().myHealthBarSprite as Sprite;

//		HealthbarSpriteManager.RemoveSprite(healthbarSprite);
	}

	//Call once to assign healthbars to units already in the game
	public void AddHealthBarToUnits()
	{
//		GameObject[] GameObjectArray = FindObjectsOfType(typeof(GameObject))as GameObject[];
//		List<GameObject> unitList = new List<GameObject>();

//		for(int i = 0; i < GameObjectArray.Length; i++)
//		{
//			if(GameObjectArray[i].layer == unitLayer)
//				unitList.Add(GameObjectArray[i]);
		}
		
//		if(unitList.Count == 0)
//			return;
	
//		GameObject[] Units = unitList.ToArray();
		
//		GameObject thisUnit = this.gameObject;
	
		//for(int i = 0; i < Units.Length; i ++)
		//{
//			AddHealthbarToUnitSpawned(thisUnit);				
		//}
//		HealthbarSpriteManager.transform.position = new Vector3(HealthbarSpriteManager.transform.position.x,1.0f,HealthbarSpriteManager.transform.position.z);

//		return;
//	}
}