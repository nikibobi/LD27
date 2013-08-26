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
	
	public float Speed;
	
	public WeaponType Weapon { get; set; }
	protected Queue<Key> Moves { get; private set; }
	protected SkeletonAnimation SkeletonAnimation { get; private set; }
	
	protected virtual void Start() {
		Moves = new Queue<Key>();
		SkeletonAnimation = GetComponent<SkeletonAnimation>();
	}
	
	public void Restart() {
		Moves.Clear();
	}
	
	protected virtual void Update() {
		if(SkeletonAnimation.state.Animation.Name == "walk") {
			transform.position += new Vector3(Mathf.Sign(transform.localScale.x) * Speed * Time.deltaTime , 0, 0);	
		}
	}
	
	public void Idle() {
		//stay in one place
		SkeletonAnimation.state.AddAnimation("idle", true);
	}
	
	public void Walk() {
		//moves foreward
		SkeletonAnimation.state.ClearAnimation();
		SkeletonAnimation.state.AddAnimation("walk", true);
	}
	
	public void Jump() {
		//jump up
		SkeletonAnimation.state.ClearAnimation();
		SkeletonAnimation.state.AddAnimation("jump", false);
	}
	
	public void Attack() {
		//attack in front of me
		SkeletonAnimation.state.ClearAnimation();
		SkeletonAnimation.state.SetAnimation("attack-" + Weapon.ToString().ToLower(), false);
	}
}
