using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;

public abstract class Character : MonoBehaviour {
	
	public struct Key {
		public float Time;
		public Action Action;
		
		public Key(Action move)
		:this(Game.Seconds, move) {
		}
		
		public Key(float time, Action action) {
			Time = time;
			Action = action;
		}
	}
	
	public Skin Skin { get; protected set; }
	public WeaponType Weapon { get; set; }
	protected Queue<Key> Moves { get; private set; }
	
	protected virtual void Start() {
		Moves = new Queue<Key>();
	}
	
	protected virtual void Update() {
		
	}
	
	public void Idle() {
		//stay in one place
		Debug.Log("Idle");
	}
	
	public void Walk() {
		//moves foreward
		Debug.Log("Walk");
	}
	
	public void Jump() {
		//jump up
		Debug.Log("Jump");
	}
	
	public void Attack() {
		//attack in front of me
		Debug.Log("Attack");
	}
}
