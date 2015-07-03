using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	
	private static GameMode m_Instance = null;
	public static GameMode GetGameMode(){
		return m_Instance;
	}
	
	public AudioClip BGM;
	
	private const int MaxLinkCnt = 10;
	private List<LineRenderer> m_LineRenderers = new List<LineRenderer>();
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < MaxLinkCnt; ++i) {
			GameObject Obj = new GameObject();
			Obj.name = "LineRender";
			Obj.AddComponent<LineRenderer>();
			LineRenderer comp = Obj.GetComponent<LineRenderer>();
			comp.SetWidth (0.1f, 0.1f);
			comp.useWorldSpace = true;
			comp.enabled = false;
			comp.SetVertexCount(2);
			m_LineRenderers.Add(comp);
		}

		//initiate inputmanager
		InputManager.GetInputManager ();
		
		m_Instance = this;
		AudioSource BGMComponent = gameObject.AddComponent<AudioSource> ();
		BGMComponent.loop = true;
		BGMComponent.clip = BGM;
		BGMComponent.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
		for (int i = 0; i < MaxLinkCnt; ++i) {
			LineRenderer comp = m_LineRenderers[i];
			comp.enabled = false;
		}

		bool bDrawToTouch = true;
		if (m_LinkedMonsters.Count == MaxLinkCnt)
			bDrawToTouch = false;
		for (int i = 0; i < Mathf.Min(m_LinkedMonsters.Count, m_LineRenderers.Count); ++i) {
			Vector3 Pos = m_LinkedMonsters [i].transform.position;
			Pos.z = -5;
			if(i != 0)
			{
				m_LineRenderers[i-1].enabled = true;
				m_LineRenderers[i-1].SetPosition(1,Pos);
			}
			m_LineRenderers[i].SetPosition(0,Pos);

			//draw to finger
			if(bDrawToTouch && i == m_LinkedMonsters.Count - 1)	{
				Vector3 TouchWorldPos = Camera.main.ScreenToWorldPoint(CurTouchedScreenPos);
				TouchWorldPos.z = -5;
				m_LineRenderers[i].SetPosition(1,TouchWorldPos);
				m_LineRenderers[i].enabled = true;
			}
		}
	}
	
	private List<GameObject> m_LinkedMonsters = new List<GameObject>();
	
	public void OnTouched(Vector3 ScreenPos){
	}
	
	public void OnReleased(Vector3 ScreenPos){
		for (int i = 0; i < m_LinkedMonsters.Count; ++i) {
			MonsterCreature MonsterScript = m_LinkedMonsters[i].GetComponent<MonsterCreature> ();
			if(MonsterScript)
			{
				MonsterScript.SetLink(false);
				MonsterScript.TakeDamage(300);
			}
		}
		m_LinkedMonsters.Clear ();
	}

	public static Vector3 CurTouchedScreenPos;
	public void OnTouchMoved(Vector3 ScreenPos){
		CurTouchedScreenPos = ScreenPos;
		if (m_LinkedMonsters.Count < MaxLinkCnt) {
			GameObject TouchedMonster = FindFocusObject (ScreenPos);
			if (TouchedMonster) {
				MonsterCreature MonsterScript = TouchedMonster.GetComponent<MonsterCreature> ();
				if(MonsterScript){
					if(MonsterScript.SetLink(true))
						m_LinkedMonsters.Add(TouchedMonster);
				}
			}
		}
	}
	
	private GameObject FindFocusObject(Vector2 ScreenPos){
		Ray ray = Camera.main.ScreenPointToRay (ScreenPos);
		RaycastHit Hit;
		if (Physics.Raycast (ray, out Hit, Mathf.Infinity)) {
			MonsterCreature MonsterScript = Hit.transform.GetComponent<MonsterCreature>();
			if(MonsterScript)
			{
				return MonsterScript.gameObject;
			}
		}
		return null;
	}
}
