using UnityEngine;
using System.Collections;
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
public abstract class HeavyKind : MobileUnit {
	int initHealth = 1;
	int initDefenseChance = 2;
	int initDamage = 5;
	int initMovement = 2;
	int initMaxRange = 2;
	int initMinRange = 1;
	int initPopulationCost = 4;
	int initSummoningCost = 3;
	bool initIsMobile = true;
	bool initIsSelectable = true;
	bool initIsSpawnable = true;
	int initUnitMenuItems = 3;
	private Rect GUIGroupSize = new Rect(0, 0, 0, 0);
	private const int GUIButtonWidth = 185;
	private const int GUIButtonHeight = 29;
	public string UnitTypeNameOverride = "Heavy";
	public string MyNameOverride;
	private int MinRipRange = 1;
	private int MaxRipRange = 2;
	private int MinBuffRange = 1;
	private int MaxBuffRange = 2;
	private int MaxBuffAttack;
	private int MinBuffAttack;
	private int MinRipAttack;
	private int MaxRipAttack;
	public bool ripCrushAvailable = true;
	public bool buffAvailable = true;
	int RegDamage = 5;
	int WaittoBuff = 0;
	int WaittoRip = 0;
			
	protected override void Awake()
	{
		myColumn = 4;
		base.Awake ();

		//LoadCSV csvLoad = new LoadCSV();
		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		initHealth = int.Parse(charStats[4,myColumn]);
		MinRipRange = int.Parse(charStats[14,myColumn]);
		MaxRipRange = int.Parse(charStats[15,myColumn]);
		initMovement = int.Parse(charStats[1, myColumn]);
		initDamage = int.Parse(charStats [2, myColumn]);
		initPopulationCost = int.Parse (charStats [9, myColumn]);
		initSummoningCost = int.Parse (charStats [10, myColumn]);
		MinBuffRange = int.Parse (charStats [16, myColumn]);
		MaxBuffRange = int.Parse (charStats [17, myColumn]);
		initMinRange = int.Parse (charStats [11, myColumn]);
		initMaxRange = int.Parse (charStats [12, myColumn]);
		initDefenseChance = int.Parse (charStats [3, myColumn]);
		MinRipAttack = int.Parse (charStats [5, myColumn]);
		MaxRipAttack = int.Parse (charStats [6, myColumn]);
		MinBuffAttack = int.Parse (charStats [7, myColumn]);
		MaxBuffAttack = int.Parse (charStats [8, myColumn]);
		BaseDamage = int.Parse (charStats [23, myColumn]);
		int RegDamage = 5;
		int WaittoBuff = 0;
		int WaittoRip = 0;
		
		Debug.Log("HeavyKind HP "+ initHealth);
		Debug.Log("HeavyKind MinRipRange "+ MinRipRange);
		Debug.Log("HeavyKind MaxRipRange "+ MaxRipRange);
		Debug.Log ("HeavyKind Movement " + initMovement);
		Debug.Log ("HeavyKind Damage " + initDamage);
		Debug.Log ("HeavyKind Population Cost " + initPopulationCost);
		Debug.Log ("HeavyKind Summoning Cost " + initSummoningCost);
		Debug.Log ("HeavyKind MinBuffRange " + MinBuffRange);
		Debug.Log ("HeavyKind MaxBuffRange " + MaxBuffRange);
		Debug.Log ("HeavyKind MinRange " + initMinRange);
		Debug.Log ("HeavyKind MaxRange " + initMaxRange);
		Debug.Log ("HeavyKind Defense " + initDefenseChance);
		Debug.Log ("HeavyKind MinRipAttack " + MinRipAttack);
		Debug.Log ("HeavyKind MaxRipAttack " + MaxRipAttack);
		Debug.Log ("HeavyKind MinBuffAttack " + MinBuffAttack);
		Debug.Log ("HeavyKind MaxBuffAttack " + MaxBuffAttack);


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
		UnitType = GridCS.UnitType.Heavy;
		IsKing = false;
		OnActionDeselectExtra = RemoveGUI;
		OnAttack = UnitResolveAttack;
		OnDeath += heavyDeath;
	}

	protected override int declareMyColumn () {
		return 4;
	}

	public override void UnitTypeSet (){
		UnitTypeName = UnitTypeNameOverride;
		IsSpawnable = initIsSpawnable;
		MyName = MyNameOverride;
	}

