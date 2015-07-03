using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{	
		transform.Translate (new Vector3 (0, Time.deltaTime * 0.15f, 0));
	}
	
	public void SetDamage(int Damage, Vector3 WorldLocation)
	{
		GUIText Text = GetComponent<GUIText> ();
		if (Text)
			Text.text = Damage.ToString ();
		
		if (Camera.main)
			transform.position = Camera.main.WorldToViewportPoint (WorldLocation);
		
		Destroy (gameObject, 0.5f);
	}
}
