using UnityEngine;
using System.Collections;

public abstract class MeleeKind : MobileUnit {
	int initHealth = 10;
	int initDefenseChance = 1;
	int initDamage = 4;
	int initMovement = 3;
	int initMaxRange = 1;
	int initMinRange = 1;
	int initPopulationCost = 1;
	int initSummoningCost = 1;
	const bool initIsMobile = true;
	const bool initIsSelectable = true;
	const bool initIsSpawnable = true;
	const int initUnitMenuItems = 3;
	private int MinSmashRange = 1;
	private int MaxSmashRange = 2;
	//private int SmashDamage = 5;
	private int MinThrustRange = 1;
	private int MaxThrustRange = 5;
	private int MinSmashAttack;
	private int MaxSmashAttack;
	private int MinThrustAttack;
	private int MaxThrustAttack;
	//private int TrustDamage = 7;
	public bool HolyAnnilationAvailable = true;
	public string UnitTypeNameOverride = "Melee";
	public string MyNameOverride;
	int regDamage = 6;
	int WaittoAnnih = 0;

	protected override void Awake()
	{
		myColumn = 2;

		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		initHealth = int.Parse(charStats[4,myColumn]);
		MinSmashRange = int.Parse(charStats[14,myColumn]);
		MaxSmashRange = int.Parse(charStats[15,myColumn]);
		initMovement = int.Parse(charStats[1, myColumn]);
		initDamage = int.Parse(charStats [2, myColumn]);
		initPopulationCost = int.Parse (charStats [9, myColumn]);
		initSummoningCost = int.Parse (charStats [10, myColumn]);
		MinThrustRange = int.Parse (charStats [16, myColumn]);
		MaxThrustRange = int.Parse (charStats [17, myColumn]);
		initMinRange = int.Parse (charStats [11, myColumn]);
		initMaxRange = int.Parse (charStats [12, myColumn]);
		initDefenseChance = int.Parse (charStats [3, myColumn]);
		MinSmashAttack = int.Parse (charStats [5, myColumn]);
		MaxSmashAttack = int.Parse (charStats [6, myColumn]);
		MinThrustAttack = int.Parse (charStats [7, myColumn]);
		MaxThrustAttack = int.Parse (charStats [8, myColumn]);
		BaseDamage = int.Parse (charStats [23, myColumn]);
		
		
		
		
		Debug.Log("MeleeKind HP "+ initHealth);
		Debug.Log("MeleeKind MinRipRange "+ MinSmashRange);
		Debug.Log("MeleeKind MaxRipRange "+ MaxSmashRange);
		Debug.Log ("MeleeKind Movement " + initMovement);
		Debug.Log ("MeleeKind Damage " + initDamage);
		Debug.Log ("MeleeKind Population Cost " + initPopulationCost);
		Debug.Log ("MeleeKind Summoning Cost " + initSummoningCost);
		Debug.Log ("MeleeKind MinBuffRange " + MinThrustRange);
		Debug.Log ("MeleeKind MaxBuffRange " + MaxThrustRange);
		Debug.Log ("MeleeKind MinRange " + initMinRange);
		Debug.Log ("MeleeKind MaxRange " + initMaxRange);
		Debug.Log ("MeleeKind Defense " + initDefenseChance);
		Debug.Log ("MeleeKind MinRipAttack " + MinSmashAttack);
		Debug.Log ("MeleeKind MaxRipAttack " + MaxSmashAttack);
		Debug.Log ("MeleeKind MinBuffAttack " + MinThrustAttack);
		Debug.Log ("MeleeKind MaxBuffAttack " + MaxThrustAttack);
		Debug.Log ("MeleeKind BaseDamage " + BaseDamage);

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
		unitGUI = UnitGUI;
		UnitType = GridCS.UnitType.Melee;
		IsKing = false;
		//OnAttack = AOEAttack;
		OnDeath += meleeDeath;
		base.Awake ();
	}

	protected override int declareMyColumn () {
		return 2;
	}

	public override void UnitTypeSet (){
		UnitTypeName = UnitTypeNameOverride;
		IsSpawnable = initIsSpawnable;
		MyName = MyNameOverride;
	}

	public virtual void meleeDeath () {
		if (WaittoAnnih > 0) {
			GameManager.Instance.OnTurnBegin -= HolyAnnilationCountdown;
		}
	}

	#region GUI Buttons

	public override void SpecButton1 () {
		OnSpecial = PhysicalDeath;
		RemoveAbilityRange ();
		SeeIfCanSmash ();
		RemoveAbilityRange += RemoveSeeIfCanSmash;
		OnSpecialSelect = SpecialSelection;
		OnSpecialSelect += spec1Anim;
	}

	public override void SpecButton2 () {
		if ( WaittoAnnih == 0) {
			OnSpecial = HolyAnnilation;
			RemoveAbilityRange ();
			SeeIfCanThrustAttack ();
			RemoveAbilityRange += RemoveSeeIfCanThrustAttack;
			HolyAnnilationAvailable = false;
			OnSpecialSelect = SpecialSelection;
			OnSpecialSelect += spec2Anim;
		}
	}

	#endregion

	public virtual void PhysicalDeath(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " We got hit bad!!!");
		HasInteracted = false;
		OnActionDeselect ();
	}
	public virtual void HolyAnnilation(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " Man that hurt!!!");
		RipEffect (3, GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer));
		HasInteracted = false;
		WaittoAnnih = 4;
		GameManager.Instance.OnTurnBegin += HolyAnnilationCountdown;
		OnActionDeselect ();
	}
	
	public virtual void HolyAnnilationCountdown(){
		if (SwitchButton.Instance.CurrentPlayer == UnitOwner) {
			WaittoAnnih -= 1;
			Debug.Log (WaittoAnnih);
			if (WaittoAnnih <= 0){
				GameManager.Instance.OnTurnBegin -= HolyAnnilationCountdown;
				HolyAnnilationAvailable = true;
				WaittoAnnih = 0;
			}
		}
	}
	
	public virtual void RipEffect(int amount, Unit target){
		target.Health -= regDamage;
	}
	
	public virtual void SeeIfCanSmash(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinSmashRange,MaxSmashRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);
	}
	
	public virtual void RemoveSeeIfCanSmash(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxSmashRange,layer);
	}
	
	//public virtual void HolyAnnilAttack(Tile thisTile){
	//	GridCS.Instance.CalculateCircularRange (new Vector2 (thisTile.xcoord, thisTile.zcoord), Tile.OverlayType.SpecialAvailable, 0, MaxThrustRange, Owner, true, layer);
	//}
	
	public virtual void SeeIfCanThrustAttack(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinThrustRange,MaxThrustRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);
	}
	
	public virtual void RemoveSeeIfCanThrustAttack(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxThrustRange,layer);
	}
	public virtual void UndoThrustAttackSelection (Vector2 TargetPosition, Vector2 InitiatorPosition){
		OnActionSelect ();
		HasInteracted = !HasInteracted;
		TurnInteract = new Vector2(0,0);
	}
	
}