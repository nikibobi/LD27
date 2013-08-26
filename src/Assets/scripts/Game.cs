using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	public GUISkin SkinGUI;
	public GameObject[] SiblingPrefabs;
	public GameObject[] EnemyPerfabs;
	
	private Transform tfm;
	private Vector3 initialPosition;
	private List<Enemy> enemies;
	private LinkedList<Sibling> siblings;
	private LinkedListNode<Sibling> current;
	private bool recording;
	
	public LinkedListNode<Sibling> Current {
		get {
			return current;
		}
		private set {
			if(Recording) {
				if(current != null) {
					current.Value.State = Sibling.MoveState.Paused;
				}
				value.Value.State = Sibling.MoveState.Recording;
			}
			current = value;
			CurrentSibling = current.Value;
		}
	}
	public bool Recording { 
		get {
			return recording;	
		}
		private set {
			recording = value;
			RecordingState = recording;
			Seconds = 0;
			foreach(var sibling in siblings) {
				sibling.State = (recording?Sibling.MoveState.Paused:Sibling.MoveState.Playing);
			}
		}
	}
	
	public static Sibling CurrentSibling { get; private set; }
	public static bool RecordingState { get; private set; }
	public static float Seconds { get; private set; }

	void Start() {
		enemies = new List<Enemy>();
		siblings = new LinkedList<Sibling>();
		tfm = transform;
		initialPosition = tfm.position;
		Restart();
	}
	
	void Restart() {
		Recording = true;
		
		enemies.Clear();
		foreach(var prefab in EnemyPerfabs) {
			var enemy = prefab.GetComponent<Enemy>();
			enemy.Weapon = (WeaponType)UnityEngine.Random.Range(0, 3);
			enemies.Add(enemy);
		}
		
		siblings.Clear();
		foreach(var prefab in SiblingPrefabs) {
			var sibling = prefab.GetComponent<Sibling>();
			sibling.Weapon = (WeaponType)UnityEngine.Random.Range(0, 3);
			siblings.AddLast(sibling);
		}
		Current = siblings.First;
	}
	
	void Update() {
		
		if(Input.GetKeyDown(Settings.Keymap.NextSibling))
			Current = Current.NextOrFirst();
		if(Input.GetKeyDown(Settings.Keymap.LastSibling))
			Current = Current.PreviousOrLast();
		
		if(Input.GetKeyDown(Settings.Keymap.SwapStates))
			Recording = !Recording;
		
		if(Recording) {
			if(Input.GetKeyDown(Settings.Keymap.Restart))
				Current.Value.Restart();
		} else {
			Seconds += Time.deltaTime;
			
			if(Seconds >= 10) {
				Seconds = 10;
				if(Input.GetKeyDown(Settings.Keymap.Restart))
					this.Restart();
				//go to next wave?
			}
		}
		
		tfm.position = new Vector3(Current.Value.transform.position.x, Current.Value.transform.position.y)  + initialPosition;
	}
	
	void OnGUI() {
		//draw gui stuff here depending on state
		GUI.skin = SkinGUI;
		GUILayout.BeginVertical();
		GUILayout.Label((Recording?Current.Value.Seconds:Seconds).ToString("G3"));
		GUILayout.Label(Recording?"R":"P");
		GUILayout.EndVertical();
	}
}
