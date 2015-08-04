using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using _Define;

public class MenuManager : MonoBehaviour {

	private int[] areaOnTemp = {1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
	private int[] areaIsPerfect = {59,29,10,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

	// Use this for initialization
	void Start () {

		updateMapAreaItem();

		GameObject leftBtn = transform.FindChild("RepositionTopleft").transform.FindChild("_PMainMenu").gameObject;
		for(int i=0;i<leftBtn.transform.childCount;i++){
			UIButton _btn = leftBtn.transform.GetChild(i).GetComponent<UIButton>();
			_btn.onClick.Add (new EventDelegate( ()=>{
				Debug.Log("Left Menu Clicked : "+_btn.name);
				if(_btn.name.Equals("_Btn1")){
					SKCommon.m_MainManager.setMainState(StateMain.BARRACKS);
				}
				else if(_btn.name.Equals("_Btn2")){
					SKCommon.m_MainManager.setMainState(StateMain.TAVERN);
				}
				else if(_btn.name.Equals("_Btn3")){
					SKCommon.m_MainManager.setMainState(StateMain.ACADEMY);
				}
				else if(_btn.name.Equals("_Btn4")){
					SKCommon.m_MainManager.setMainState(StateMain.STORAGE);
				}
				else if(_btn.name.Equals("_Btn5")){
//					SKCommon.m_MainManager.setMainState(StateMain.ACADEMY);
				}
				else if(_btn.name.Equals("_Btn6")){
					SKCommon.m_MainManager.setMainState(StateMain.SHOP);
				}
			} ));
		}

//		transform.FindChild("RepositionTopleft").transform.FindChild("energy").transform.FindChild("btn").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
//		} ));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void updateMapAreaItem()
	{
		GameObject mapLayer = transform.FindChild("RepositionCenter").transform.FindChild("main_bg").transform.FindChild("_LayerMapObj").gameObject;
		for(int i=0;i<mapLayer.transform.childCount;i++){
			GameObject point = mapLayer.transform.GetChild(i).gameObject;
			GameObject obj =  SKCommon.loadPrefeb("_PMapObj", point);
			obj.transform.localPosition = Vector3.zero;

			MapAreaManager mapClass = obj.GetComponent<MapAreaManager>();
			mapClass.setMapObjIndex(i);
			mapClass.setMapObjOn(areaOnTemp[i]==1?true:false);
			mapClass.setMapObjClearCount(areaIsPerfect[i]);
			mapClass.drawMapObj();
		}
	}
}
