using UnityEngine;
using System;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public enum MenuState {
		Main,
		Credits,
		Play
	}
	
	public GUISkin Skin;
	public MenuState State;
	
	private Rect screen;
	
	void Start () {
		screen = new Rect(0, 0, Screen.width, Screen.height);
	}
	
	void OnGUI() {
		GUI.skin = Skin;
		GUILayout.BeginArea(screen);
			GUILayout.BeginVertical();
			switch(State) {
				case MenuState.Main:
					GUILayout.FlexibleSpace();
					MenuButton("Play", Color.green, () => State = MenuState.Play);
					MenuButton("Credits", Color.blue, () => State = MenuState.Credits);
					MenuButton("Quit", Color.red, Application.Quit);
					GUILayout.FlexibleSpace();
					break;
				case MenuState.Credits:
					GUILayout.FlexibleSpace();
					MenuButton("Borislav Kosharov", Color.yellow);
					MenuButton("Viktor Danev", Color.cyan);
					MenuButton("Back", Color.magenta, () => State = MenuState.Main);
					GUILayout.FlexibleSpace();
					break;
				case MenuState.Play:
					GUILayout.FlexibleSpace();
					MenuButton("Random", Color.green, () => Application.LoadLevel("Game"));
					MenuButton("Back", Color.magenta, () => State = MenuState.Main);
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
