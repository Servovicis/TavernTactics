using UnityEngine;
using System.Collections;

public class WerewolfKind : MeleeKind {
	
	// Use this for initialization
	protected override void Awake(){
		Special1Name = "Death Strike!";
		spec1AnimName = "deathstrikel";
		Special2Name = "Annihilation!";
		spec2AnimName = "annihilation";
		base.Awake ();
	}

	public override void UnitTypeSet (){
		MyNameOverride = "Werewolf";
		base.UnitTypeSet ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
