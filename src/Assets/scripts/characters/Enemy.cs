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
		Moves.Enqueue(new Key(0, Idle));
		Moves.Enqueue(new Key(UnityEngine.Random.Range(0f, 5f), Walk));
		Direction = -1;
	}
	
	protected override void Update()
	{
		base.Update();
		if(Moves.Count > 0 && Moves.Peek().Time <= Game.Seconds) {
			Moves.Dequeue().Action();
		}
	}
}
