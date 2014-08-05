using UnityEngine;
using System.Collections;

public class UnitInfo_Range : MonoBehaviour {
	
	#region Singleton
	
	public static UnitInfo_Range Instance { get; set; } //The singleton for this script. It can be called from any other script by typing UnitInfo_Range.Instance
	//as long as the UnitInfo_Range has been initialized.
	
	#endregion
	
	public UILabel Label;
	public UISprite Sprite;
	
	void Awake () {
		Label = this.gameObject.GetComponentInChildren <UILabel> ();
		Sprite = this.gameObject.GetComponentInChildren <UISprite> ();
		Instance = this;
		Label.text = "";
 }
}
