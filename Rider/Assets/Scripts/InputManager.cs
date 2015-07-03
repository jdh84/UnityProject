using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	private static GameObject m_TrailEffect;
	private static InputManager m_Instance;
	private static GameObject container;
	public static InputManager GetInputManager()
	{
		if(!m_Instance)
		{
			container = new GameObject();  
			container.name = "InputManager";  
			m_Instance = container.AddComponent(typeof(InputManager)) as InputManager;  
		}
		return m_Instance;
	}

	// Use this for initialization
	void Start () {
		m_IsWindows = (Application.platform.ToString () == "WindowsEditor");
		m_TrailEffect = (GameObject)Instantiate(Resources.Load ("Prefabs/Effects/eff_trail_fire01"));
		if(m_TrailEffect)
			m_TrailEffect.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		UpdateInput ();
	}

	private bool m_IsWindows = false;
	private bool IsWindows(){
		return m_IsWindows;
	}

	public bool OldIsTouched = false;
	private void UpdateInput()
	{
		bool IsTouched = false;
		Vector3 TouchedPos = new Vector3(0,0,0);

		//For Windows Compatible, do not using touch.phase
		if (IsWindows ()) 
		{		
			IsTouched = Input.GetMouseButton (0);
			TouchedPos = Input.mousePosition;
		} 
		else 
		{
			if(Input.touchCount > 0)
				IsTouched = true;
			
			if(IsTouched)
			{
				Touch CurTouch = Input.GetTouch(0);
				TouchedPos = CurTouch.position;
			}
			
		}
		
		if (IsTouched && !OldIsTouched) 
		{
			OnTouched (TouchedPos);
		}
		
		if (!IsTouched && OldIsTouched)    
		{
			OnReleased (TouchedPos);
		}
		
		if (IsTouched) 
		{
			OnTouchMoved (TouchedPos);		
		}

		OldIsTouched = IsTouched;
	}
		
	private void OnTouched(Vector3 ScreenPos){
		if(GameMode.GetGameMode ())
			GameMode.GetGameMode ().OnTouched (ScreenPos);

		SetTrailPos (ScreenPos);
		if (m_TrailEffect) {
			m_TrailEffect.SetActive (true);
		}
	}

	private void OnReleased(Vector3 ScreenPos){
		if(GameMode.GetGameMode ())
			GameMode.GetGameMode ().OnReleased (ScreenPos);

		if(m_TrailEffect)
			m_TrailEffect.SetActive (false);
	}
	private void OnTouchMoved(Vector3 ScreenPos){
		if(GameMode.GetGameMode ())
			GameMode.GetGameMode ().OnTouchMoved (ScreenPos);

		SetTrailPos (ScreenPos);
	}

	private void SetTrailPos(Vector2 ScreenPos)	{
		if (m_TrailEffect) {
			Vector3 TouchWorldPos = Camera.main.ScreenToWorldPoint (ScreenPos);
			TouchWorldPos.z = -5;
			m_TrailEffect.transform.position = TouchWorldPos;
		}
	}
}
