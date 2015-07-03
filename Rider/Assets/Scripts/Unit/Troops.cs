using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Troops : MonoBehaviour {

	public MovingPattern TroopsMovingPattern;
	public List<GameObject> Monsters;

	private List<GameObject> m_SpawnedMonsters = new List<GameObject>();
	// Use this for initialization
	void Start () {
		if (!TroopsMovingPattern || Monsters.Count == 0)
			Destroy (this);

		//spawn all troop members at once
		foreach (GameObject Monster in Monsters) {
			if(!Monster)
				continue;

			GameObject SpawnedMonster = Instantiate<GameObject>(Monster);
			m_SpawnedMonsters.Add(SpawnedMonster);
		}
	}
	
	// Update is called once per frame
	private float m_ElapsedTime = 0.0f;
	void Update () {
		m_ElapsedTime += Time.deltaTime;

		//update Troops Location
		int TroopCnt = m_SpawnedMonsters.Count;
		for (int i = 0; i < TroopCnt; ++i) {
			if(!m_SpawnedMonsters[i])
				continue;

			Vector3 WantedLoc = TroopsMovingPattern.GetTroopLoc(i, TroopCnt, m_ElapsedTime, gameObject.transform.position);		
			m_SpawnedMonsters[i].transform.Translate(WantedLoc - m_SpawnedMonsters[i].transform.position);
		}
	}

	private void DestroyAllMonster()
	{
		foreach (GameObject Monster in m_SpawnedMonsters) {
			if (!Monster)
				continue;

			Destroy(Monster);
		}
		m_SpawnedMonsters.Clear ();
	}
}
