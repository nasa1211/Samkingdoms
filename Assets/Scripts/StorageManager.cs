using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _Define;

public class StorageManager : MonoBehaviour
{

	GameObject mainObj;
	GameObject prefab;
	GameObject selectPanel;
	int TOTAL_GRADE = 5;
	int mainPenalCount = 3;
	int itemTotalCount = 0;
	StorageState m_state;
	List<GameObject> _List = new List<GameObject> ();
	StorageItem itemMg;

	//temp
	int equip_count = 10;
	int tools_count = 15;
	int boxs_count = 20;
	
	// Use this for initialization
	void Start ()
	{

		checkToggleOnEvent ();

		transform.FindChild("RepositionTopleft").transform.FindChild("btn_back").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			SKCommon.m_MainManager.setMainState(StateMain.MENU);
		} ));;

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void checkToggleOnEvent ()
	{
		mainObj = transform.FindChild ("RepositionCenter").transform.FindChild ("main_window").gameObject;
		//main menu toggle
		for (int i=1; i<mainPenalCount+1; i++) {
			UIToggle to = mainObj.transform.FindChild ("Toggle_obj").transform.FindChild ("toggle_" + i).GetComponent<UIToggle> ();
			to.onChange.Add (new EventDelegate (() => {
				if (to.value) {
					if (to.name.Equals ("toggle_1"))
						createStorage (StorageState.TREASURE_BOX);
					else if (to.name.Equals ("toggle_2"))
						createStorage (StorageState.EQUIL);
					else if (to.name.Equals ("toggle_3"))
						createStorage (StorageState.TOOLS);
				}
			}));
		}

		if (m_state == StorageState.NONE) {
			createStorage (StorageState.TREASURE_BOX);
		}

		UIButton btnObj = mainObj.transform.FindChild("_SelectItem").transform.FindChild("_BtnBuy").GetComponent<UIButton>();
		EventDelegate.Set(btnObj.onClick, delegate {
			onClicked();
		});
	}

	void createStorage (StorageState state)
	{
		Debug.Log("createStorage (StorageState "+state+")");

		if(selectPanel == mainObj.transform.FindChild ("_PDrawble_" + (int)state).gameObject)
			return;

		if (state == StorageState.NONE)
			return;

		if (_List != null) {
			foreach (GameObject obj in _List) {
				DestroyObject (obj);
			}
			_List.Clear ();
		}

		//bool bItemBtn = false;

		m_state = state;
		prefab = Resources.Load ("prefabs/_PShopToolsBox") as GameObject;

		selectPanel = mainObj.transform.FindChild ("_PDrawble_" + (int)state).gameObject;
		
		switch (m_state) {
		case StorageState.TREASURE_BOX:
			itemTotalCount = equip_count;
			break;
		case StorageState.EQUIL:
			itemTotalCount = tools_count;
			break;
		case StorageState.TOOLS:
			itemTotalCount = boxs_count;
			break;
		}//switch
		
		Debug.Log ("state : " + state + ", cnt:" + itemTotalCount);
		
		for (int i=0; i<itemTotalCount; i++) {
			GameObject popObj = SKCommon.loadPrefeb("_PShopToolsBox", selectPanel);
			popObj.GetComponent<UIToggle> ().group = (int)m_state;
			
			//item setting
			StorageItem im = popObj.GetComponent<StorageItem> ();
			im.state = (int)m_state;
			im.type = Random.Range (1, 10);
			im.index = i;
			im.imageName = "Icon_Equip_00" + Random.Range (1, 4);
			im.totalCount = Random.Range (1, 99);
			
			//image input
			popObj.transform.FindChild ("_Image").GetComponent<UISprite> ().spriteName = im.imageName;
			
			switch (m_state) {
				
			case StorageState.TREASURE_BOX:
				popObj.transform.FindChild ("NumBoard").gameObject.SetActive (true);
				popObj.transform.FindChild ("NumBoard").transform.FindChild ("_Num").GetComponent<UILabel> ().text = "" + im.totalCount;
				break;
				
			case StorageState.EQUIL:
					//아이템 착용 여부
				popObj.transform.FindChild ("_EquippedCheck").gameObject.SetActive (true);
					//등급표시 별5개
				GameObject gradeGrid = popObj.transform.FindChild ("_GradeStar").gameObject;
				gradeGrid.SetActive (true);
				int cnt = TOTAL_GRADE - (Random.Range (1, 5));
				for (int z=0; z<cnt; z++) {
					GameObject o = gradeGrid.transform.FindChild ("obj_" + z).gameObject;
					DestroyImmediate (o);
				}
				gradeGrid.GetComponent<UIGrid> ().Reposition ();
				break;
				
			case StorageState.TOOLS:
				popObj.transform.FindChild ("NumBoard").gameObject.SetActive (true);
				popObj.transform.FindChild ("NumBoard").transform.FindChild ("_Num").GetComponent<UILabel> ().text = "" + im.totalCount;
				break;
			}
			
			//init select box 0 index
			if (i == 0) {
				popObj.GetComponent<UIToggle> ().startsActive = true;
			}

			_List.Add (popObj);
		}
		addToggleDelegate ();
		checkToggleSelect ();
		selectPanel.GetComponent<UIGrid> ().Reposition ();
	}

	void checkToggleSelect ()
	{
		foreach (GameObject obj in _List) {
			bool bSelect = obj.GetComponent<UIToggle> ().startsActive;
			if (bSelect) {
				insertSelectPanel (obj.GetComponent<StorageItem> ());
			}
		}
	}

	void insertSelectPanel (StorageItem im)
	{
		if(itemMg == im)
			return;

		itemMg = im;

		GameObject panel = mainObj.transform.FindChild ("_SelectItem").gameObject;
		if (!panel.activeInHierarchy)
			panel.SetActive (true);

		panel.transform.FindChild ("_Image").GetComponent<UISprite> ().spriteName = im.imageName;

		UILabel label = panel.transform.FindChild ("_Name").GetComponent<UILabel> ();
		label.text = im.itemName;
		label.AssumeNaturalSize();
		//remove
		for(int i=0;i<panel.transform.FindChild ("_Name").childCount;i++){
			DestroyImmediate(panel.transform.FindChild ("_Name").transform.GetChild(i).gameObject);
		}

		panel.transform.FindChild ("_Info").GetComponent<UILabel> ().text = im.itemInfo;

		switch (m_state) {
			
		case StorageState.TREASURE_BOX:
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "수요";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_ImageKey").GetComponent<UISprite> ().spriteName = "Detail_Icon_Key_01";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Num").GetComponent<UILabel> ().text = "8/1";
			panel.transform.FindChild ("_BtnBuy").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "사용";
			break;
			
		case StorageState.EQUIL:
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "단가";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_ImageKey").GetComponent<UISprite> ().spriteName = "Detail_Icon_Price";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Num").GetComponent<UILabel> ().text = "999,999";
			panel.transform.FindChild ("_BtnBuy").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "판매";

			float nx = label.width+100;
			GameObject abilltyPanel = SKCommon.loadPrefeb("_PAbillity", label.gameObject);
			for(int i=3;i>=0;i--){
				bool bt = Random.Range(0,2) == 0 ? false : true;
				if(!bt)
					DestroyImmediate (abilltyPanel.transform.GetChild(i).gameObject);
			}
			abilltyPanel.GetComponent<UIGrid>().Reposition();
			abilltyPanel.transform.localPosition = new Vector3(nx, 0f, 0f);
			//info label hiden
			panel.transform.FindChild ("_Info").gameObject.SetActive(false);
			break;
			
		case StorageState.TOOLS:
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "단가";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_ImageKey").GetComponent<UISprite> ().spriteName = "Detail_Icon_Price";
			panel.transform.FindChild ("_BgNum").transform.FindChild ("_Num").GetComponent<UILabel> ().text = "900";
			panel.transform.FindChild ("_BtnBuy").transform.FindChild ("_Name").GetComponent<UILabel> ().text = "판매";
			break;
		}

	}

	void addToggleDelegate ()
	{
		for (int i=0; i<itemTotalCount; i++) {
			UIToggle to = selectPanel.transform.GetChild (i).GetComponent<UIToggle> ();
			to.onChange.Add (new EventDelegate (() => {
				if (to.value) {
					StorageItem im = to.GetComponent<StorageItem> ();
					insertSelectPanel (im);
				}
			}));
		}
	}

	public void onClicked()
	{
		GameObject popup = GameObject.FindGameObjectWithTag("popupController");
		if (popup) {
			PopupManager p = popup.GetComponent<PopupManager> ();

			if(m_state == StorageState.TREASURE_BOX){
				p.startPopup (PopState.OPEN_TREASURE, null);
			}else if(m_state == StorageState.EQUIL){
				p.startPopup (PopState.BUY_EQUIP, null);
			}else if(m_state == StorageState.TOOLS){
				p.startPopup (PopState.BUY_EQUIP, null);
			}

			p._onPopupBtn += (int type, int tag) => {
				p.onDestory ();
			};
		}
	}

}
