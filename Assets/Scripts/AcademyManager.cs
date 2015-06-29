using UnityEngine;
using System.Collections;
using _Define;

public class AcademyManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.FindChild("RepositionTopleft").transform.FindChild("btn_back").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			SKCommon.m_MainManager.setMainState(StateMain.MENU);
		} ));;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
