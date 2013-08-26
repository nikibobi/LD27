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
	public float Direction {
		get {
			return Mathf.Sign(Tfm.localScale.x);
		}
		set {
			Tfm.localScale = new Vector3(Mathf.Sign(value) * Tfm.localScale.x, Tfm.localScale.y, Tfm.localScale.z);
		}
	}
	public bool Walking { get; protected set; }
	protected Queue<Key> Moves { get; private set; }
	protected SkeletonAnimation SkeletonAnimation { get; private set; }
	protected Transform Tfm { get; private set; }
	protected Vector3 InitialPosition { get; private set; }
	
	protected virtual void Start() {
		Moves = new Queue<Key>();
		SkeletonAnimation = GetComponent<SkeletonAnimation>();
		Tfm = transform;
		InitialPosition = Tfm.position;
	}
	
	public virtual void Restart() {
		Moves.Clear();
		Tfm.position = InitialPosition;
	}
	
	protected virtual void Update() {
		if(Walking)
			Tfm.position += new Vector3(Direction * Speed * Time.deltaTime , 0, 0);
	}
	
	public void Idle() {
		//stay in one place
		SkeletonAnimation.skeleton.SetBonesToSetupPose();
		if(Walking)
			SkeletonAnimation.state.SetAnimation(WeaponPostfix("idle"), true);
		else
			SkeletonAnimation.state.AddAnimation(WeaponPostfix("idle"), true);
		Walking = false;
	}
	
	public void Walk() {
		//moves foreward
		SkeletonAnimation.skeleton.SetBonesToSetupPose();
		SkeletonAnimation.state.SetAnimation("walk", true);
		Walking = true;
	}
	
	public void Attack() {
		//attack in front of me
		SkeletonAnimation.skeleton.SetBonesToSetupPose();
		SkeletonAnimation.state.SetAnimation(WeaponPostfix("attack"), false);
		Walking = false;
	}
	
	protected string WeaponPostfix(string str) {
		return 	str + "-" + Weapon.ToString().ToLower();
	}
}
