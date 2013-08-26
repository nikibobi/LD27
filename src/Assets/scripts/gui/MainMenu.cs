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
					break;
				case MenuState.Credits:
					GUILayout.FlexibleSpace();
					MenuButton("Borislav Kosharov", Settings.Palete.Yellow, () => Application.OpenURL("http://bosakkoshi.tumblr.com/"));
					MenuButton("Viktor Danev", Settings.Palete.Magenta, () => Application.OpenURL("http://metaknigth.tumblr.com/"));
					MenuButton("Nikolai Kosharov", Settings.Palete.Cyan, () => Application.OpenURL("http://medik3.tumblr.com/"));
					MenuButton("Back", Settings.Palete.Red, () => State = MenuState.Main);
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
