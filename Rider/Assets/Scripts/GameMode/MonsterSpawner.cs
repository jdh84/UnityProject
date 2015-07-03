using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour {

	private static Vector3 DefaultSpawnPos = new Vector3 (0.0f, 8.0f, -5.0f);

	public int MonsterID = -1;
	public Vector3 SpawnPos = DefaultSpawnPos;
	public float SpawnDelay = 1.0f;
	
	private float m_RemainSpawnDelay = 0.0f;

	//random
	private System.Random r = new System.Random ();

	private void DoSpawn(){
		MonsterData MonData; 
		DataTable.Get ().GetMonsterData (MonsterID, out MonData);
		if (MonData != null) {
			GameObject MonsterObject = (GameObject)Resources.Load("Prefabs/Creature/Monster/" + MonData.Name) as GameObject;
			GameObject NewMon = Instantiate<GameObject>(MonsterObject);
			if(NewMon){
				NewMon.transform.position = SpawnPos;
				MonsterCreature MonScript = NewMon.AddComponent<MonsterCreature>();

				if(MonScript != null){
					MonScript.Speed = 2.0f;
					MonScript.HP = MonData.HP;

					//set collision size
					MonScript.SetColliderRadius(MonData.CollisionRadius);
					
					//set attr randomly
					Attribute CurAttr = (Attribute)(r.Next((int)Attribute.ATTR_NONE, (int)Attribute.ATTR_MAX));
					MonScript.SetAttr(CurAttr);
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		m_RemainSpawnDelay = SpawnDelay;	
	}
	
	// Update is called once per frame
	void Update () {
		//assume Deltatime >= SpawnDelay;
		m_RemainSpawnDelay -= Time.deltaTime;
		if (m_RemainSpawnDelay <= 0.0f) {
			DoSpawn();
			m_RemainSpawnDelay += SpawnDelay;
		}
	}
}
