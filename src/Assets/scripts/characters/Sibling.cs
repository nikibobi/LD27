using UnityEngine;
using System;
using System.Collections;
using Spine;

public class Sibling : Character {
	
	public enum MoveState {
		NotSelected,
		Recording,
		Playing
	}
	
	private MoveState state;
	
	public MoveState State { 
		get {
			return state;
		}
		set {
			state = value;
			SkeletonAnimation.state.ClearAnimation();
			switch(state) {
				case MoveState.NotSelected:
				case MoveState.Playing:
					SkeletonAnimation.state.SetAnimation("not-selected", false);
					break;
				case MoveState.Recording:
					SkeletonAnimation.state.SetAnimation("selected", false);					
					break;
			}
		}
	}
	public bool Ability { get; set; }
	
	protected override void Start() {
		base.Start();
		State = MoveState.NotSelected;
		Ability = true;
	}
	
	protected override void Update() {
		base.Update();
		switch(State) {
			case MoveState.NotSelected:
				break;
			case MoveState.Recording:
				var key = new Key();
				if(Input.GetKeyUp(Settings.Keymap.Walk) || 
				   Input.GetKeyUp(Settings.Keymap.Jump) ||
				   Input.GetKeyUp(Settings.Keymap.Attack) ||
				   Input.GetKeyUp(Settings.Keymap.UseAbility))
					key.Action = Idle;
				if(Input.GetKeyDown(Settings.Keymap.Walk))
					key.Action = Walk;
				if(Input.GetKeyDown(Settings.Keymap.Jump))
					key.Action = Jump;
				if(Input.GetKeyDown(Settings.Keymap.Attack))
					key.Action = Attack;
				if(Input.GetKeyDown(Settings.Keymap.UseAbility))
					key.Action = UseAbility;
				
				if(key.Action != null)
				{
					key.Action();
					Moves.Enqueue(key);
				}
				break;
			case MoveState.Playing:
				while(Moves.Count > 0 && Moves.Peek().Time >= Game.Seconds)
					Moves.Dequeue().Action();
				break;
			default:
				throw new NotImplementedException(string.Format("State {0} not implemented", State));
		}
	}
	
	public void UseAbility() {
		if(!Ability)
			return;
		Debug.Log(SkeletonAnimation.initialSkinName + "'s ability");
	}
}
