using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0414 // variable is assigned but its value is never used.
public abstract class RangedKind : MobileUnit {
	int initHealth = 10;
	int initDefenseChance = 1;
	int initDamage = 5;
	int initMovement = 4;
	int initMaxRange = 7;
	int initMinRange = 3;
	int initPopulationCost = 1;
	int initSummoningCost = 1;
	const bool initIsMobile = true;
	const bool initIsSelectable = true;
	const bool initIsSpawnable = true;
	const int initUnitMenuItems = 3;
	private Rect GUIGroupSize = new Rect(0, 0, 0, 0);
	private const int GUIButtonWidth = 185;
	private const int GUIButtonHeight = 29;
	public string UnitTypeNameOverride = "Ranged";
	public string MyNameOverride;
	private int MinPowerRange = 1;
	private int MaxPowerRange = 10;
	private int MinPowerAttack;
	private int MaxPowerAttack;
	private int MinSnareAttack;
	private int MaxSnareAttack;
	private int PowerDamage = 8;
	private int MinSnareRange = 1;
	private int MaxSnareRange = 2;
	int SnareCounter = 0;
	float MyVelocity = 5F;
	public SnareTrap mySnare;
	bool SnareSelected = false;
	
	protected override void Awake()
	{
		myColumn = 3;

		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		initHealth = int.Parse(charStats[4,myColumn]);
		MinPowerRange = int.Parse(charStats[14,myColumn]);
		MaxPowerRange = int.Parse(charStats[15,myColumn]);
		initMovement = int.Parse(charStats[1, myColumn]);
		initDamage = int.Parse(charStats [2, myColumn]);
		initPopulationCost = int.Parse (charStats [9, myColumn]);
		initSummoningCost = int.Parse (charStats [10, myColumn]);
		MinSnareRange = int.Parse (charStats [16, myColumn]);
		MaxSnareRange = int.Parse (charStats [17, myColumn]);
		initMinRange = int.Parse (charStats [11, myColumn]);
		initMaxRange = int.Parse (charStats [12, myColumn]);
		initDefenseChance = int.Parse (charStats [3, myColumn]);
		MinPowerAttack = int.Parse (charStats [5, myColumn]);
		MaxPowerAttack = int.Parse (charStats [6, myColumn]);
		MinSnareAttack = int.Parse (charStats [7, myColumn]);
		MaxSnareAttack = int.Parse (charStats [8, myColumn]);
		BaseDamage = int.Parse (charStats [23, myColumn]);
		
		
		
		Debug.Log("RangedKind HP "+ initHealth);
		Debug.Log("RangedKind MinRipRange "+ MinPowerRange);
		Debug.Log("RangedKind MaxRipRange "+ MaxPowerRange);
		Debug.Log ("RangedKind Movement " + initMovement);
		Debug.Log ("RangedKind Damage " + initDamage);
		Debug.Log ("RangedKind Population Cost " + initPopulationCost);
		Debug.Log ("RangedKind Summoning Cost " + initSummoningCost);
		Debug.Log ("RangedKind MinBuffRange " + MinSnareRange);
		Debug.Log ("RangedKind MaxBuffRange " + MaxSnareRange);
		Debug.Log ("RangedKind MinRange " + initMinRange);
		Debug.Log ("RangedKind MaxRange " + initMaxRange);
		Debug.Log ("RangedKind Defense " + initDefenseChance);
		Debug.Log ("RangedKind MinRipAttack " + MinPowerAttack);
		Debug.Log ("RangedKind MaxRipAttack " + MaxPowerAttack);
		Debug.Log ("RangedKind MinBuffAttack " + MinSnareAttack);
		Debug.Log ("RangedKind MaxBuffAttack " + MaxSnareAttack);

		Health = initHealth;
		CritChance = initDamage;
		DefenseChance = initDefenseChance;
		Movement = initMovement;
		MaxRange = initMaxRange;
		MinRange = initMinRange;
		summoningCost = initSummoningCost;
		populationCost = initPopulationCost;
		IsMobile = initIsMobile;
		IsSelectable = initIsSelectable;
		IsSpawnable = initIsSpawnable;
		UnitMenuItems = initUnitMenuItems;
		GUIGroupSize.height = GUIButtonHeight;
		GUIGroupSize.width = 1000;
		unitGUI = UnitGUI;
		UnitType = GridCS.UnitType.Ranged;
		IsKing = false;
		OnActionDeselectExtra = RemoveGUI;
		OnAttack = UnitResolveAttack;
		//OnAttack = AOEAttack;
		base.Awake ();
	}

