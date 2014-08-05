using UnityEngine;

[RequireComponent(typeof(HUDText))]
public class Test : MonoBehaviour{
	HUDText ht;
	string sayWhat = "";
	void Update()
	{
		Debug.Log ("Is This Running?!?!?!? v3");
		UILabel stuff = GetComponent<UILabel> ();
		//stuff.text ("Hello");
		//System.Type stuff = ht.GetComponentInChildren ("UILabel");
		          
	}
	void Start()
	{
		
		ht = GetComponent<HUDText> ();
		ht.Add(sayWhat, Color.white, 0f);
	}
}