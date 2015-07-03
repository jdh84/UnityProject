using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CVSData{
	public int ID { get; set; }

	public virtual void DoDebug(){
		Debug.Log ( "CVSData Added : " + ID);
	}
}

public class MonsterData : CVSData{
	public string Name { get; set; }
	public int HP { get; set; }
	public float CollisionRadius { get; set; }
	public override void DoDebug(){
		Debug.Log ( "MonsterData Added : " + ID + " " + Name + " " + HP + " " + CollisionRadius);
	}
}

public class DataTable{
	
	//Singleton interfaces
	private static DataTable m_Instance;
	public static DataTable Get(){
		if (m_Instance == null) {
			m_Instance = new DataTable();
			//load datas when created
			LoadDatas();
		}
		return m_Instance;
	}

	private Dictionary<int, MonsterData> MonsterDatas = null;
	private static void LoadDatas()
	{
		if (m_Instance == null)
			return;

		m_Instance.LoadCVSData<MonsterData>(out m_Instance.MonsterDatas);
	}

	private void LoadCVSData<T>(out Dictionary<int, T> DatasArray) where T:CVSData, new()
	{
		string ClassName = typeof(T).Name;
		DatasArray = new Dictionary<int, T> ();
		TextAsset CVSAsset = (TextAsset)Resources.Load("CVS/" + ClassName) as TextAsset;
		if (!CVSAsset) {
			Debug.Log ("Cannot Find CVS File : CVS/" + ClassName);
			return;
		}
		
		Debug.Log ("Load " + ClassName + "...start");
		
		string str = CVSAsset.text;
		string[] LineList = str.Split('\n');
		int LineCnt = 0;

		List<PropertyInfo> Properties = new List<PropertyInfo> ();
		foreach (string Line in LineList) {
			//ignore schema line
			if(LineCnt++ == 0)
			{
				//collect properties
				string[] SchemaTokenList = Line.Split(',');
				foreach (string SchemaToken in SchemaTokenList) {
					SchemaToken.Replace("\n", "");
					PropertyInfo Prop = typeof(T).GetProperty(SchemaToken);
					Properties.Add(Prop);
				}
				continue;
			}
			
			T newData = new T ();
			string[] TokenList = Line.Split(',');
			int TokenCnt = 0;
			//supports string, int, float
			foreach (string Token in TokenList) {
				if(Properties[TokenCnt].PropertyType == typeof(string))
					Properties[TokenCnt].SetValue(newData, Token, null);
				if(Properties[TokenCnt].PropertyType == typeof(int))
					Properties[TokenCnt].SetValue(newData, System.Convert.ToInt32(Token), null);
				if(Properties[TokenCnt].PropertyType == typeof(float))
					Properties[TokenCnt].SetValue(newData, System.Convert.ToSingle(Token), null);
				TokenCnt++;
			}
			newData.DoDebug();
			DatasArray.Add (newData.ID, newData);
		}
		Debug.Log ("Load " + ClassName + "...Done");
	}
	
	public bool GetMonsterData(int FindID, out MonsterData OutData)
	{
		return MonsterDatas.TryGetValue (FindID, out OutData);
	}
}