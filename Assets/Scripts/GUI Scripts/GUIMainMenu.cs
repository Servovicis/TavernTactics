using UnityEngine;
using System;
using System.Collections;
using System.IO;
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0649 // variable field is never assigned to, and will always have its default value null.

public class GUIMainMenu : MonoBehaviour 
{
	public event System.Action MultiplayerButtonClicked;

	public GUISkin customSkin;
	public GUISkin backgroundBox;
	
	public AudioClip buttonHover;
	public AudioClip buttonPress;

	[SerializeField] private UIButton SinglePlayerButton;
	[SerializeField] private UIButton MultiplayerButton;
	[SerializeField] private UIButton OptionsButton;
	[SerializeField] private UIButton HelpButton;
	[SerializeField] private UIButton ExitButton;

	void Awake()
	{
		if (MultiplayerButton != null)
		{
			UIEventListener.Get (MultiplayerButton.gameObject).onClick += OnMultiplayerButtonClicked;
		}
		if (SinglePlayerButton != null)
		{
			UIEventListener.Get (SinglePlayerButton.gameObject).onClick += OnSinglePlayerButtonClicked;
		}
		if (OptionsButton !=null)
		{
			UIEventListener.Get (OptionsButton.gameObject).onClick += OnOptionsButtonClicked;
		}
		if (HelpButton !=null)
		{
			UIEventListener.Get (HelpButton.gameObject).onClick += OnHelpButtonClicked;
		}
		if (ExitButton !=null)
		{
			UIEventListener.Get (ExitButton.gameObject).onClick += OnExitButtonClicked;
		}
	}
	void OnDestroy ()
	{
		if (MultiplayerButton !=null)
		{
			UIEventListener.Get (MultiplayerButton.gameObject).onClick -= OnMultiplayerButtonClicked;
		}
		if (SinglePlayerButton !=null)
		{
			UIEventListener.Get (SinglePlayerButton.gameObject).onClick -= OnSinglePlayerButtonClicked;
		}
		if (OptionsButton !=null)
		{
			UIEventListener.Get (OptionsButton.gameObject).onClick -= OnOptionsButtonClicked;
		}
		if (HelpButton !=null)
		{
			UIEventListener.Get (HelpButton.gameObject).onClick -= OnHelpButtonClicked;
		}
		if (ExitButton !=null)
		{
			UIEventListener.Get (ExitButton.gameObject).onClick -= OnExitButtonClicked;
		}
	}
	
	
	/*void OnGUI () 
	
	{
		
		GUI.skin = backgroundBox;
		GUI.Box(new Rect (0, 0, Screen.width, Screen.height), "Genre Wars");

		GUI.skin = customSkin;
		
		GUI.BeginGroup(new Rect (Screen.width * 0.25f - 150, Screen.height * 0.5f - 250, 300, 500));
		
		// Main Menu Background Box

		
		// If Pressed, Load Single Player (Currently Disabled)
		if(GUI.Button(new Rect (75, 75, 160, 35), "Single Player")) 
		{
			//			Application.LoadLevel();
		}
		
		// If Pressed, Load Multiplayer  
		if(GUI.Button(new Rect (75, 165, 160, 35), "Multiplayer"))
		{
			//OnMultiplayerButtonPressed ();
		}
		
		// If Pressed, Load Options (Currently Disabled)
		if(GUI.Button(new Rect(75, 255, 160, 35), "Options")) 
		{
			//			Application.LoadLevel(3);
		}
		
		// If Pressed, Load Help (Currently Disabled)
		if(GUI.Button(new Rect(75, 345, 160, 35), "Help")) 
		{
			//			Application.LoadLevel(4);
		}
		
		// If Pressed, Quit game
		if(GUI.Button(new Rect(75, 435, 160, 35), "Exit")) 
		{
			Application.Quit();

		}
		
		GUI.EndGroup ();
	}*/

	void OnSinglePlayerButtonClicked (GameObject Button)
	{
		audio.clip = buttonPress;
		audio.Play ();
	}
	void OnMultiplayerButtonClicked (GameObject Button)
	{
		if (MultiplayerButtonClicked != null)
		{
			audio.clip = buttonPress;
			audio.Play ();
			MultiplayerButtonClicked();
		}
	}
	void OnOptionsButtonClicked (GameObject Button)
	{
		audio.clip = buttonPress;
		audio.Play ();
	}
	void OnHelpButtonClicked (GameObject Button)
	{
		audio.clip = buttonPress;
		audio.Play ();
//		Application.LoadLevel(4);
	}
	void OnExitButtonClicked (GameObject Button)
	{
		audio.clip = buttonPress;
		audio.Play ();
		Application.Quit();
	}
}