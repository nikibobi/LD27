using UnityEngine;
using System;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public enum MenuState {
		Main,
		Credits
	}
	
	public GUISkin SkinGUI;
	public MenuState State;
	
	private Rect screen;
	
	void Start () {
		screen = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	void OnGUI() {
		GUI.skin = SkinGUI;
		GUILayout.BeginArea(screen);
			GUILayout.BeginVertical();
			switch(State) {
				case MenuState.Main:
					GUILayout.FlexibleSpace();
					MenuButton("Play", Settings.Palete.Green, () => Application.LoadLevel("Game"));
					MenuButton("Credits", Settings.Palete.Blue, () => State = MenuState.Credits);
					MenuButton("Quit", Settings.Palete.Red, Application.Quit);
					GUILayout.FlexibleSpace();
					break;
				case MenuState.Credits:
					GUILayout.FlexibleSpace();
					MenuButton("Borislav Kosharov", Settings.Palete.Yellow, () => Application.OpenURL("https://github.com/nikibobi"));
					MenuButton("Viktor Danev", Settings.Palete.Cyan, () => Application.OpenURL("https://github.com/Metaknigth"));
					MenuButton("Back", Settings.Palete.Magenta, () => State = MenuState.Main);
					GUILayout.FlexibleSpace();
					break;
			}
			GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	
	private void MenuButton(string text, Color color, Action action = null) {
		Color temp = GUI.contentColor;
		GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.contentColor = color;
			if(GUILayout.Button(text) && action != null)
				action();
		GUILayout.EndHorizontal();
		GUI.contentColor = temp;
	}
}
