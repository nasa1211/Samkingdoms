using UnityEngine;
using System.Collections;
using _Define;

public class MainMananger : MonoBehaviour {

	public StateMain m_state;
	public StateMain m_state_prev;

	// Use this for initialization
	void Start () {

		SKCommon.m_MainManager = transform.GetComponent<MainMananger>();

		m_state_prev = StateMain.NONE;
		setMainState(StateMain.MENU);
	}
	
	// Update is called once per frame
	void Update () {
			
	}

	public void setMainState(StateMain _state){

		if(m_state == _state)
			return;

		Debug.Log("setMainState : "+ _state);

		m_state = _state;

		if(m_state_prev != StateMain.NONE)
		{
			if(m_state_prev == StateMain.MENU)
				transform.FindChild("main").gameObject.SetActive(false);
			else if(m_state_prev == StateMain.BARRACKS)
				transform.FindChild("barracks").gameObject.SetActive(false);
			else if(m_state_prev == StateMain.TAVERN)
				transform.FindChild("tavern").gameObject.SetActive(false);
			else if(m_state_prev == StateMain.ACADEMY)
				transform.FindChild("academy").gameObject.SetActive(false);
			else if(m_state_prev == StateMain.STORAGE)
				transform.FindChild("storage").gameObject.SetActive(false);
			else if(m_state_prev == StateMain.SHOP)
				transform.FindChild("shop").gameObject.SetActive(false);
		}

		if(m_state != StateMain.NONE)
		{
			if(m_state == StateMain.MENU)
				transform.FindChild("main").gameObject.SetActive(true);
			else if(m_state == StateMain.BARRACKS)
				transform.FindChild("barracks").gameObject.SetActive(true);
			else if(m_state == StateMain.TAVERN)
				transform.FindChild("tavern").gameObject.SetActive(true);
			else if(m_state == StateMain.ACADEMY)
				transform.FindChild("academy").gameObject.SetActive(true);
			else if(m_state == StateMain.STORAGE)
				transform.FindChild("storage").gameObject.SetActive(true);
			else if(m_state == StateMain.SHOP)
				transform.FindChild("shop").gameObject.SetActive(true);
		}

		m_state_prev = m_state;
	}
}
