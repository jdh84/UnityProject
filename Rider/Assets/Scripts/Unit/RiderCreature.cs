using UnityEngine;
using System.Collections;

public class RiderCreature : Creature {

	public override void OnDead(){
	}

	protected override void UpdateMovement(){
		//do not move
	}

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateMovement ();
	}
}
