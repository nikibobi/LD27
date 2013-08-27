using UnityEngine;
using System;
using System.Collections;

public class Menu : MonoBehaviour {
	public enum MenuState {
		Main,
		Credits
	}
	
	public GUISkin SkinGUI;
	public MenuState State;
	
	void Start () {
		
	}
	
	void OnGUI() {
		GUI.skin = SkinGUI;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
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
	
	public static void MenuButton(string text, Color color, Action action = null) {
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
