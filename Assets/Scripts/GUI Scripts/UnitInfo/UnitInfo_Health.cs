using UnityEngine;
using System.Collections;

public class UnitInfo_Health : MonoBehaviour {
	
	#region Singleton
	
	public static UnitInfo_Health Instance { get; set; } //The singleton for this script. It can be called from any other script by typing UnitInfo_Health.Instance
	//as long as the UnitInfo_Health has been initialized.
	
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
