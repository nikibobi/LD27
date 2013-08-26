using UnityEngine;
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
	
	public LinkedListNode<Sibling> Current {
		get {
			return current;
		}
		set {
			if(current != null) {
				current.Value.State = Sibling.MoveState.Playing;
			}
			current = value;
			current.Value.State = Sibling.MoveState.Recording;
		}
	}
	
	public static float Seconds { get; private set; }

	void Start() {
		enemies = new List<Enemy>();
		siblings = new LinkedList<Sibling>();
		tfm = transform;
		initialPosition = tfm.position;
		Restart();
	}
	
	void Restart() {
		Seconds = 0;
		
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
		current = siblings.First;
		current.Value.State = Sibling.MoveState.Recording;
	}
	
	void Update() {
		Seconds += Time.deltaTime;
		
		if(Current.Value.Seconds >= 10)
			Current = Current.NextOrFirst();
		
		if(Input.GetKeyDown(Settings.Keymap.NextSibling))
			Current = Current.NextOrFirst();
		if(Input.GetKeyDown(Settings.Keymap.LastSibling))
			Current = Current.PreviousOrLast();
		if(Input.GetKeyDown(Settings.Keymap.Restart))
			Current.Value.Restart();
		
		tfm.position = new Vector3(Current.Value.transform.position.x, Current.Value.transform.position.y)  + initialPosition;
	}
	
	void OnGUI() {
		//draw gui stuff here depending on state
		GUI.skin = SkinGUI;
		GUILayout.BeginVertical();
		GUILayout.Label(Current.Value.Seconds.ToString("G3"));
		GUILayout.Label(Seconds.ToString("G3"));
		GUILayout.EndVertical();
	}
}
