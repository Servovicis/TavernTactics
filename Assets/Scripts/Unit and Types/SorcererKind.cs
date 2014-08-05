using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SorcererKind : MobileUnit {
	int initHealth = 10;
	int initDefenseChance = 1;
	int initDamage = 6;
	int initMovement = 2;
	int initMaxRange = 15;
	int initMinRange = 10;
	int initPopulationCost;
	int initSummoningCost;
	const bool initIsMobile = true;
	const bool initIsSelectable = true;
	const bool initIsSpawnable = true;
	const int initUnitMenuItems = 3;
	private Rect GUIGroupSize = new Rect(0, 0, 0, 0);
	private const int GUIButtonWidth = 185;
	private const int GUIButtonHeight = 29;
	private int MinSpellRange = 1;
	private int MaxSpellRange = 20;
	private int MinHealRange;
	private int MinSpellAttack;
	private int MaxSpellAttack;
	private int SpellDamage = 6;
	private int MaxHealRange = 2;
	private int MinHeal;
	private int MaxHeal;
	public string UnitTypeNameOverride = "Sorcerer";
	public string MyNameOverride;
	float MyVelocity = 8.75F;
	int WaittoHeal = 0;

	protected override void Awake()
	{
		myColumn = 5;
		TextAsset csv = Resources.Load("CharacterBalance") as TextAsset;
		string [,] charStats = LoadCSV.SplitCsvGrid(csv.text);
		initHealth = int.Parse(charStats[4,myColumn]);
		MinSpellRange = int.Parse(charStats[14,myColumn]);
		MaxSpellRange = int.Parse(charStats[15,myColumn]);
		initMovement = int.Parse(charStats[1, myColumn]);
		initDamage = int.Parse(charStats [2, myColumn]);
		initPopulationCost = int.Parse (charStats [9, myColumn]);
		initSummoningCost = int.Parse (charStats [10, myColumn]);
		MinHealRange = int.Parse (charStats [16, myColumn]);
		MaxHealRange = int.Parse (charStats [17, myColumn]);
		initMinRange = int.Parse (charStats [11, myColumn]);
		initMaxRange = int.Parse (charStats [12, myColumn]);
		initDefenseChance = int.Parse (charStats [3, myColumn]);
		MinSpellAttack = int.Parse (charStats [5, myColumn]);
		MaxSpellAttack = int.Parse (charStats [6, myColumn]);
		MinHeal = int.Parse (charStats [7, myColumn]);
		MaxHeal = int.Parse (charStats [8, myColumn]);
		BaseDamage = int.Parse (charStats [23, myColumn]);
		
		
		Debug.Log("Sorcerer HP "+ initHealth);
		Debug.Log("Sorcerer MinRipRange "+ MinSpellRange);
		Debug.Log("Sorcerer MaxRipRange "+ MaxSpellRange);
		Debug.Log ("Sorcerer Movement " + initMovement);
		Debug.Log ("Sorcerer Damage " + initDamage);
		Debug.Log ("Sorcerer Population Cost " + initPopulationCost);
		Debug.Log ("Sorcerer Summoning Cost " + initSummoningCost);
		Debug.Log ("Sorcerer MinBuffRange " + MinHealRange);
		Debug.Log ("Sorcerer MaxBuffRange " + MaxHealRange);
		Debug.Log ("Sorcerer MinRange " + initMinRange);
		Debug.Log ("Sorcerer MaxRange " + initMaxRange);
		Debug.Log ("Sorcerer Defense " + initDefenseChance);
		Debug.Log ("Sorcerer MinRipAttack " + MinSpellAttack);
		Debug.Log ("Sorcerer MaxRipAttack " + MaxSpellAttack);
		Debug.Log ("Sorcerer MinBuffAttack " + MinHeal);
		Debug.Log ("Sorcerer MaxBuffAttack " + MaxHeal);

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
		UnitType = GridCS.UnitType.Sorcerer;
		IsKing = false;
		OnActionDeselectExtra = RemoveGUI;
		OnAttack = UnitResolveAttack;
		//OnAttack = AOEAttack;
		OnDeath += sorcererDeath;
		base.Awake ();
	}
	
	public override void UnitTypeSet (){
		UnitTypeName = UnitTypeNameOverride;
		IsSpawnable = initIsSpawnable;
		MyName = MyNameOverride;
	}
	
	public virtual void sorcererDeath () {
		if (WaittoHeal > 0) {
			GameManager.Instance.OnTurnBegin -= HealCountdown;
		}
	}

	public override void SpecButton1 () {
		OnSpecial = SpellAttack;
		RemoveAbilityRange ();
		SeeIfCanSpell ();
		RemoveAbilityRange += RemoveSeeIfCanSpell;
		OnSpecialSelect = SpecialSelection;
		OnSpecialSelect += spec1Anim;
	}
	
	public override void SpecButton2 () {
		if (WaittoHeal == 0){
			OnSpecial = GiveThemHealth;
			RemoveAbilityRange ();
			SeeIfCanHeal ();
			RemoveAbilityRange += RemoveSeeIfCanHeal;
			OnSpecialSelect = SpecialSelection;
			OnSpecialSelect += spec2Anim;
		}
		else
			Debug.Log ("Turn until Heal = " + WaittoHeal);
	}
	
	public virtual void SpellAttack(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " Big hit!!!");
		if (GridCS.Instance.GetUnitFromGrid(TargetPosition, TargetLayer) != null) {
			Unit TargetUnit = GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer);
		if (!TargetUnit.IsKing || (TargetUnit.IsKing && TargetUnit.UnitOwner.WallIsDestroyed)){
			int thisAttackDamage = 1;
			for (int rolls = 0; rolls < level; rolls++){
				System.Random random = new System.Random();
				int randomNumber = random.Next(1,7);
				if(randomNumber >= SpellDamage){
					thisAttackDamage += 1;
					print ("Attack dealt extra damage!");
					}
					else if (randomNumber <= TargetUnit.DefenseChance){
						thisAttackDamage -= TargetUnit.DamageReduction;
						print ("Defender prevented 1 damage!");
					}
				}
				if (thisAttackDamage > 0)
					TargetUnit.Health -= thisAttackDamage;
				if (TargetUnit.OnHit != null)
					TargetUnit.OnHit (TargetPosition, InitiatorPosition,TargetLayer,InitiatorLayer);
			}
			else print ("Deflected!");
			if (TargetUnit.IsMobile) {
				double h = ActionHelper.CalculateTwoDiminsionalDistance(InitiatorPosition, TargetPosition);
				int a = (int) (TargetPosition.x - InitiatorPosition.x);
				int o = (int) (TargetPosition.y - InitiatorPosition.y);
				double sin = o/h * 1f;
				double cos = a/h * 1f;
				h += 1;
				int nO = (int) Mathf.Round((float)(sin * h));
				Debug.Log (nO);
				int nA = (int) Mathf.Round((float)(cos * h));
				Debug.Log (nA);
				Vector2 nextPosition = new Vector2 (nA + InitiatorPosition.x, nO + InitiatorPosition.y);
				Debug.Log (nextPosition);
				if (nextPosition.x < GridCS.GRIDSIZEX && nextPosition.x >= 0 && nextPosition.y < GridCS.GRIDSIZEZ && nextPosition.y >= 0
				    && GridCS.Instance.GetTile(nextPosition,TargetLayer) != null && GridCS.Instance.GetUnitFromGrid(TargetPosition,TargetLayer) == null){
					GridCS.Instance.PlaceUnitInGrid (TargetUnit, TargetPosition, TargetLayer, nextPosition, TargetLayer);
					Move MoveToSpace = new Move (TargetPosition,nextPosition,TargetLayer,TargetLayer);
					TargetUnit.Position = nextPosition;
					MoveToSpace.Resolve ();
				}
			}
		}
		HasInteracted = false;
		OnActionDeselect ();
	}
	
	public virtual void GiveThemHealth(Vector2 TargetPosition, Vector2 InitiatorPosition, int TargetLayer, int InitiatorLayer){
		Debug.Log (TargetPosition + " Were healded up!!!");
		double dist = ActionHelper.CalculateTwoDiminsionalDistance (InitiatorPosition, TargetPosition);
		if (dist > 1) {
			HealEffect (0.5f, GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer));
		}
		else{
			HealEffect (1.0f, GridCS.Instance.GetUnitFromGrid (TargetPosition, TargetLayer));
		}
		WaittoHeal = 2;
		GameManager.Instance.OnTurnBegin += this.HealCountdown;
		HasInteracted = false;
		OnActionDeselect ();
	}
	
	public virtual void HealCountdown(){
		if (SwitchButton.Instance.CurrentPlayer = UnitOwner) {
			WaittoHeal -= 1;
			Debug.Log (WaittoHeal);
			if (WaittoHeal <= 0)
				GameManager.Instance.OnTurnBegin -= HealCountdown;
		}
	}
	
	public virtual void SeeIfCanSpell(){
		GridCS.Instance.CalculateCircularRange(Position,Tile.OverlayType.SpecialAvailable,MinSpellRange,MaxSpellRange,SwitchButton.Instance.CurrentPlayer.player, true, false, false, layer);	}
	
	public virtual void RemoveSeeIfCanSpell(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.SpecialAvailable, MaxSpellRange,layer);
	}
	
	public virtual void SeeIfCanHeal(){
		StepHealDetect (GridCS.Instance.GetTile(Position,layer), MaxHealRange - 1);
	}
	
	public virtual void StepHealDetect(Tile thisTile, int Steps){
		Tile nextTile = GridCS.Instance.GetTile (new Vector2 (thisTile.xcoord + 1, thisTile.zcoord), (int) thisTile.layerNumber.x);
		if (nextTile.LoadedUnitScript != null) {
			if(nextTile.LoadedUnitScript.UnitOwner == UnitOwner && nextTile.LoadedUnitScript != this && nextTile.LoadedUnitScript.isHealable)
				nextTile.TileSelectionType = Tile.OverlayType.BuffAvailable;
		}
		if (Steps > 0)
			StepHealDetect (nextTile, Steps - 1);
		nextTile = GridCS.Instance.GetTile (new Vector2 (thisTile.xcoord - 1, thisTile.zcoord), (int)thisTile.layerNumber.x);
		if (nextTile.LoadedUnitScript != null) {
			if(nextTile.LoadedUnitScript.UnitOwner == UnitOwner && nextTile.LoadedUnitScript != this && nextTile.LoadedUnitScript.isHealable)
				nextTile.TileSelectionType = Tile.OverlayType.BuffAvailable;
		}		if (Steps > 0)
			StepHealDetect (nextTile, Steps - 1);
		nextTile = GridCS.Instance.GetTile (new Vector2 (thisTile.xcoord, thisTile.zcoord + 1),(int) thisTile.layerNumber.x);
		if (nextTile.LoadedUnitScript != null) {
			if(nextTile.LoadedUnitScript.UnitOwner == UnitOwner && nextTile.LoadedUnitScript != this && nextTile.LoadedUnitScript.isHealable)
				nextTile.TileSelectionType = Tile.OverlayType.BuffAvailable;
		}		if (Steps > 0)
			StepHealDetect (nextTile, Steps - 1);
		nextTile = GridCS.Instance.GetTile (new Vector2 (thisTile.xcoord, thisTile.zcoord - 1),(int) thisTile.layerNumber.x);
		if (nextTile.LoadedUnitScript != null) {
			if(nextTile.LoadedUnitScript.UnitOwner == UnitOwner && nextTile.LoadedUnitScript != this && nextTile.LoadedUnitScript.isHealable)
				nextTile.TileSelectionType = Tile.OverlayType.BuffAvailable;
		}		if (Steps > 0)
			StepHealDetect (nextTile, Steps - 1);
	}
	
	public void RemoveSeeIfCanHeal(){
		GridCS.Instance.EraseRange (Position, Tile.OverlayType.BuffAvailable, MaxHealRange,layer);
	}
	
	public void HealEffect(float amount, Unit target){
		int HealthAmount = target.Health + (int) (target.MaxHealth * amount);
		if (HealthAmount > target.MaxHealth)
			target.Health = target.MaxHealth;
		else
			target.Health = HealthAmount;
	}
	
	public virtual void UndoHealSelection (Vector2 TargetPosition, Vector2 InitiatorPosition){
		OnActionSelect ();
		HasInteracted = !HasInteracted;
		TurnInteract = new Vector2(0,0);
	}
	
	public override void CalculateAttackRange (){
		bool KeepGoing = true;
		LinkedListNode<Unit> thisNode = GameManager.Instance.AllUnits.First;
		//foreach (Tile targetUnit in GridCS.Instance.grid){
			//if (targetUnit != null){
				//if (targetUnit.xcoord == Position.x || targetUnit.zcoord == Position.y){
					//if (ActionHelper.IsInTrajectoryRange(this.transform, targetUnit.transform,MyVelocity, 0,0))
						//GridCS.Instance.grid[(int)targetUnit.xcoord, (int)targetUnit.zcoord,(int)targetUnit.layerNumber.x].TileSelectionType = Tile.OverlayType.AttackAvailable;
				//}
			//}
		//}
		while (KeepGoing) {
			if (thisNode != null){
				Unit targetUnit = thisNode.Value;
				if (targetUnit!= null){
					if (ActionHelper.IsInTrajectoryRange(this.transform, targetUnit.transform,MyVelocity, 30F) && targetUnit.UnitOwner != this.UnitOwner)
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
	
	public static void InstantiateHeightMarker(float x, float y, float z){
		Instantiate (GameManager.Instance.HeightMarker, new Vector3 (x, y, z), Quaternion.identity);
	}
}