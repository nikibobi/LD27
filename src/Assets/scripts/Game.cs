using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public GameObject[] SiblingPrefabs;
	
	private Transform cameraTransform;
	private Vector3 cameraOffset;
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
		cameraTransform = transform;
		cameraOffset = cameraTransform.position;
		Restart();
	}
	
	void Restart() {
		siblings = new LinkedList<Sibling>();
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
		if(Input.GetKeyDown(Settings.Keymap.NextSibling))
			Current = Current.NextOrFirst();
		if(Input.GetKeyDown(Settings.Keymap.LastSibling))
			Current = Current.PreviousOrLast();
		cameraTransform.position = new Vector3(Current.Value.transform.position.x, Current.Value.transform.position.y)  + cameraOffset;
	}
}
