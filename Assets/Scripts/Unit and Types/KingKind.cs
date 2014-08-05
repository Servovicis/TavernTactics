using UnityEngine;
using System.Collections;

#pragma warning disable 0414 // variable is assigned but its value is never used.
public class KingKind : Unit {
	int initHealth = 100;
	int initDefenseChance = 1;
	int initDamage = 5;
	int initMovement = 0;
	int initMaxRange = 5;
	int initMinRange = 1;
	int initPopulationCost = 0;
	int initSummoningCost = 0;
	const bool initIsMobile = false;
	const bool initIsSelectable = true;
	const bool initIsSpawnable = false;
	const int initUnitMenuItems = 3;
	private Rect GUIGroupSize = new Rect(0, 0, 0, 0);
	private const int GUIButtonWidth = 185;
	private const int GUIButtonHeight = 29;
	private int MinStrikeRange = 1;
	private int MaxStrikeRange = 5;
	private int StrikeDamage = 8;
	private int MinUltraRange = 1;
	private int MaxUltraRange = 10;
	private int MinStrikeAttack;
	private int MaxStrikeAttack;
	private int MinUltraAttack;
	private int	MaxUltraAttack;
	private int UltraDamage = 15;
	public string UnitTypeNameOverride = "King";
	public string MyNameOverride;
	protected string Special1Name = "Spear/Sickle Thrust";
	protected string Special2Name = "Ultra Special";

	protected override void Awake(){
		myColumn = 6;

		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		initHealth = int.Parse(charStats[4,myColumn]);
		MinStrikeRange = int.Parse(charStats[14,myColumn]);
		MaxStrikeRange = int.Parse(charStats[15,myColumn]);
		initMovement = int.Parse(charStats[1, myColumn]);
		initDamage = int.Parse(charStats [2, myColumn]);
		initPopulationCost = int.Parse (charStats [9, myColumn]);
		initSummoningCost = int.Parse (charStats [10, myColumn]);
		MinUltraRange = int.Parse (charStats [16, myColumn]);
		MaxUltraRange = int.Parse (charStats [17, myColumn]);
		initMinRange = int.Parse (charStats [11, myColumn]);
		initMaxRange = int.Parse (charStats [12, myColumn]);
		initDefenseChance = int.Parse (charStats [3, myColumn]);
		MinStrikeAttack = int.Parse (charStats [5, myColumn]);
		MaxStrikeAttack = int.Parse (charStats [6, myColumn]);
		MinUltraAttack = int.Parse (charStats [7, myColumn]);
		MaxUltraAttack = int.Parse (charStats [8, myColumn]);
		BaseDamage = int.Parse (charStats [23, myColumn]);
		
		
		Debug.Log("King HP "+ initHealth);
		Debug.Log("King MinRipRange "+ MinStrikeRange);
		Debug.Log("King MaxRipRange "+ MaxStrikeRange);
		Debug.Log ("King Movement " + initMovement);
		Debug.Log ("King Damage " + initDamage);
		Debug.Log ("King Population Cost " + initPopulationCost);
		Debug.Log ("King Summoning Cost " + initSummoningCost);
		Debug.Log ("King MinBuffRange " + MinUltraRange);
		Debug.Log ("King MaxBuffRange " + MaxUltraRange);
		Debug.Log ("King MinRange " + initMinRange);
		Debug.Log ("King MaxRange " + initMaxRange);
		Debug.Log ("King Defense " + initDefenseChance);
		Debug.Log ("King MinRipAttack " + MinStrikeAttack);
		Debug.Log ("King MaxRipAttack " + MaxStrikeAttack);
		Debug.Log ("King MinBuffAttack " + MinUltraAttack);
		Debug.Log ("King MaxBuffAttack " + MaxUltraAttack);


		Health = initHealth;
		MaxHealth = initHealth;
		CritChance = initDamage;
		DefenseChance = initDefenseChance;
		Movement = initMovement;
		MaxRange = initMaxRange;
		MinRange = initMinRange;
		isHealable = false;
		isBuffable = false;
		summoningCost = initSummoningCost;
		populationCost = initPopulationCost;
		IsMobile = initIsMobile;
		IsSelectable = initIsSelectable;
		IsSpawnable = initIsSpawnable;
		UnitMenuItems = initUnitMenuItems;
		GUIGroupSize.height = GUIButtonHeight;
		GUIGroupSize.width = 1000;
		unitGUI = UnitGUI;
		UnitType = GridCS.UnitType.King;
		OnDeath = DeathAction;
		IsKing = true;
		OnActionSelect += InsertGUI;
		OnActionSelect += GUIButtons;
		OnActionDeselectExtra = RemoveGUI;
		OnAttack = UnitResolveAttack;
		//OnAttack = AOEAttack;
		base.Awake ();
	}

