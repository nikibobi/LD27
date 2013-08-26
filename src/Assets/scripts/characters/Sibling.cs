using UnityEngine;
using System;
using System.Collections;
using Spine;

public class Sibling : Character {
	
	public enum MoveState {
		Paused,
		Recording,
		Playing
	}
	
	private bool started;
	private MoveState state;
	
	public string SkinName {
		get {
			return SkeletonAnimation.initialSkinName;	
		}
	}
	public MoveState State { 
		get {
			return state;
		}
		set {
			state = value;
			started = false;
		}
	}
	public bool Selected {
		get {
			return Tfm.position.z == 0;
		}
		set {
			Tfm.position = new Vector3(Tfm.position.x, value?0:10, value?0:1);
			SkeletonAnimation.state.SetAnimation((value?"":"not-") + "selected", false);
		}
	}
	public bool Ability { get; set; }
	public float Seconds { get; set; }
	
	protected override void Start() {
		base.Start();
		Restart();
		State = MoveState.Paused;
	}
	
	public override void Restart()
	{
		base.Restart();
		State = MoveState.Recording;
		Ability = true;
		Seconds = 0;
		Tfm.position = InitialPosition;
		Direction = 1;
	}
	
	protected override void Update() {
		base.Update();
		switch(State) {
			case MoveState.Paused:
				Idle();
				break;
			case MoveState.Recording:
				if(started) {
					Seconds += Time.deltaTime;
				} else {
					Idle();
				}
			
				if(Seconds >= 10) {
					Seconds = 10;
					return;
				}
			
				var key = new Key(this.Seconds);
				if(Input.GetKeyUp(Settings.Keymap.Walk) || 
				   Input.GetKeyUp(Settings.Keymap.Attack) ||
				   Input.GetKeyUp(Settings.Keymap.UseAbility))
					key.Action = Idle;
				if(Input.GetKeyDown(Settings.Keymap.Walk)) {
					key.Action = Walk;
				} else if(Input.GetKeyDown(Settings.Keymap.Attack)) {
					key.Action = Attack;
				} else if(Input.GetKeyDown(Settings.Keymap.UseAbility)) {
					key.Action = UseAbility;
				}
				
				if(key.Action != null)
				{
					if(key.Action != Idle)
						started = true;
				
					key.Action();
					Moves.Enqueue(key);
				}
				break;
			case MoveState.Playing:
				if(!started) {
					Tfm.position = InitialPosition;
					Ability = true;
					started = true;
				}
				if(Moves.Count > 0 && Game.Seconds >= Moves.Peek().Time)
					Moves.Dequeue().Action();
				break;
			default:
				throw new NotImplementedException(string.Format("State {0} not implemented", State));
		}
	}
	
	public void UseAbility() {
		if(!Ability)
			return;
		SkeletonAnimation.skeleton.SetBonesToSetupPose();
		SkeletonAnimation.state.SetAnimation("special-" + SkinName, false);
		Ability = false;
		Walking = false;
	}
}
