using UnityEngine;
using System.Collections;

public class Lobby : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Load Datatable Using Singleton initiation
		DataTable.Get ();
		//Set Screen
		Screen.SetResolution (600, 800, true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OpenLevel(int levelIndex)
	{
		Application.LoadLevel ("Level" + levelIndex.ToString ());
	}
}