	protected override int declareMyColumn () {
		return 3;
	}
	
	public override void UnitTypeSet (){
		UnitTypeName = UnitTypeNameOverride;
		IsSpawnable = initIsSpawnable;
		MyName = MyNameOverride;
	}

	public override void SpecButton1 () {
		OnSpecial = PowerShot;
		RemoveAbilityRange ();
		SeeIfCanPowerShot ();
		RemoveAbilityRange += RemoveSeeIfCanPowerShot;
		OnSpecialSelect = SpecialSelection;
		OnSpecialSelect += spec1Anim;
		if (SnareSelected && IsMobile){
			CalculateMoveRange ();
			SnareSelected = false;
		}
	}
	
	public override void SpecButton2 () {
		OnSpecial = SetSnare;
		RemoveAbilityRange ();
		SeeIfCanSnare ();
		RemoveAbilityRange += RemoveSeeIfCanSnare;
		OnSpecialSelect = SnareSelection;
		OnSpecialSelect += spec2Anim;
		SnareSelected = true;
	}
	
	public virtual void PowerShot(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		HasInteracted = false;
		OnActionDeselect ();
	}
	
	public virtual void SetSnare(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		HasInteracted = false;
		mySnare = new SnareTrap (TargetPosition, TargetLayer, this);
		OnActionDeselect ();
	}
	
	public virtual void SeeIfCanPowerShot(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinPowerRange,MaxPowerRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);	}
	
	public virtual void RemoveSeeIfCanPowerShot(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxPowerRange,layer);
	}
	
	public virtual void SeeIfCanSnare(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinSnareRange,MaxSnareRange,SwitchButton.Instance.CurrentPlayer.player, false, false, true, layer);
	}
	
	public virtual void RemoveSeeIfCanSnare(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxSnareRange,layer);
	}
	
	public virtual void UndoSnareSelection (Vector2 TargetPosition, Vector2 InitiatorPosition){
		OnActionSelect ();
		HasInteracted = !HasInteracted;
		TurnInteract = new Vector2(0,0);
	}
	
	public override void CalculateAttackRange (){
		bool KeepGoing = true;
		LinkedListNode<Unit> thisNode = GameManager.Instance.AllUnits.First;
		while (KeepGoing) {
			if (thisNode != null){
				Unit targetUnit = thisNode.Value;
				if (targetUnit!= null){
					if (ActionHelper.IsInTrajectoryRange(this.transform, targetUnit.transform,MyVelocity, 45F) && targetUnit.UnitOwner != this.UnitOwner)
						GridCS.Instance.grid[(int)targetUnit.Position.x, (int)targetUnit.Position.y,(int)targetUnit.layer].TileSelectionType = Tile.OverlayType.AttackAvailable;
					thisNode = thisNode.Next;
				}
			}
			else{
				KeepGoing = false;
			}
		}
	}
	
	public override void RemoveAttackRange () {
		bool KeepGoing = true;
		LinkedListNode<Unit> thisNode = GameManager.Instance.AllUnits.First;
		while (KeepGoing) {
			if (thisNode != null){
				Unit targetUnit = thisNode.Value;
				if (targetUnit!= null){
					if (targetUnit.UnitOwner != this.UnitOwner)
						GridCS.Instance.grid[(int)targetUnit.Position.x, (int)targetUnit.Position.y,(int)targetUnit.layer].TileSelectionType = Tile.OverlayType.Unselected;
					thisNode = thisNode.Next;
				}
			}
			else{
				KeepGoing = false;
			}
		}
	}
	
	public virtual void SnareSelection (Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		print ("The Script Started");
		OnActionDeselect ();
		RemoveAbilityRange ();
		HasInteracted = !HasInteracted;
		TurnInteract = TargetPosition;
		TurnActionOrderHandler.Instance.actionList.AddLast (new PlaceItem(InitiatorPosition, TargetPosition,InitiatorLayer,TargetLayer));
	}
}