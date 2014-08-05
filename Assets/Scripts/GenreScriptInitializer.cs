using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GenreScriptInitializer{

	public static char TextSeparator = ',';

	public static void LoadItemsIntoGenre (PrefabGenreScript genreScript) {
		string[] UnitScripts = Resources.Load <TextAsset> (genreScript.genreName).text.Split (new char[]{TextSeparator},System.StringSplitOptions.RemoveEmptyEntries);
		foreach (string UnitName in UnitScripts) {
			GameObject UnitObject = Resources.Load("Genre/" + genreScript.genreName+"/Units/"+UnitName) as GameObject;
			Unit UnitScript = UnitObject.GetComponent<Unit>();
			GameObject UnitPortrait = Resources.Load ("Genre/" + genreScript.genreName + "/UnitPortraits/" + UnitName) as GameObject;
			UnitScript.UnitTypeSet ();
			UnitScript.myPortrait = UnitPortrait;
			genreScript.UnitTypes.Add (UnitScript.UnitTypeName);
			genreScript.UnitsList.Add (UnitObject);
		}
		AlphabetizeItems (genreScript);
	}

	public static void AlphabetizeItems (PrefabGenreScript genreScript) {
		genreScript.UnitTypes.Sort ();
		int counter = 0;
		GameObject[] Units = new GameObject[genreScript.UnitTypes.Count];
		foreach (GameObject UnitObject in genreScript.UnitsList) {
			foreach(string UnitType in genreScript.UnitTypes){
				Unit UnitScript = UnitObject.GetComponent<Unit>();
				if (UnitScript.UnitTypeName == UnitType){
					Units[counter] = UnitObject;
					counter = 0;
					break;
				}
				else counter++;
			}
		}
		counter = 0;
		foreach (GameObject UnitObject in Units) {
			genreScript.UnitsList[counter] = UnitObject;
			counter++;
		}
	}
}
