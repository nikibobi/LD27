using UnityEngine;
using System.Collections;
using Spine;

public class Bobi : Sibling {

	protected override void Start() {
		base.Start();
		SkeletonAnimation.skeleton.SetSkin("Bobi");
	}
	
	protected override void Update() {
		base.Update();
	}
	
	protected override void MyAbility()
	{
		Debug.Log("Bobi's ability");
	}
}
