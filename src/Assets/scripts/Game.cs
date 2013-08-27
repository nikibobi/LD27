using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	public GUISkin SkinGUI;
	public GameObject[] SiblingPrefabs;
	public GameObject[] EnemyPerfabs;
	
	public const float ATTACK_RANGE = 300f;
	
	private Transform tfm;
	private Vector3 initialPosition;
	private List<Enemy> enemies;
	private LinkedList<Sibling> siblings;
	private LinkedListNode<Sibling> current;
	private bool recording;
	private bool won;
	
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
			foreach(var enemy in enemies) {
				enemy.Restart();	
			}
		}
	}
	private bool Won {
		get {
			foreach(var enemy in enemies)
				if(!enemy.Dieing)
					return false;
			return true;
		}
	}
	private bool Lost {
		get {
			foreach(var sibling in siblings)
				if(!sibling.Dieing)
					return false;
			return true;
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
			}
		}
		
		tfm.position = new Vector3(Current.Value.transform.position.x, Current.Value.transform.position.y)  + initialPosition;
		
		foreach(var sibling in siblings) {
			foreach(var enemy in enemies) {
				if(!sibling.Dieing && !enemy.Dieing)
					if(enemy.Tfm.position.x - sibling.Tfm.position.x <= ATTACK_RANGE)
						Battle(sibling, enemy);
			}
		}
	}
	
	private void Battle(Sibling sibling, Enemy enemy) {
		if(sibling.Weapon.Beats(enemy.Weapon)) {
			sibling.Attack();
			enemy.Die();
		} else if(enemy.Weapon.Beats(sibling.Weapon)) {
			if(sibling.Ability) {
				sibling.UseAbility();
				enemy.Die();
			} else {
				sibling.Die();
				enemy.Attack();
			}
		}
	}
	
	void OnGUI() {
		GUI.skin = SkinGUI;
		if(Won || Lost) {
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();;
			Menu.MenuButton(Won?"Won":"Lost", Won?Settings.Palete.Green:Settings.Palete.Red, () => Application.LoadLevel("Game"));
			Menu.MenuButton("Menu", Settings.Palete.Blue, () => Application.LoadLevel("Main"));
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginVertical();
			GUILayout.Label((Recording?CurrentSibling.Seconds:Seconds).ToString("G3"));
			if(Recording)
				Menu.MenuButton("Record", Settings.Palete.Red, () => Recording = false);
			else
				Menu.MenuButton("Play", Settings.Palete.Green, () => Recording = true);
			GUILayout.EndVertical();
		}
	}
}