	public virtual void heavyDeath () {
		if (WaittoBuff > 0) {
			GameManager.Instance.OnTurnBegin -= BuffCountdown;
		}
		if (WaittoRip > 0) {
			GameManager.Instance.OnTurnBegin -= RipCrushCountdown;
		}
	}
	public override void SpecButton1 () {
		if (WaittoRip == 0) {
			OnSpecial = RipCrushThem;
			RemoveAbilityRange ();
			SeeIfCanRipCrush ();
			RemoveAbilityRange += RemoveSeeIfCanRipCrush;
			ripCrushAvailable = false;
			OnSpecialSelect = SpecialSelection;
			OnSpecialSelect += spec1Anim;
		}
	}
	
	public override void SpecButton2 () {
		if (WaittoBuff == 0) {
			OnSpecial = GiveThemBuff;
			RemoveAbilityRange ();
			SeeIfCanBuff ();
			RemoveAbilityRange += RemoveSeeIfCanBuff;
			buffAvailable = false;
			OnSpecialSelect = SpecialSelection;
			OnSpecialSelect += spec2Anim;
		}
	}

	public virtual void RipCrushThem(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " We're Crushed, man!!");
		double dist = ActionHelper.CalculateTwoDiminsionalDistance (InitiatorPosition, TargetPosition);
		RipEffect (3, GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer));
		WaittoRip = 4;
		HasInteracted = false;
		OnActionDeselect ();
		GameManager.Instance.OnTurnBegin += RipCrushCountdown;
	}
	
	public virtual void GiveThemBuff(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " Buffed up!!!");
		double dist = ActionHelper.CalculateTwoDiminsionalDistance (InitiatorPosition, TargetPosition);
		BuffEffect (3, GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer));
		WaittoBuff = 3;
		HasInteracted = false;
		OnActionDeselect ();
		GameManager.Instance.OnTurnBegin += BuffCountdown;
		
	}
	
	public virtual void RipEffect(int amount, Unit target){
		target.Health -= RegDamage;
	}
			
	public virtual void RipCrushCountdown(){
		if (SwitchButton.Instance.CurrentPlayer == UnitOwner) {
			WaittoRip -= 1;
			Debug.Log (WaittoRip);
			if (WaittoRip <= 0){
				GameManager.Instance.OnTurnBegin -= RipCrushCountdown;
				ripCrushAvailable = true;
				WaittoRip = 0;
			}
		}
	}
		
	//public virtual void RipCrushAttack (Tile thisTile){
	//	GridCS.Instance.CalculateCircularRange (Position, Tile.OverlayType.SpecialAvailable, MinRipRange, MaxRipRange, SwitchButton.Instance.CurrentPlayer.player, true, layer);
	//}
		
	public virtual void SeeIfCanRipCrush(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinRipRange,MaxRipRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);
	}

	public virtual void BuffCountdown(){
		if (SwitchButton.Instance.CurrentPlayer == UnitOwner) {
			WaittoBuff -= 1;
			Debug.Log (WaittoBuff);
			if (WaittoBuff <= 0) {
				GameManager.Instance.OnTurnBegin -= BuffCountdown;
				buffAvailable = true;
			}
		}
	}
		
	public virtual void RemoveSeeIfCanRipCrush(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxRipRange,layer);
	}

	public virtual void SeeIfCanBuff(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.BuffAvailable,MinBuffRange,MaxBuffRange,SwitchButton.Instance.CurrentPlayer.player, false, true, false, layer);
	}
		
	//public virtual void PowerBuffUp(Tile thisTile, int Steps){
	//	GridCS.Instance.CalculateCircularRange (Position, Tile.OverlayType.BuffAvailable, MinBuffRange, MaxBuffRange, SwitchButton.Instance.CurrentPlayer.player, false, layer);
	//}
		
	public virtual void RemoveSeeIfCanBuff(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.BuffAvailable, MaxBuffRange,layer);
	}
		
	public void BuffEffect(int amount, Unit target){
		int BuffAmount = target.BaseDamage + amount;
		if (BuffAmount > target.MaxBuff) 
			target.BaseDamage = target.MaxBuff;
		else
			target.BaseDamage = BuffAmount;
	}
		
	public virtual void UndoHealSelection (Vector2 TargetPosition, Vector2 InitiatorPosition){
		OnActionSelect ();
		HasInteracted = !HasInteracted;
		TurnInteract = new Vector2(0,0);
	}
		
}