using UnityEngine;
using System.Collections;

public abstract class Creature : MonoBehaviour {
	public string CreatureName = "UnNamedCreature";
	public int HP = 100;
	public float Speed = 1.0f;
	public Vector3 Dir = new Vector3(0.0f,-1.0f,0.0f);
	public GameObject DamageTextUI;

	protected bool IsDead(){
		return m_IsDead;
	}
	private bool m_IsDead = false;
	private int m_CurrentHP = 0;
	// Use this for initialization

	protected virtual void Init()
	{
		m_CurrentHP = HP;
		Dir.Normalize ();
	}

	void Start () {

	}

	public virtual void OnDead(){
	}

	public virtual int TakeDamage(int Damage){
		if (IsDead ())
			return 0;
		int OldHP = m_CurrentHP;
		m_CurrentHP -= Damage;
		if (m_CurrentHP <= 0) {
			m_IsDead = true;
			OnDead();
		}
		int ActualDamage = OldHP - m_CurrentHP;
		if (DamageTextUI) {
			GameObject DText = Instantiate<GameObject> (DamageTextUI);
			DamageText script = DText.GetComponent<DamageText>();
			if(script)
				script.SetDamage(ActualDamage,transform.position);
		}
		return ActualDamage;
	}

	protected virtual void UpdateMovement(){
		transform.Translate (Speed * Time.deltaTime * Dir);
	}
	// Update is called once per frame
	void Update () {
		UpdateMovement ();
	}
}
