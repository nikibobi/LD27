using UnityEngine;
using System;
using System.Collections;
using Spine;

public abstract class Sibling : Character {
	
	public enum MoveState {
		None,
		Recording,
		Playing
	}
	
	public MoveState State { get; set; }
	public bool Ability { get; set; }
	
	protected override void Start() {
		base.Start();
		State = MoveState.None;
		Ability = true;
	}
	
	protected override void Update() {
		base.Update();
		switch(State) {
			case MoveState.None:
				Idle();
				break;
			case MoveState.Recording:
				var key = new Key(Idle);
				if(Input.GetKey(Settings.Keymap.Walk))
					key.Action = Walk;
				if(Input.GetKey(Settings.Keymap.Jump))
					key.Action = Jump;
				if(Input.GetKey(Settings.Keymap.UseAbility))
					key.Action = UseAbility;
				Moves.Enqueue(key);
				break;
			case MoveState.Playing:
				while(Moves.Peek().Time >= Game.Seconds)
					Moves.Dequeue().Action();
				break;
			default:
				throw new NotImplementedException(string.Format("State {0} not implemented", State));
		}
	}
	
	public void UseAbility() {
		if(!Ability)
			return;
		MyAbility();
	}
	
	protected abstract void MyAbility();
}
