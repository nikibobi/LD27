﻿using UnityEngine;
using System;
using System.Collections;
using Spine;

public abstract class Sibling : MonoBehaviour {
	
	public float Health { get; set; }
	public bool Ability { get; set; }
	public Skin Skin { get; protected set; }
	
	void Start() {
		Health = 100f;
		Ability = true;
	}
	
	void Update() {
		
	}
	
	public bool UseAbility() {
		if(!Ability)
			return false;
		MyAbility();
		return Ability = false;
	}
	
	protected abstract void MyAbility();
	
	public bool Attack() {
		//attack and return true if the attack was successfull?
		throw new NotImplementedException("attack not implemented");
	}
	
	public void Move(Vector3 direction) {
		//check if direction is left or right
		if(direction != Vector3.left && direction != Vector3.right)
			throw new NotImplementedException("Can only move left or right. Used:" + direction);
		//move the character in that direction
	}
	
	public void Jump() {
		//jump the character
	}
}
