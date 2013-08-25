using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	public static float Seconds { get; private set; }

	void Start() {
		
	}
	
	void Update() {
		Seconds += Time.deltaTime;
	}
}
