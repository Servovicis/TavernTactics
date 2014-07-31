using UnityEngine;
using System.Collections;

public class UnitInfo_Attack : MonoBehaviour {

	#region Singleton
	
	public static UnitInfo_Attack Instance { get; set; } //The singleton for this script. It can be called from any other script by typing UnitInfo_Attack.Instance
	//as long as the UnitInfo_Attack has been initialized.
	
	#endregion

	public UILabel Label;
	public UISprite Sprite;
	
	void Awake () {
		Label = this.gameObject.GetComponentInChildren <UILabel> ();
		Sprite = this.gameObject.GetComponentInChildren <UISprite> ();
		Instance = this;
		Label.text = "";
		Sprite.enabled = false;
	}
}
