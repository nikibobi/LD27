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
	
	private Vector3 initialPosition;
	private WeaponType weapon;
	
	public WeaponType Weapon {
		get {
			return weapon;
		}
		set {
			weapon = value;
		}
	}
	public float Direction {
		get {
			return Mathf.Sign(Tfm.localScale.x);
		}
		set {
			Tfm.localScale = new Vector3(Mathf.Sign(value) * Tfm.localScale.x, Tfm.localScale.y, Tfm.localScale.z);
		}
	}
	protected Queue<Key> Moves { get; private set; }
	protected SkeletonAnimation SkeletonAnimation { get; private set; }
	protected Transform Tfm { get; private set; }
	
	protected virtual void Start() {
		Moves = new Queue<Key>();
		SkeletonAnimation = GetComponent<SkeletonAnimation>();
		Tfm = transform;
		initialPosition = Tfm.position;
	}
	
	public virtual void Restart() {
		Moves.Clear();
		Tfm.position = initialPosition;
	}
	
	protected virtual void Update() {
		if(SkeletonAnimation.state.Animation.Name == "walk") {
			Tfm.position += new Vector3(Direction * Speed * Time.deltaTime , 0, 0);	
		}
	}
	
	public void Idle() {
		//stay in one place
		if(SkeletonAnimation.state.Animation.Name == "walk")
			SkeletonAnimation.state.SetAnimation("idle", true);
		else
			SkeletonAnimation.state.AddAnimation("idle", true);
	}
	
	public void Walk() {
		//moves foreward
		SkeletonAnimation.state.ClearAnimation();
		SkeletonAnimation.state.AddAnimation("walk", true);
	}
	
	public void Attack() {
		//attack in front of me
		SkeletonAnimation.state.ClearAnimation();
		SkeletonAnimation.state.SetAnimation("idle", false);
		SkeletonAnimation.state.SetAnimation("attack-" + Weapon.ToString().ToLower(), false);
	}
}
