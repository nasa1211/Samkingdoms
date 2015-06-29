using UnityEngine;
using System.Collections;

namespace _Define
{
	public class Define
	{
		public static int STAGE_CLERA_MAX_COUNT = 60;

		public static void POPUP_OPNE(PopState state, GameObject manager)
		{
			GameObject popup = GameObject.FindGameObjectWithTag("popupController");
			if (popup) {
				PopupManager p = popup.GetComponent<PopupManager> ();
				p.startPopup (state, manager);
				p._onPopupBtn += (int type, int tag) => {
					p.onDestory ();
				};
			}
		}
	}

	public enum StateMain{
		NONE = 0,
		MENU,
		BARRACKS,
		TAVERN,
		ACADEMY,
		STORAGE,
		SHOP,
	};

	public enum StorageState{
		NONE = 0,
		TREASURE_BOX,
		EQUIL,
		TOOLS,
	};

	public enum ShopState{
		EQUIP = 0,
		TOOLS,
		BOOKS,
		TREASURE_BOX,
	};

	public enum PopState{
		NONE = 0,
		VIEW_TEXT,
		OPEN_TREASURE,
		BUY_SHOP,
		BUY_EQUIP,
		STAGE_SELECT,
	};

	public class SKItem{
		public string ID = "IT_00001";
		public int STAR = 1;
		public string KIND = "WEAPON";
		public string NAME = "1성무기임";
		public bool ONSALE = true;
		public int BUY_JEWEL = 10;
		public int SELL_GOLD = 10000;
		public int USER_LIMIT = 0;
		public int HEALTH = 100;
		public int ATTACK = 100;
		public int DEFENSE = 100;
		public int  CRITICAL = 100;
		public int DODGE =100;
		public int[] ADDITIONAL;
		public int[] RANDOM_OPTION;
		public string DESC = "설명";
	};

	public class SKTools{
		public string ID = "TO_00001";
		public string NAME = "은안장";
		public bool ONSALE = true;
		public int BUY_JEWEL = 2;
		public int SELL_GOLD = 1000;
		public int ONEDAY_LIMIT = 3;
		public string DESC = "은안장임다";
	};

	public class SKBox{
		public string ID = "BX_00001";
		public string KIND = "BOX";
		public string RELATED_TO = "BX_00002";
		public string NAME = "나무상자";
		public bool ONSALE = true;
		public int BUY_JEWEL = 5;
		public int ONEDAY_LIMIT = 20;
		public string DESC = "허름한 나무상자";
	};

	public class SKBook{
		public string ID =  "BK_00001";
		public string NAME = "하급스크롤";
		public bool ONSALE = true;
		public int BUY_JEWEL = 20;
		public int ONEDAY_LIMIT = 5;
		public string DESC = "1~3스킬업";
	};

	public class SKHero{
		public string ID = "HR_00001";
		public int STAR = 1;
		public string ARMY = "ARROW";
		public string NAME = "1성히어로";
		public bool SALE = true;
		public string EARN_FROM = "술집";
		public int HEALTH = 100;
		public int ATTACK = 100;
		public int DEFENSE = 100;
		public int CRITICAL = 100;
		public int DODGE = 100;
		public string SKILL1 = "SK_00001";
		public string SKILL2 = "SK_00002";
		public string DESC = "설명";
	};

	public class SKSkillLevelStat{
		public int LEVEL = 2;
		public int[] STAT_VALUE;
	};

	public class SKSkill{
		public string ID = "SK_00001";
		public string NAME = "스킬이름";
		public string DESC = "공격시 {V1%}의 확률로 발동되며 자신의 주위 일정 범위내에 병사들의 공격력을 10초동안 {V2%}상승시킨다.";
		public SKSkillLevelStat	LEVEL_STAT;
	};

	public class SKStageReward{
		public string KIND = "ITEM";
		public string ID = "IT_00001";
		public int COUNT = 1;
		public int COUNT_SUB = 20;
	};

	public class SKStageRally{

	};

	public class SKStages{
		public int STAGE_LEVEL = 0;
		public string ID = "N_001";
		public string NAME = "Stage 01";
		public int DAILY_LIMIT = 100;
		public int LEVEL = 5;
		public int EXP = 1000;
		public int GOLD = 3000;
		public int HERO_EXP = 1500;
		public SKStageReward REWARD;
		public SKStageRally RALLY;
	};

	public class SKStarReward{

	};

	public class SKStage{
		public int STAGE_SET = 1;
		public string NAME = "전초기지";
		public SKStages STAGES;
		public SKStarReward STAR_REWARD;
	};
}


public class SKCommon : MonoBehaviour {

	public static MainMananger m_MainManager;

	public static GameObject loadPrefeb(string loadName, GameObject insertPanel)
	{
		Debug.Log("loadPrefeb : "+"prefabs/"+loadName);
		GameObject pf = Resources.Load ("prefabs/"+loadName) as GameObject;
		
		GameObject Obj = GameObject.Instantiate (pf) as GameObject;
		Obj.transform.parent = insertPanel.transform;// insertPanel.GetComponentInParent<UIPanel> ().gameObject.transform;
		Obj.transform.localPosition = insertPanel.transform.localPosition;
		Obj.transform.localRotation = Quaternion.identity;
		Obj.transform.localScale = new Vector3 (1, 1, 1);
		Obj.SetActive (true);
		
		return Obj;
	}

//	public static void onClicked()
//	{
//		GameObject popup = GameObject.FindGameObjectWithTag("popupController");
//		if (popup) {
//			PopupManager p = popup.GetComponent<PopupManager> ();
//			p.startPopup (PopState.BUY_SHOP, null);
//			p._onPopupBtn += (int type, int tag) => {
//				p.onDestory ();
//			};
//		}
//	}

}