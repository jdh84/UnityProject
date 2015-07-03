using UnityEngine;
using System.Collections;

public class HudUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackToLobby()
	{
		Application.LoadLevel ("Lobby");
	}
}
