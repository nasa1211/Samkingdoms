using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _Define;

public class ShopManager : MonoBehaviour {

	GameObject mainObj;
	GameObject subObj;
	GameObject prefab;

	int mainCnt = 4;
	int equip_count = 10;
	int tools_count = 15;
	int boxs_count = 20;

	List<GameObject> _List = new List<GameObject>();

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		if(mainObj == null){
			mainObj = transform.FindChild("RepositionCenter").transform.FindChild("main_window").gameObject;
		}

		for(int toggleIndex=0;toggleIndex<mainCnt;toggleIndex++){
			UIToggle toggle = mainObj.transform.FindChild("Toggle_obj").transform.GetChild(toggleIndex).GetComponent<UIToggle>();
			toggle.onChange.Add(new EventDelegate( () => {
				Debug.Log("toggle.value : "+toggle.value+", name : "+toggle.name);
				if(toggle.value){

					createItems(toggle.name);

				}
			}));
		}//for

		//init
		createItems("toggle_1");

		transform.FindChild("RepositionTopleft").transform.FindChild("btn_back").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			SKCommon.m_MainManager.setMainState(StateMain.MENU);
		} ));;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void createItems(string itemName){
		for(int index=0;index<mainCnt;index++){
			string strName = itemName.Substring(0,7)+(index+1);
			if(itemName.Equals(strName)){
				
				if(_List != null){
					foreach(GameObject obj in _List){
						DestroyObject(obj);
					}
					_List.Clear();
				}
				
				bool bItemBtn = false;
				int item_count = 0;
				
				ShopState state = (ShopState)index;
				
				switch(state){
				case ShopState.EQUIP:
					subObj = mainObj.transform.FindChild("Equip_obj").transform.gameObject;
					prefab = Resources.Load("prefabs/_PShopEquipIItem") as GameObject;
					item_count = equip_count;
					bItemBtn = true;
					break;
				case ShopState.TOOLS:
					subObj = mainObj.transform.FindChild("Tools_obj").transform.FindChild("_PDrawble").gameObject;
					prefab = Resources.Load("prefabs/_PShopToolsBox") as GameObject;
					item_count = tools_count;
					break;
				case ShopState.BOOKS:
					break;
				case ShopState.TREASURE_BOX:
					subObj = mainObj.transform.FindChild("Box_obj").transform.FindChild("_PDrawble").gameObject;
					prefab = Resources.Load("prefabs/_PShopTreasureBox") as GameObject;
					item_count = boxs_count;
					bItemBtn = true;
					
					break;
				}//switch
				
				for(int i=0;i<item_count;i++)
				{
					GameObject popObj = GameObject.Instantiate(prefab) as GameObject;
					popObj.transform.parent = subObj.GetComponentInParent<UIPanel>().gameObject.transform;
					popObj.transform.localPosition = subObj.transform.localPosition;
					popObj.transform.localRotation = Quaternion.identity;
					popObj.transform.localScale = new Vector3(1,1,1);
					popObj.SetActive(true);
					
					//init select box 0 index
					if(state == ShopState.TOOLS && i == 0){
						popObj.GetComponent<UIToggle>().startsActive = true;
					}
					else if(state == ShopState.TREASURE_BOX){
						//상자 보유 갯수
						popObj.transform.FindChild("_NumBg").transform.FindChild("_Num").GetComponent<UILabel>().text = ""+Random.Range(0, 100);
					}
					
					if(bItemBtn){
						UIButton btnObj = popObj.transform.FindChild("_Btn").GetComponent<UIButton>();
						EventDelegate.Set(btnObj.onClick, delegate {
							onClicked();
						});
					}
					_List.Add(popObj);
				}
				subObj.GetComponent<UIGrid>().Reposition();
				
			}//if
			
		}
	}

	public void onClicked()
	{
		GameObject popup = GameObject.FindGameObjectWithTag("popupController");
		if (popup) {
			PopupManager p = popup.GetComponent<PopupManager> ();
			p.startPopup (PopState.BUY_SHOP, null);
			p._onPopupBtn += (int type, int tag) => {
				p.onDestory ();
			};
		}
	}

}