	protected override int declareMyColumn () {
		return 6;
	}

	public override void UnitTypeSet (){
		UnitTypeName = UnitTypeNameOverride;
		IsSpawnable = initIsSpawnable;
		MyName = MyNameOverride;
	}
	
	public void KingUnitGUI ()
	{
		if (!HasInteracted) {
		GUI.BeginGroup (GUIGroupSize);
		if (GUI.Button (new Rect  (175, 0, GUIButtonWidth, GUIButtonHeight), "Attack!")) 
		{
			RemoveAbilityRange ();
			CalculateAttackRange ();
			RemoveAbilityRange += RemoveAttackRange;
			//OnActionDeselect = RemoveMoveRange;
			//OnActionDeselect += RemoveAbilityRange;
		}
		if (GUI.Button (new Rect (400, 0, GUIButtonWidth, GUIButtonHeight), Special1Name)) 
			{
				OnSpecial = SpearThrust;
				RemoveAbilityRange ();
				SeeIfCanThrust ();
				RemoveAbilityRange += RemoveSeeIfCanThrust;
				OnSpecialSelect = SpecialSelection;
				OnSpecialSelect += spec1Anim;
				//OnActionDeselect = RemoveMoveRange;
				//OnActionDeselect += RemoveAbilityRange;
			}
		if (GUI.Button (new Rect (625, 0, GUIButtonWidth, GUIButtonHeight), Special2Name)) 
			{
				OnSpecial = UltraSpecial;
				RemoveAbilityRange ();
				SeeIfCanUltra ();
				RemoveAbilityRange += RemoveSeeIfCanUltra;
				OnSpecialSelect = SpecialSelection;
				OnSpecialSelect += spec2Anim;
				//OnActionDeselect = RemoveMoveRange;
				//OnActionDeselect += RemoveAbilityRange;
			}
			GUI.EndGroup ();
		}
	}

		public virtual void SpearThrust(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
			Debug.Log (TargetPosition + " Ouch!!!");
			HasInteracted = false;
			OnActionDeselect ();
		}
		public virtual void UltraSpecial(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
			Debug.Log (TargetPosition + "What a incredible hit!!!");
			HasInteracted = false;
			OnActionDeselect ();
		}
		public virtual void SeeIfCanThrust(){
			GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinStrikeRange,MaxStrikeRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);	}
		
		public virtual void RemoveSeeIfCanThrust(){
			GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxStrikeRange,layer);
		}
		
		public virtual void SeeIfCanUltra(){
			GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinUltraRange,MaxUltraRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);
		}
		
		public virtual void RemoveSeeIfCanUltra(){
			GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxUltraRange,layer);
		}
		public virtual void UndoUltraSelection (Vector2 TargetPosition, Vector2 InitiatorPosition){
			OnActionSelect ();
			HasInteracted = !HasInteracted;
			TurnInteract = new Vector2(0,0);
		}
		
		public override void InsertGUI(){
			GameManager.Instance.buttonsGUIFunction += KingUnitGUI;
		}
		
		public virtual void RemoveGUI(){
			GameManager.Instance.buttonsGUIFunction = null;
			OnActionSelect += InsertGUI;
			RemoveAbilityRange = RemoveAttackRange;
		}
	void DeathAction (){
		Application.LoadLevel ("WinScene");
	}
}