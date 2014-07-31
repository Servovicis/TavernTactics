using UnityEngine;
using System.Collections;

public class GUIPauseMenu : MonoBehaviour 
{
	public GUISkin customSkin;
	public GameObject background;
	
	public AudioClip buttonHover;
	public AudioClip buttonPress;

	private GameObject bg;

	private GUIOptionsMenu optionsMenu;
	private UnitChoice unitChoice;
	private SwitchButton switchButton;
	private GameManager gameManager;
	
	// Initialization
	void Awake () 
	{
		optionsMenu = gameObject.GetComponent<GUIOptionsMenu> (); //as GUIOptionsMenu;
		unitChoice = gameObject.GetComponent<UnitChoice> (); //as UnitChoice;
		switchButton = gameObject.GetComponent<SwitchButton> (); //as SwitchButton;
		gameManager = gameObject.GetComponent<GameManager> ();
	}

	void OnEnable ()
	{
		// Creates Overlay on Pause
		bg = Instantiate(background) as GameObject;
		bg.transform.parent = GameObject.FindGameObjectWithTag("MainCamera").transform;
		bg.transform.localPosition = bg.transform.position;
		bg.transform.localRotation = Quaternion.identity;
		Time.timeScale = 0f;
		gameManager._GoToPauseMenu ();
		//gameManager.guiFunction = GUIFunction;
	}

	void OnDisable ()
	{
		// Destroys Overlay
		Destroy(bg);

		// Unpause Game
		Time.timeScale = 1f;

		gameManager._ExitMenu();
	}

	public void GUIFunction() {
		GUI.skin = customSkin;

		GUI.BeginGroup(new Rect (Screen.width * 0.5f - 150, Screen.height * 0.5f - 125, 300, 250));
		
		// Pause Menu Background Box
		GUI.Box(new Rect (0, 0, 300, 500), "Game Paused");
		
		// If Pressed, Pause Menu disappears and Options Menu Opens
		if(GUI.Button(new Rect (75, 50, 150, 35), "Options")) 
		{
			audio.clip = buttonPress;
			audio.Play ();
			//optionsMenu.enabled = true;
			//this.enabled = false;
			gameManager._GoToOptionsMenu();
		}
		
		// If Pressed, Resume 
		if(GUI.Button(new Rect(75, 125, 150, 35), "Resume")) 
		{
			audio.clip = buttonPress;
			audio.Play ();
			//unitChoice.enabled = !unitChoice.enabled;
			//switchButton.enabled = !switchButton.enabled;
			// Disables Pause Menu
			this.enabled = false;
			gameManager._ExitMenu();
		}

		// If Pressed, Quit Game (In Application)
		if(GUI.Button(new Rect(75, 200, 150, 35), "Exit")) 
		{
			audio.clip = buttonPress;
			audio.Play ();
			Application.Quit();
		}
		
		GUI.EndGroup ();
	}
}