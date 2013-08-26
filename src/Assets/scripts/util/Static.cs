using UnityEngine;
using System.Collections;

public class Static : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(this);	
	}
}
