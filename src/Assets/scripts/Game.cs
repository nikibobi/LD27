using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public GameObject[] SiblingPrefabs;
	
	private LinkedList<Sibling> siblings;
	private LinkedListNode<Sibling> current;
	
	public static float Seconds { get; private set; }

	void Start() {
		Restart();
	}
	
	void Restart() {
		siblings = new LinkedList<Sibling>();
		foreach(var prefab in SiblingPrefabs) {
			siblings.AddLast(prefab.GetComponent<Sibling>());
		}
		current = siblings.First;
		current.Value.State = Sibling.MoveState.Recording;
	}
	
	void Update() {
		Seconds += Time.deltaTime;
		if(Input.GetKeyDown(Settings.Keymap.NextSibling)) {
			current.Value.State = Sibling.MoveState.Playing;
			current = current.NextOrFirst();
			current.Value.State = Sibling.MoveState.Recording;
		}
		if(Input.GetKeyDown(Settings.Keymap.LastSibling)) {
			current.Value.State = Sibling.MoveState.Playing;
			current = current.PreviousOrLast();
			current.Value.State = Sibling.MoveState.Recording;
		}
	}
}
