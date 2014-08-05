using UnityEngine;
using System.Collections;

public class MobileUnit : Unit {

	public GameManager.GenericFunction[] MyButtons;

	protected string Special1Name = "Special 1";
	protected string Special2Name = "Special 2";
	protected int[] HealthAtLevel = new int[] {4, 26, 27};
	protected int[] MovementAtLevel = new int[] {1, 28, 29};
	protected int[] DefenseAtLevel = new int [] {3, 30, 31};
	protected int[] DamageAtLevel = new int[] {2, 32, 33};

	protected override void Awake() {
		MyButtons = new GameManager.GenericFunction[4];
		MyButtons [0] = AttackButton;
		MyButtons [1] = DefendButton;
		MyButtons [2] = SpecButton1;
		MyButtons [3] = SpecButton2;
		GameManager.Instance.OnEndPhaseTransition += SummonSickness;
		base.Awake ();
		OnActionSelect += InsertGUI;
		OnActionDeselectExtra = RemoveGUI;
		OnAttack = UnitResolveAttack;
		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		DamageReduction = int.Parse (charStats [34, myColumn]);
	}

	public virtual void AttackButton () {
		RemoveAbilityRange ();
		CalculateAttackRange ();
		RemoveAbilityRange += RemoveAttackRange;
	}
	
	public virtual void DefendButton () {
		UnitHasSelectedDefend ();
	}
	
	public virtual void SpecButton1 () {
	}
	
	public virtual void SpecButton2 () {
	}

	public override void InsertGUI(){
		base.InsertGUI ();
		if (!HasInteracted){
			int thisButtonNum = 0;
			foreach (GUILeftPaneButton thisButton in GameManager.Instance.LeftPaneButtons) {
				thisButton.onClick = MyButtons [thisButtonNum];
				thisButtonNum++;
				NGUITools.SetActive(thisButton.gameObject, true);
			}
		}
		else {
			foreach (GUILeftPaneButton thisButton in GameManager.Instance.LeftPaneButtons) {
				thisButton.onClick = null;
				thisButton.myLabel.text = "";
				NGUITools.SetActive(thisButton.gameObject, false);
			}
		}
		GameManager.Instance.LeftPaneButtons [0].myLabel.text = "Attack";
		GameManager.Instance.LeftPaneButtons [1].myLabel.text = "Defend";
		GameManager.Instance.LeftPaneButtons [2].myLabel.text = Special1Name;
		GameManager.Instance.LeftPaneButtons [3].myLabel.text = Special2Name;
	}
	
	public virtual void RemoveGUI(){
		GameManager.Instance.buttonsGUIFunction = null;
		OnActionSelect += InsertGUI;
		RemoveAbilityRange = RemoveAttackRange;
	}

	public override bool HasInteracted {
		get { return _HasInteracted; }
		set {
			_HasInteracted = value;
			InsertGUI ();
		}
	}

	public void SummonSickness () {
		_HasInteracted = false;
		GameManager.Instance.OnEndPhaseTransition -= SummonSickness;
	}

	public override void LevelUp(){
		level++;
		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		MaxHealth = int.Parse(charStats[HealthAtLevel[level - 1],myColumn]);
		Health = MaxHealth;
		BaseDamage = int.Parse(charStats[DamageAtLevel[level - 1],myColumn]);
		DamageReduction = int.Parse(charStats[DefenseAtLevel[level - 1],myColumn]);
		Movement = int.Parse(charStats[MovementAtLevel[level - 1],myColumn]);
		Debug.Log (MaxHealth + ", " + BaseDamage + ", " + DamageReduction + ", " + Movement);
	}
}