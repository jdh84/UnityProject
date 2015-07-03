using UnityEngine;
using System.Collections;

public class CurveSMovingPattern : MovingPattern {
	public float Amplitude = 1.0f;
	public float PeriodTimeInSec = 1.0f;
	private float m_ElapsedTime = 0.0f;
	private float m_InitalVerticalLoc = 0.0f;

	// Use this for initialization
	void Start () {
		m_InitalVerticalLoc = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override Vector3 GetNextMove(float Deltatime, float Speed)
	{
		Vector3 Result = Vector3.zero;
		m_ElapsedTime += Deltatime;
		Result.y = -Speed * Deltatime;
		float DesiredLocX = m_InitalVerticalLoc + Amplitude * Mathf.Sin (m_ElapsedTime * 3.14f / PeriodTimeInSec);
		Result.x = DesiredLocX - transform.position.x;
		return Result;
	}
}
