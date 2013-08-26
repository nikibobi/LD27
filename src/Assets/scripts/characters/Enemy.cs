using UnityEngine;
using System;
using System.Collections;

public class Enemy : Character {
	
	protected override void Start()
	{
		base.Start();
		Restart();
		Moves.Enqueue(new Character.Key(UnityEngine.Random.Range(0f, 4f), Attack));
	}
	
	public override void Restart()
	{
		base.Restart();
		Direction = -1;
	}
	
	protected override void Update()
	{
		if(Moves.Count > 0 && Moves.Peek().Time <= Game.Seconds) {
			Moves.Dequeue().Action();
			base.Update();
		}
	}
}
