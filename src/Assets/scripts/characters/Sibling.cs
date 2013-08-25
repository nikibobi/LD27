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
		//Debug.Log(string.Format("[{0}] State = {1}", Game.Seconds, State));
		switch(State) {
			case MoveState.None:
				//do nothing
				break;
			case MoveState.Recording:
				var key = new Key();
				if(Input.GetKeyDown(Settings.Keymap.Walk))
					key.Action = Walk;
				if(Input.GetKeyUp(Settings.Keymap.Walk))
					key.Action = Idle;
				if(Input.GetKeyDown(Settings.Keymap.Jump))
					key.Action = Jump;
				if(Input.GetKeyDown(Settings.Keymap.UseAbility))
					key.Action = UseAbility;
				
				if(key.Action != null)
				{
					key.Action();
					Moves.Enqueue(key);
				}
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
