using UnityEngine;
using System.Collections;
using _Define;

public class PopupManager : MonoBehaviour
{

	public GameObject popPenal;
	public bool isPopup = false;
	private PopState m_state;
	static PopupManager current = null;
	static GameObject container = null;
	public GameObject bg;
	GameObject popObj;

	public delegate void _delegate (int type,int tag);

	public event _delegate _onPopupBtn;

	float INVOKE_TIME = 2f;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public static PopupManager Instance {
		get {
			if (current == null) {
				container = new GameObject ();
				container.name = "PopupManager";
				current = container.AddComponent (typeof(PopupManager)) as PopupManager;
			}
			return current;
		}
	}

	public void onDestory ()
	{
		Destroy (popObj);
		Destroy (current);
		DestroyObject (container);
		current = null;
		container = null;
		isPopup = false;
		bg.SetActive (isPopup);
	}

	public void startPopup (PopState state, GameObject manager)
	{
		Debug.Log("Popup state : "+state);
		onDestory ();
		UIButton btnObj;
		m_state = state;
		isPopup = true;
		bg.SetActive (isPopup);
		Debug.Log ("popup check!!!! " + popPenal.transform.childCount);

		switch (m_state) {
		case PopState.VIEW_TEXT: case PopState.BUY_EQUIP:
			popObj = SKCommon.loadPrefeb("Popup/_PopupOnlyText", popPenal);
			break;
		case PopState.BUY_SHOP:
			popObj = SKCommon.loadPrefeb("Popup/_PopupItemBuyOk", popPenal);
			
			btnObj = popObj.transform.FindChild ("_Btn").GetComponent<UIButton> ();
			EventDelegate.Set (btnObj.onClick, delegate {
				onClicked (0, 0);
			});
			btnObj = popObj.transform.FindChild ("_BtnClose").GetComponent<UIButton> ();
			EventDelegate.Set (btnObj.onClick, delegate {
				onClicked (0, 1);
			});
			break;
		case PopState.OPEN_TREASURE:
			popObj = SKCommon.loadPrefeb("Popup/_PopupGetItem", popPenal);
			btnObj = popObj.transform.FindChild ("_Btn").GetComponent<UIButton> ();
			EventDelegate.Set (btnObj.onClick, delegate {
				onClicked (0, 0);
			});
			btnObj = popObj.transform.FindChild ("_BtnClose").GetComponent<UIButton> ();
			EventDelegate.Set (btnObj.onClick, delegate {
				onClicked (0, 1);
			});
			break;

		case PopState.STAGE_SELECT:
			popObj = SKCommon.loadPrefeb("Popup/_PopupStageSelect", popPenal);
			btnObj = popObj.transform.FindChild ("_BtnClose").GetComponent<UIButton> ();
			EventDelegate.Set (btnObj.onClick, delegate {
				onClicked (0, 1);
			});
			break;
		}


		if (state == PopState.VIEW_TEXT || state == PopState.BUY_EQUIP) {
			Invoke ("onDestory", INVOKE_TIME);
		}
	}

	public void onClicked (int type, int tag)
	{
		Debug.Log ("onClicked(" + type + "," + tag + ")");
		bool hasCompleteListener = (_onPopupBtn != null);

		if (hasCompleteListener) {
			_onPopupBtn (type, tag);
		}
	}
}
