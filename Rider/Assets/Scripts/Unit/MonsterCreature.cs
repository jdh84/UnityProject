using UnityEngine;
using System.Collections;

public enum Attribute
{
	ATTR_NONE,
	ATTR_FIRE,
	ATTR_WATER,
	ATTR_EARTH,
	ATTR_WIND,
	ATTR_MAX
}

public class MonsterCreature : Creature {

	private GameObject m_LinkEffect;

	// Use this for initialization
	void Start () {	
		Init ();
		DamageTextUI = (GameObject)Instantiate(Resources.Load ("Prefabs/UI/DamageTextUI"));
	}

	//collision
	private float ColliderRadius = 1.0f;
	public void SetColliderRadius(float R){
		ColliderRadius = R;
		SphereCollider NewCollider = GetComponent<SphereCollider> ();
		if(!NewCollider)
			gameObject.AddComponent<SphereCollider> ();

		if (NewCollider)
			NewCollider.radius = ColliderRadius;
	}

	//attribute
	private Attribute m_Attribute = Attribute.ATTR_NONE;
	public void SetAttr(Attribute Attr)	{
		m_Attribute = Attr;
	}

	public Attribute GetAttr() {
		return m_Attribute;
	}

	//animation
	private Animator m_Animator = null;
	private Animator GetAnimator(){
		if (m_Animator == null) {
			m_Animator = GetComponent<Animator>();
		}
		return m_Animator;
	}

	public override void OnDead(){
		GameObject SpawnedDeadEff = (GameObject)Instantiate(Resources.Load ("Prefabs/Effects/eff_explosion_B"));
		if (SpawnedDeadEff) {
			SpawnedDeadEff.transform.position = gameObject.transform.position;
			Destroy (SpawnedDeadEff, 0.5f); //lasting 1sec.
		}
		Destroy (gameObject, 1.0f);
	}

	//stun
	public void MakeStuned(float StunTime){
		if (IsDead ())
			return;
		m_bRemainStunTime = StunTime;
	}
	private float m_bRemainStunTime = 0.0f;
	private bool IsStuned() {
		return m_bRemainStunTime > 0.0f;
	}

	private MovingPattern m_MovingPattern;
	public void SetMovingPattern <T> () where T:MovingPattern{
		if (!m_MovingPattern)
			m_MovingPattern = gameObject.AddComponent<T>();
	}

	//update movement
	protected override void UpdateMovement(){
		if (IsDead ())
			return;
		if (IsStuned ())
			return;
		if (IsLinked ())
			return;

		Vector3 NextMove;
		if (!m_MovingPattern) {
			NextMove = Speed * Dir * Time.deltaTime;
		} else {
			NextMove = m_MovingPattern.GetNextMove(Time.deltaTime, Speed);
		}

		transform.Translate (NextMove);
	}

	private void UpdateAnim()
	{
		Animator Animator = GetAnimator ();
		if (Animator) {
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateMovement ();
		UpdateAnim ();
	}

	private bool m_bIsLinked = false;

	public bool SetLink(bool bLink){

		bool OldLink = m_bIsLinked;

		m_bIsLinked = bLink;

		if (m_bIsLinked) {
			//if (!m_LinkEffect)
			//	m_LinkEffect = (GameObject)Instantiate (Resources.Load ("Prefabs/Effects/eff_explosion_B"));
			if(m_LinkEffect){
				m_LinkEffect.SetActive (true);
				m_LinkEffect.transform.position = gameObject.transform.position;
			}
		} else {
			if(m_LinkEffect){
				m_LinkEffect.SetActive(false);
			}
		}

		if (OldLink != m_bIsLinked) {
			if(m_bIsLinked)
				return true; //noti newly linked
		}
		return false;
	}
	public bool IsLinked(){
		if (IsDead ())
			return false;

		return m_bIsLinked;
	}
}
