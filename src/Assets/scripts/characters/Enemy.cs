using UnityEngine;
using System;
using System.Collections;

public class Enemy : Character {
	
	protected override void Start()
	{
		base.Start();
		Restart();
	}
	
	public override void Restart()
	{
		base.Restart();
		Moves.Enqueue(new Key(0f, Idle));
		Moves.Enqueue(new Key(1f, Walk));
		Direction = -1;
	}
	
	protected override void Update()
	{
		base.Update();
		if(Game.RecordingState) {
			if(Game.CurrentSibling.State == Sibling.MoveState.Recording) {
				if(Moves.Count > 0 && Game.CurrentSibling.Seconds >= Moves.Peek().Time) {
					Moves.Dequeue().Action();
				}	
			}
		} else {
			if(Moves.Count > 0 && Game.Seconds >= Moves.Peek().Time) {
				Moves.Dequeue().Action();
			}
		}
	}
}
