using UnityEngine;
using System.Collections;

public class MovingPattern : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual Vector3 GetNextMove(float Deltatime, float Speed)
	{
		return Vector3.zero;
	}

	public virtual Vector3 GetTroopLoc(int TroopIndex, int TroopNum, float ElapsedTime, Vector3 InitialLoc)	{
		return InitialLoc;
	}
}
