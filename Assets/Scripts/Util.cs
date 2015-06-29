using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {

	public GameObject sortObj;
	public int state;
	// Use this for initialization
	void Start () {
		setObjHorizontalAlignment ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setObjHorizontalAlignment()
	{
		if(state == 0){
			UISprite sprite = sortObj.transform.FindChild("_Gem").GetComponent<UISprite>();
			UILabel label_0 = sortObj.transform.FindChild("_LBuyNum").GetComponent<UILabel>();
			UILabel label_1 = sortObj.transform.FindChild("_LInfo").GetComponent<UILabel>();
			//get total width
			float _w = 0;
			_w += sprite.width;
			label_0.AssumeNaturalSize();
			_w += label_0.width;
			label_1.AssumeNaturalSize();
			_w += label_1.width;
			
			//obj Alignment
			float _m = sprite.width - 5f;
			label_0.transform.localPosition = new Vector3(_m, label_0.transform.localPosition.y+1f, 0f);
			_m += label_0.width + 5f;
			label_1.transform.localPosition = new Vector3(_m, label_1.transform.localPosition.y, 0f);
			
			//main obj Alignment
			sortObj.transform.localPosition = new Vector3(-(_w*0.5f), sortObj.transform.localPosition.y);
		}
		else if(state == 1){//only label
			string[] str = {"판매성공.","90,000","코인을 획득 하였습니다."};
			float _m = 0, _t = 0;
			for(int i=0;i<sortObj.transform.childCount;i++){
				UILabel la = sortObj.transform.GetChild(i).GetComponent<UILabel>();
				string tmp = i==0?"  ":(i==1?" ":"");
				la.text = str[i]+tmp;
				la.transform.localPosition = new Vector3(_m, la.transform.localPosition.y, 0f);
				_m += la.width;
			}
			sortObj.transform.localPosition = new Vector3(-(_m/2), sortObj.transform.localPosition.y+7, 0f);
		}
		else if(state == 2){//only label Center
			float _m = 0, _t = 0;
			for(int i=0;i<sortObj.transform.childCount;i++){
				UILabel la = sortObj.transform.GetChild(i).GetComponent<UILabel>();
				la.transform.localPosition = new Vector3(_m, la.transform.localPosition.y, 0f);
				_m += la.width;
			}
			sortObj.transform.localPosition = new Vector3(sortObj.transform.localPosition.x-(_m/2), sortObj.transform.localPosition.y, 0f);
		}
		else if(state == 3){//only label Left
			float _m = 0, _t = 0;
			for(int i=0;i<sortObj.transform.childCount;i++){
				UILabel la = sortObj.transform.GetChild(i).GetComponent<UILabel>();
				la.transform.localPosition = new Vector3(_m, la.transform.localPosition.y, 0f);
				_m += la.width;
			}
//			sortObj.transform.localPosition = new Vector3(sortObj.transform.localPosition.x, sortObj.transform.localPosition.y, 0f);
		}



//		int count = sortObj.transform.childCount;
//		float _w = 0, _m = 0;
//		for(int i=0;i<count;i++)
//		{
//			System.Type type = sortObj.transform.GetChild(i).GetComponent<UIWidget>().GetType();
//			if(type == typeof(UISprite))
//			{
//				UISprite sprite = sortObj.transform.GetChild(i).GetComponent<UISprite>();
//				_w += sprite.width;
//				if(i > 0)
//				{
//
//				}
//				_m += sprite.width - 5f;
//			}
//			else if(type == typeof(UILabel))
//			{
//				UILabel label = sortObj.transform.GetChild(i).GetComponent<UILabel>();
//				label.AssumeNaturalSize();
//				_w += label.width;
//			}
//		}
//		sortObj.transform.localPosition = new Vector3(-(_w*0.5f), sortObj.transform.localPosition.y);
	}
}
