using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;

public abstract class Character : MonoBehaviour {
	
	public class Key {
		public float Time;
		public Action Action;
		
		public Key()
		:this(Game.Seconds, null) { }
		
		public Key(float time)
		:this(time, null) { }
		
		public Key(Action move)
		:this(Game.Seconds, move) { }
		
		public Key(float time, Action action) {
			Time = time;
			Action = action;
		}
	}
	
	public WeaponType Weapon { get; set; }
	protected Queue<Key> Moves { get; private set; }
	protected SkeletonAnimation SkeletonAnimation { get; private set; }
	
	protected virtual void Start() {
		Moves = new Queue<Key>();
		SkeletonAnimation = GetComponent<SkeletonAnimation>();
	}
	
	protected virtual void Update() {
		SkeletonAnimation.Update();
	}
	
	public void Idle() {
		//stay in one place
	}
	
	public void Walk() {
		//moves foreward
		SkeletonAnimation.state.SetAnimation("walk", true);
	}
	
	public void Jump() {
		//jump up
		SkeletonAnimation.state.SetAnimation("jump", false);
	}
	
	public void Attack() {
		//attack in front of me
	}
}
