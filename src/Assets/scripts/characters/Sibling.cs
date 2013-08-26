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
	
	private bool started;
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
					Selected = false;
					break;
				case MoveState.Recording:
					started = false;
					Selected = true;		
					break;
			}
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
		State = MoveState.NotSelected;
	}
	
	public override void Restart()
	{
		base.Restart();
		State = MoveState.Recording;
		Direction = 1;
		Ability = true;
		Seconds = 0;
	}
	
	protected override void Update() {
		base.Update();
		switch(State) {
			case MoveState.NotSelected:
				break;
			case MoveState.Recording:
				if(started)
					Seconds += Time.deltaTime;
			
				var key = new Key();
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
				if(Moves.Count > 0 && Moves.Peek().Time <= Game.Seconds)
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
