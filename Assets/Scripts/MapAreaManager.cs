using UnityEngine;
using System.Collections;
using _Define;

public class MapAreaManager : MonoBehaviour {

	int index;
	int clearCount;
    bool bOn;
	GameObject[] MapObj;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void setMapObjOn(bool _on)				{	bOn = _on; }
	public void setMapObjIndex(int _idx)				{	index = _idx;	}
	public void setMapObjClearCount(int _idx)		{	clearCount = _idx;	}

	public void drawMapObj(){
		if(MapObj == null){
			MapObj = new GameObject[transform.childCount];
			for(int i=0;i<transform.childCount;i++){
				MapObj[i] = transform.GetChild(i).gameObject;
				//btn delegate
				MapObj[i].GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
					if(bOn){
						Define.POPUP_OPNE(PopState.STAGE_SELECT, null);
					}
				} ));
			}
		}

		MapObj[0].SetActive(bOn);
		MapObj[1].SetActive(!bOn);

		for(int i=0;i<MapObj.Length;i++){
			MapObj[i].transform.GetChild(0).GetComponent<UILabel>().text = ""+(index+1);
		}

		if(bOn){
			MapObj[0].transform.GetChild(1).transform.GetChild(0).GetComponent<UILabel>().text = clearCount+"/"+Define.STAGE_CLERA_MAX_COUNT;
		}
	}
}
