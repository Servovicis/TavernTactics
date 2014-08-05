using UnityEngine;
using System.Collections;

public class GUILeftPaneButton : MonoBehaviour {

	public int MyButtonNumber;
	public GameManager.GenericFunction onClick;
	public GameManager.GenericFunction Hover;
	public GameManager.GenericFunction unHover;
	public UILabel myLabel;

	void Awake () {
		myLabel = this.gameObject.GetComponentInChildren <UILabel> ();
		GameManager.Instance.LeftPaneButtons[MyButtonNumber] = this;
	}

	void Start () {
	}

	void OnClick() {
		if (onClick != null) {
			onClick ();
		}
	}

	void OnHover(bool isOver) {
		if (isOver && Hover != null) {
			Hover ();
		} else if (!isOver && unHover != null) {
			unHover ();
		}
	}

	public void UnloadButtons () {
		myLabel.text = "";
		onClick = null;
		Hover = null;
		unHover = null;
	}

	public void SpawnedUnitButton () {
		UnitChoice.Instance.SpawnedUnit = UnitChoice.Instance.AllSpawnableUnits[MyButtonNumber].gameObject;
	}
}
