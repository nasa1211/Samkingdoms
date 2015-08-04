using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum PlayerPosition{
	Hero,
	Enemy,
};
public enum GameState{
	ready,
	moveArmy,
	idle,
	gameOver,
	wait,
	loading,
};
public enum TargetState{
	Complete,
	Parent,
	targetParent,
};

public class InGameManager : MonoBehaviour {

	public GameState nowGameState = GameState.moveArmy;
	// 적 체력 표시 유저 인터페이스 생성에 사용한다.
	public GameObject PlayerHPBar;
	public Transform PlayerHPBarRoot;
	// 적 체력 표시 유저 인터페이스 할당에 사용한다.
	public UIPanel PlayerHPBarPanel;
	public Camera PlayerHPBarCam;
	// 게임 오브젝트 풀에 들어가는 게임 오브젝트의 최초로 생성되는 위치.
	public Transform HeroObjPoolPosition;
	public Transform EnemyObjPoolPosition;

	public List<GameObject> spwanPlayerObjs =new List<GameObject>();

	public Dictionary<string, Player> gameSoldierPools = new Dictionary<string, Player>();
	public Dictionary<PlayerPosition, List<General>> gameGeneralPools = new Dictionary<PlayerPosition, List<General>>();

	public GameObject posiEmptyOpj;

	Dictionary<int, int> positionToAmountHeros = new Dictionary<int, int> {
		{0,25},{1,25},{2,25},{3,25},
		{4,25},{5,25},{6,25},{7,25},
		{8,25},{9,25},{10,25},{11,25},
		{12,25},{13,25},{14,25},{15,25},
		{16,25},{17,25},{18,25},{19,25},
		{20,25},{21,25},{22,25},{23,25},
		{24,25},{25,25},{26,25},{27,25}

//		{11,25},{15,25},{19,25},
//		{11,25},{15,10}
//		{15,25}
//		{11,25},{15,25}
//		{15,25}

//		{10,25},{11,25},
//		{14,25},{15,25},
//		{18,25},{19,25},

	};
	Dictionary<int, int> positionToAmountEnemys = new Dictionary<int, int> {
		{0,25},{1,25},{2,25},{3,25},
		{4,25},{5,25},{6,25},{7,25},
		{8,25},{9,25},{10,25},{11,25},
		{12,25},{13,25},{14,25},{15,25},
		{16,25},{17,25},{18,25},{19,25},
		{20,25},{21,25},{22,25},{23,25},
		{24,25},{25,25},{26,25},{27,25}

//		{11,25},{15,25},{19,25},
//		{11,10},{15,25}
//		{15,25}
//		{11,25},{15,25}
//		{15,25}

//		{10,25},{11,25},
//		{14,25},{15,25},
//		{18,25},{19,25},

	};

	List<SoliderBuffer> emptyBuf = new List<SoliderBuffer>();

	public int MaxDisposeCount = 28;

	int[,] gArray = new int[,]{
		{0,1,2,3},
		{4,5,6,7},
		{8,9,10,11},
		{12,13,14,15},
		{16,17,18,19},
		{20,21,22,23},
		{24,25,26,27}
	};

	void Awake()
	{
		GameData.Instance.inGameMananger = this;

		GameObject.Find("Btn_Plus").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			PlayerHPBarCam.orthographicSize -= 1f;
		} ));;
		GameObject.Find("Btn_Minus").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			PlayerHPBarCam.orthographicSize += 1f;
		} ));;
		GameObject.Find("Btn_Reset").GetComponent<UIButton>().onClick.Add (new EventDelegate( ()=>{
			if(gameSoldierPools.Count == 0){
				OnEnable();
			}
		} ));;
	}
	
	void OnDestroy()
	{
		GameData.Instance.inGameMananger = null;
	}
	
	void OnEnable()
	{
		CreateGeneralObject(PlayerPosition.Hero, HeroObjPoolPosition);
		CreateGeneralObject(PlayerPosition.Enemy, HeroObjPoolPosition);

//		int[,] nums = new int[3,2]{{9,99},{3,33},{5,55}};
//		foreach (int idx in nums) {
//			Debug.Log("---> "+idx+", "+nums.GetLength(1));
//		}

		if(gameGeneralPools.ContainsKey(PlayerPosition.Hero)){
			foreach(General _g in gameGeneralPools[PlayerPosition.Hero]){
				for(int i=0;i<gArray.GetLength(0);i++){
					for(int j=0;j<gArray.GetLength(1);j++){
						Debug.Log("arr["+i+"]["+j+"] = "+gArray[i, j]);
					}
				}
			}
		}
		if(gameGeneralPools.ContainsKey(PlayerPosition.Enemy)){
			Debug.Log("2222");
		}

//		//check target General
//		setGeneralTarget();
//
//		//game start
//		Invoke("gameStart", 3f);
	}			

	void gameStart(){
		foreach(PlayerPosition _k in gameGeneralPools.Keys){
			foreach(General _g in gameGeneralPools[_k]){
				if(_g.target == null)
					continue;

				foreach(Player _p in _g.soldierObj){
					_p.setMoveStartDelay(Random.Range(0.01f, 0.05f));
				}
			}
		}

//		Player hero1 = GameObject.Find("Hero_15_0").GetComponent<Player>();
//		Player hero2 = GameObject.Find("Enemy_15_0").GetComponent<Player>();
//
////		hero1.setTargetObj(hero2);
//		hero2.setTargetObj(hero1);
//		
////		hero1.setPlayerState(PlayerState.Move_target);
//		hero2.setPlayerState(PlayerState.Move_target);

	}

	void setGeneralTarget(){

		Debug.Log("00 ========================== start");

		foreach(PlayerPosition key in gameGeneralPools.Keys)
		{
//			emptyGeneralTarget(key, true);
#if true
//			PlayerPosition pState;
			foreach(General mGe in gameGeneralPools[key])
			{
				emptyGeneralTarget(mGe, true);
//				if(mGe.target == null){
//
//					pState = (key == PlayerPosition.Hero) ? PlayerPosition.Enemy : PlayerPosition.Hero;
//					float tmpDist = 0, saveDist = 10000;
//					General temp = null;
//					bool check = false;
//					foreach(General _tg in gameGeneralPools[pState]){
//
////						tmpDist = Vector3.Distance(mGe.transform.position, _tg.transform.position);
////						tmpDist = Vector3.Distance(mGe.soldierObj[0].transform.position, _tg.soldierObj[0].transform.position);
//						foreach(Player _p1 in mGe.soldierObj){
//							foreach(Player _p2 in _tg.soldierObj){
//								tmpDist = Vector3.Distance(_p1.transform.position, _p2.transform.position);
//								check = true;
//								break;
//							}
//						}
//
//						Debug.Log("22 ===> "+_tg.name+", ("+tmpDist+" < "+saveDist+")");
//
//						if(_tg.target == null && tmpDist < saveDist){
//							saveDist = tmpDist;
//							temp = _tg;
//						}
//					}
//
//					if(check && temp != null){
//						mGe.target = temp.transform;
//						temp.target = mGe.transform;
//					}
//				}
			}
#endif
		}
	}

	bool emptyGeneralTarget(General general, bool bFirst)
	{
		bool checkGeneral = false;
		if(general == null || general.soldierObj.Count == 0)
			return checkGeneral;

		if(general.target == null || !bFirst)
		{
			PlayerPosition pState = (general.generalPosition == PlayerPosition.Hero) ? PlayerPosition.Enemy : PlayerPosition.Hero;
			float tmpDist = 0, saveDist = 10000;
			General temp = null;
			bool check = false;
			foreach(General _tg in gameGeneralPools[pState])
			{
				foreach(Player _p1 in general.soldierObj)
				{
					foreach(Player _p2 in _tg.soldierObj)
					{
						tmpDist = Vector3.Distance(_p1.transform.position, _p2.transform.position);
						check = true;
						break;
					}
				}
//				Debug.Log("22 ===> "+_tg.name+", ("+tmpDist+" < "+saveDist+")");
				if(_tg.target == null && tmpDist < saveDist)
				{
					saveDist = tmpDist;
					temp = _tg;
				}
			}
			if(check && temp != null)
			{
				general.target = temp.transform;
				temp.target = general.transform;
				checkGeneral = true;
			}
		}

		return checkGeneral;

//		if(gameGeneralPools[key].Count == 0)
//			return false;
//
//		PlayerPosition pState;
//		bool checkGeneral = false;
//		foreach(General mGe in gameGeneralPools[key])
//		{
//			if(mGe.target == null || !bFirst){
//				
//				pState = (key == PlayerPosition.Hero) ? PlayerPosition.Enemy : PlayerPosition.Hero;
//				float tmpDist = 0, saveDist = 10000;
//				General temp = null;
//				bool check = false;
//				foreach(General _tg in gameGeneralPools[pState]){
//					
//					//tmpDist = Vector3.Distance(mGe.transform.position, _tg.transform.position);
//					//tmpDist = Vector3.Distance(mGe.soldierObj[0].transform.position, _tg.soldierObj[0].transform.position);
//					foreach(Player _p1 in mGe.soldierObj){
//						foreach(Player _p2 in _tg.soldierObj){
//							tmpDist = Vector3.Distance(_p1.transform.position, _p2.transform.position);
//							check = true;
//							break;
//						}
//					}
//					
//					Debug.Log("22 ===> "+_tg.name+", ("+tmpDist+" < "+saveDist+")");
//
//					if(_tg.target == null || !bFirst){
//						if(tmpDist < saveDist){
//							saveDist = tmpDist;
//							temp = _tg;
//						}
//					}
//
//				}
//				
//				if(check && temp != null){
//					mGe.target = temp.transform;
//					temp.target = mGe.transform;
//					checkGeneral = true;
//				}
//			}
//		}
//
//		return checkGeneral;
	}

	void CreateGeneralObject(PlayerPosition playerPosi, Transform objPool)
	{
		float m_space = 9f;//11f;
		bool facingRight = (playerPosi == PlayerPosition.Hero)?true:false;
		int loadPrefabIndex = (playerPosi == PlayerPosition.Hero)?0:1;
		
		float m_x = objPool.position.x+(facingRight?-m_space:m_space-0.3f);
		float m_y = objPool.position.y+4.5f;
		float m_index=0, m_line=0;

		Dictionary<int,int> tmpDic = (playerPosi == PlayerPosition.Hero)? positionToAmountHeros : positionToAmountEnemys;
		List<General> tmpGeneral = new List<General>();

		for(int i=0;i<MaxDisposeCount;i++)
		{
			m_index = i%4;
			if(i%4 == 0 && i != 0) m_line++;
//			float mainX = m_x+(m_line*0.45f);
			float mainX = m_x+(m_line*0.72f);
			float mainY = m_y-(m_line*1.5f);

			if(tmpDic.ContainsKey(i) == false)
				continue;
			int soldierCount = tmpDic[i];

			GameObject positionObj = Instantiate(posiEmptyOpj, objPool.position, Quaternion.identity) as GameObject;
			positionObj.transform.parent = objPool;
			positionObj.name = (playerPosi == PlayerPosition.Hero)?"Hero_"+i:"Enemy_"+i;
			positionObj.SetActive(true);
			positionObj.transform.localPosition = new Vector3(mainX+(facingRight?(m_index*2.5f):-(m_index*2.5f)),
			                                                  mainY, objPool.position.z);

			//add General Pools
			General _general = positionObj.GetComponent<General>(); 
			_general.generalPosition = playerPosi;
			_general.index = i;
			tmpGeneral.Add(_general);

			//add Soldier
			//gameObjPools.Add(positionObj.name, positionObj);
			Vector3 generalPos = positionObj.transform.localPosition;
			if(playerPosi == PlayerPosition.Enemy){
				generalPos.x = generalPos.x-2.1f;
			}
			createSolider(_general, loadPrefabIndex, positionObj, generalPos, i, 0, 0);

			//sub player
			float s_x = positionObj.transform.position.x-0.5f;
			float s_y = positionObj.transform.position.y+0.8f;
			float defaulSpaceW = 0.25f;
			float defaulSpaceH = 0.5f;
			float defaulSpaceL = 0.6f;
			float s_nx=0, s_index=0;
			int s_line=0;

			//Dictionary<int, object> soldiersDic = new Dictionary<int, object>();

			for(int sol=0;sol<soldierCount;sol++)
			{
				s_index = sol%5;
				if(sol%5 == 0 && sol != 0) s_line++;
				s_nx = s_x+(facingRight?-(s_line*defaulSpaceL):(s_line*defaulSpaceL));

				Vector3 pos = new Vector3(														  
				                          s_nx+(defaulSpaceW*s_index), 
				                          s_y-(defaulSpaceH*s_index),
				                          positionObj.transform.position.z);

				createSolider(_general, loadPrefabIndex, positionObj, pos, i, sol+1, s_line);
			}
		}

		if(tmpGeneral.Count > 0){
			gameGeneralPools.Add(playerPosi, tmpGeneral);
		}
	}

	void createSolider(
		General general,
		int loadPrefabIndex, 
		GameObject positionObj,
		Vector3 pos,
		int groupIdx,
		int soliderIdx,
		int solideLine
		)
	{
		GameObject obj = Instantiate(spwanPlayerObjs[loadPrefabIndex], positionObj.transform.position, Quaternion.identity) as GameObject;
		obj.transform.parent = positionObj.transform;
		obj.transform.localPosition = pos;
//		obj.name = positionObj.name+"_soldier_" + soliderIdx;
		obj.name = positionObj.name+"_" + soliderIdx;

		//boss
		if(soliderIdx == 0){
			obj.transform.localScale = new Vector3((loadPrefabIndex==0?1.2f:-1.2f),1.2f,0f);
		}
		

		Player pp = obj.GetComponent<Player>();
		pp.group = groupIdx;
		pp.index = soliderIdx;
//		pp.sKey = positionObj.name+"_"+soliderIdx;
		pp.startDelayTime = solideLine*Random.Range(0.1f, 0.12f);
		pp.currentState = PlayerState.None;

		pp.rangeSize = obj.transform.localScale.y * 2f;
//		Debug.Log(this.name+", "+pp.rangeSize+" : "+obj.transform.localScale.y);


//		float maxhp = Random.Range(0.5f, 2f);
//		float maxhp = loadPrefabIndex==0?Random.Range(1f, 1.5f):Random.Range(1.1f, 1.4f);
//		float maxhp = loadPrefabIndex==0? 10f : 0.3f;
		float maxhp = pp.index==0? 3f : Random.Range(0.5f, 1f);

		pp.InitPlayer (maxhp,//maxHP
		               //Random.Range(0.1f, 0.5f),
		               0.1f,//damage
		               2.0f);//moveSpeed
		GameObject currentEnemyHPBar;
		currentEnemyHPBar = Instantiate(PlayerHPBar, positionObj.transform.position, Quaternion.identity) as GameObject;
		
		currentEnemyHPBar.transform.parent = PlayerHPBarRoot;
		currentEnemyHPBar.transform.localScale = Vector3.one;
		currentEnemyHPBar.name = positionObj.name+"_hp_"+soliderIdx;
		// 적 체력 표시 인터페이스 할당.
		UISlider tempEnemyHPBarSlider = currentEnemyHPBar.GetComponent<UISlider>();
		pp.InitHPBar(tempEnemyHPBarSlider, PlayerHPBarPanel, PlayerHPBarCam); 

		general.soldierObj.Add(obj.GetComponent<Player>());
//		gameSoldierPools.Add(pp.name, obj.GetComponent<Player>());
	}

	//void LateUpdate()
	void FixedUpdate()
	//void Update()
	{
		if(nowGameState == GameState.idle){

			for(int i=gameGeneralPools.Count-1;i>=0;i--)
			{
				List<General> gList = gameGeneralPools[i==0?PlayerPosition.Hero:PlayerPosition.Enemy];
				for(int j=gList.Count-1;j>=0;j--)
				{
					General _g = gList[j];
					for(int k=_g.soldierObj.Count-1;k>=0;k--)
					{
						Player _p = _g.soldierObj[k];

						if(nowGameState == GameState.idle)
						{
							//================================================//
							for(int _c=_p.targetObjPools.Count-1;_c>=0;_c--){
								Player _tmp = _p.targetObjPools[_c];
								if(_tmp == null){
									Debug.Log("Missing !!!@!@! Delete!!!   :  _p.targetObjPools["+_c+"]");
									_p.targetObjPools.RemoveAt(_c);
								}
							}
							//================================================//

//							if(_p.soldierFlag == PlayerPosition.Hero)
//								Debug.Log("==========> "+_p.currentState+", "+_p.aniState);

							if(_p.currentState == PlayerState.Move_target)
							{
								if(_p.targetObjPools.Count == 0 && _p.aniState != PlayerAnimation.IDLE){
									_p.setPlayerAnimation(PlayerAnimation.IDLE);
									return;
								}
							}
							else if(_p.currentState == PlayerState.SearchGeneral)
							{
								if(_g.getTargetGeneral() == null){
									//Next General Search
									Debug.Log("Next General Search");
								}
								else{
									if(_g.getTargetGeneral().soldierObj.Count == 0){
										Destroy(_g.target.gameObject);
										_g.target = null;
									}
									else{
										Debug.Log("SearchGeneral : name : "+_p.name);
//										Player nearPlayer = _g.getTargetGeneral().getNearingSoldier(_p);
//										if(nearPlayer != null){
//											_p.setTargetObj(nearPlayer);
//											if(_p.targetObjPools.Count == 0){
//												_p.setTargetRange();
//												_p.setPlayerState(PlayerState.Move_target);
//											}
//
//											nearPlayer.setTargetObj(_p);
//											nearPlayer.setTargetRange();
//											nearPlayer.setPlayerState(PlayerState.Move_target);
//										}
									}
								}
								
							}
							else if(_p.currentState == PlayerState.SearchSoldier)
							{
								if(_p.aniState != PlayerAnimation.IDLE)
									_p.setPlayerAnimation(PlayerAnimation.IDLE);

								if(_p.targetObjPools.Count > 0){

									if(_p.getTarget()){
										_p.setTargetRange();
										_p.setPlayerState(PlayerState.Move_target);
									}
									else{
										_p.targetObjPools.RemoveAt(0);
									}
								}
								else
								{
									Player targetSoldier = _g.getTargetGeneral().getNearingSoldier(_p);
									if(targetSoldier != null){

										_p.setTargetObj(targetSoldier);
										_p.setTargetRange();
										_p.setPlayerState(PlayerState.Move_target);

										targetSoldier.setTargetObj(_p);

										Debug.Log("New Target !!!! "+_p.name+", "+_p.currentState+" ==> "+targetSoldier.name+", "+targetSoldier.currentState);

										if(targetSoldier.targetObjPools.Count == 2 && targetSoldier.facingRight != _p.facingRight)
											_p.targetPassMove = true;

									}
								}

							}
							else if(_p.currentState == PlayerState.Attack)
							{
								if(_p.targetObjPools.Count == 0){
									_p.setPlayerState(PlayerState.SearchSoldier);
									return;
								}

								if(_p.targetObjPools[0].currentHP == 0)
								{
									Player deadPlayer = _p.targetObjPools[0];
									Destroy(deadPlayer.gameObject);
									deadPlayer.currentState = PlayerState.Dead;
									deadPlayer.setPlayerAnimation(PlayerAnimation.IDLE);
									deadPlayer.targetObjPools.RemoveAt(0);

									List<General> tmp;
									if(_g.generalPosition == PlayerPosition.Hero){
										tmp = gameGeneralPools[PlayerPosition.Enemy];
									}
									else{
										tmp = gameGeneralPools[PlayerPosition.Hero];
									}
									foreach(General tg in tmp){
										foreach(Player tp in tg.soldierObj){
											if(deadPlayer == tp){
												Debug.Log(_p.name+" ==> 00 DELETE :: "+deadPlayer.name+" == "+tp.name);
												tg.soldierObj.Remove(tp);
												break;
											}
										}
									}

									_p.targetObjPools.RemoveAt(0);
									_p.setPlayerState(PlayerState.SearchSoldier);
								}
							}
							else if(_p.currentState == PlayerState.Dead)
							{
							}
							else if(_p.currentState == PlayerState.None)
							{
							}
						}//if


					}
				}
			}


//			foreach(PlayerPosition _k in gameGeneralPools.Keys){
//				foreach(General _g in gameGeneralPools[_k]){
//					
////					if(_g.soldierObj.Count > 0){
//					if(_g.getTargetGeneral() && _g.getTargetGeneral().soldierObj.Count > 0){
//						
//						foreach(Player _p in _g.soldierObj){
//							
//							if(nowGameState == GameState.idle)
//							{
//								if(_p.currentState == PlayerState.SearchGeneral)
//								{
//									//Next General Search
//
//								}
//								else if(_p.currentState == PlayerState.Move_target)
//								{
//									if(_p.targetObjPools.Count == 0){
//										Debug.Log("@@@@@@@@@@@@@@@@@@@@ Move_target : "+_p.name);
//									}
//								}
//								else if(_p.currentState == PlayerState.SearchSoldier)
//								{
//									_g.soldiersTargetSettings();
//									if(_p.targetObjPools.Count > 0){
//										_p.setPlayerState(PlayerState.Move_target);
//									}
////									else{
////										Debug.Log("Next GeneralTarget Serch!!!");
////										_p.setPlayerState(PlayerState.SearchGeneral);
////										return;
////									}
//								}
//								else if(_p.currentState == PlayerState.Attack)
//								{
//									Debug.Log("Att : "+_p.name+", T:"+_p.targetObjPools.Count+", target:"+_p.targetObjPools[0]);
//
//									if(_p.targetObjPools[0].currentState == PlayerState.Dead)
//									{
//										if(_p.targetObjPools[0] != null)
//										{
//											Debug.Log("Dead Name : "+_p.targetObjPools[0].name);
//											
//											//Target Delete
//											Destroy(_p.targetObjPools[0].hpBarSlider.gameObject);
//											Destroy(_p.targetObjPools[0].gameObject);
//											_g.getTargetGeneral().soldierObj.Remove(_p.targetObjPools[0]);
//										}
//										_p.targetObjPools.RemoveAt(0);
//
//										if(_p.targetObjPools.Count > 0)
//										{
//											_p.flipTarget();
//										}
//										else
//										{
//											Debug.Log("Next SoldierTarget Serch!!!");
//											_p.setPlayerState(PlayerState.SearchSoldier);
//										}
//
//										//Next Target Check
//										_g.getTargetGeneral().soldiersTargetSettings();
//										foreach(Player _tp in _g.getTargetGeneral().soldierObj){
//											if(_tp.currentState == PlayerState.None){
//												Debug.Log("#@#$@#$@#$@#$ ::: "+_p.facingRight+" == "+_tp.facingRight);
//												_tp.setPlayerState(PlayerState.Move_target);
//											}
//										}
//									}
//
//								}
//								else if(_p.currentState == PlayerState.Dead)
//								{
////									_g.soldierObj.Remove(_p);
////									Destroy(_p.hpBarSlider.gameObject);
////									Destroy(_p.gameObject);
//								}
//								else if(_p.currentState == PlayerState.None)
//								{
//									if(_g.target)
//									{
//									}
//									else
//									{
//										
//										if(_k == PlayerPosition.Hero)
//										{
//											if(gameGeneralPools[PlayerPosition.Enemy].Count > 0)
//											{
//												Debug.Log("Hero Next General !!!!!!!!!!....."+gameGeneralPools[PlayerPosition.Enemy].Count);
//											}
//											else
//											{
//												Debug.Log("game over (WiN)");
//											}
//										}
//										else
//										{
//											if(gameGeneralPools[PlayerPosition.Hero].Count > 0)
//											{
//												Debug.Log("Enemy Next General !!!!!!!!!!....."+gameGeneralPools[PlayerPosition.Hero].Count);
//											}
//											else
//											{
//												Debug.Log("game over (LOSE)");
//											}
//										}
//										
//									}
//								}
//							}//if
//							
//						}
//						
//					}
//					else
//					{
//						//delete general
//						if(_g.getTargetGeneral() != null){
//							Destroy(_g.getTargetGeneral().gameObject);
////							gameGeneralPools[_k].Remove(_g.getTargetGeneral());
//							_g.target = null;
//							_g.soldierStateChange(PlayerState.None);
//							Debug.Log("$%$%$%$%$%$%$% Delete General--!!!!");
//						}
//					}
//					
//				}
//			}

		}


//		renderingSortOrder();

//		if(Input.GetKeyDown("left")){
//		}
//		if(Input.GetKeyDown("right")){
//		}
//		if(Input.GetKeyDown("down")){
//		}

	} 

	public void targetDelete(Player _p){
		if(_p.getTarget()){
			Player _tp = _p.getTarget().GetComponent<Player>();
			_p.delTarget();
			gameSoldierPools.Remove(_tp.name);
			
			Destroy(_tp.hpBarSlider.gameObject);
			Destroy(_tp.gameObject);
			
			if(_p.transform.parent.childCount == 0){
				Debug.Log("11 Genenal Delete!!!");
				Destroy(_p.transform.parent.gameObject);
				gameGeneralPools.Remove(_p.soldierFlag);
			}
		}
	}

#if false
	public void playerDead(Player _p)
	{
		if(_p.getTarget())
			_p.getTarget().GetComponent<Player>().delTarget();

		gameSoldierPools.Remove(_p.name);
//		Destroy(_p.gameObject);
//		DestroyImmediate(_p.gameObject);
		DestroyObject(_p.gameObject);

		//group delete
		Debug.Log("00 Genenal Delete Check ("+_p.transform.parent.childCount+"), Del Name("+_p.name+")");
//		if(_p.transform.parent.childCount <= 1){
		if(_p.transform.parent.childCount == 0){
			Debug.Log("11 Genenal Delete!!!");
			Destroy(_p.transform.parent.gameObject);
			gameGeneralPools.Remove(_p.soldierFlag);
		}
	}
#endif

	public void nextTarget(Player _p)
	{
//		StartCoroutine(emptyGeneralSerch(_p, _p.target.transform.parent.gameObject));
//		emptyGeneralSerch(_p, _p.target.transform.parent.gameObject);
//		_p.target = null;

#if true
		targetDelete(_p);
		emptyBuf.Add(new SoliderBuffer(){_p=_p, targetGeneralName=""/*_p.getTarget().transform.parent.name*/});
#else
		emptyBuf.Add(new SoliderBuffer(){_p=_p, targetGeneralName=_p.target.transform.parent.name});
		_p.target = null;
#endif
//		emptyGeneralSerch();
//		Invoke("emptyGeneralSerch", 0.3f);

//		targetSolider(_p.transform.parent.gameObject, _p.target.transform.parent.gameObject);
	}

//	void emptyGeneralSerch(){
////	void emptyGeneralSerch(Player _p, GameObject _mtp){
////	IEnumerator emptyGeneralSerch(Player _p, GameObject _mtp){
////		yield return new WaitForSeconds(0.1f);
//
//		if(bNextSolider){
//			return;
//		}
//		if(emptyBuf.Count == 0)
//			return;
//
//		Player _p = emptyBuf[0]._p;
//		GameObject targetParent = null;// = GameObject.Find(emptyBuf[0].targetGeneralName);
//
//		if(_p.currentState == PlayerState.Dead)
//			return;
//
//		foreach(General gen in gameGeneralPools[_p.soldierFlag]){
//			if(_p.transform.parent == gen.transform){
//				targetParent = gen.target.gameObject;
//				break;
//			}
//		}
//
////		if(_p == null)
////			return;
//
//		bNextSolider = true;
//
////		Debug.Log("emptyGeneralSerch : "+targetParent.transform.childCount+", name : "+targetParent.name+" Player : "+_p.sKey);
////		targetSolider(_p.transform.parent.gameObject, targetParent, true);
//
//		if(targetParent != null){
////			bool bNextTarget = true;
//			float tmpDist = 0, saveDist=10000;
//			Player saveObj = null, tmpObj = null;
////			List<Player> saveTargetArr = new List<Player>();
//
//			Debug.Log("00 emptyGeneralSerch-----> "+_p.targetObjPools.Count+",,,,,,,,,,,"+emptyBuf.Count+", bNextSolider : "+bNextSolider+", general : "+targetParent.transform.childCount);
//
//			//장수의 타겟설정
//			if(_p.getTarget()){
//				_p.flipTarget();
//				coliderNextEnd();
//				Debug.Log("11 emptyGeneralSerch-----> "+_p.targetObjPools.Count+",,,,,,,,,,,"+emptyBuf.Count+", bNextSolider : "+bNextSolider+", general : "+targetParent.transform.childCount);
//			}
//			else{
//				for(int i=0;i<targetParent.transform.childCount;i++){
//					tmpObj = targetParent.transform.GetChild(i).GetComponent<Player>();
//					if(_p != null && tmpObj != null){
//						//						Debug.Log("111 : "+_p);
//						//						Debug.Log("222 : "+tmpObj);
//						tmpDist = Vector3.Distance(_p.transform.position, tmpObj.transform.position);
//						if(tmpObj.getTarget() == null && !checkEqulTarget(_p.transform.parent, tmpObj.transform)){
//							if(tmpDist < saveDist){
//								saveDist = tmpDist;
//								saveObj = null;
//								saveObj = tmpObj;
//							}
//						}
//					}
//				}
//				
//				if(saveObj != null){
//					_p.setTargetObj(saveObj);
//					_p.setTargetRange();
//					_p.setPlayerState(PlayerState.Move_target);
//					
//					saveObj.setTargetObj(_p);
//					saveObj.setTargetRange();
//					saveObj.setPlayerState(PlayerState.Move_target);
//					
//					coliderNextEnd();
//					Debug.Log("22 emptyGeneralSerch-----> "+_p.targetObjPools.Count+",,,,,,,,,,,"+emptyBuf.Count+", bNextSolider : "+bNextSolider+", general : "+targetParent.transform.childCount);
//				}
//			}
//
//			//상대장수의 타겟설정
//			for(int i=0;i<targetParent.transform.childCount;i++){
//				tmpObj = targetParent.transform.GetChild(i).GetComponent<Player>();
//				if(tmpObj.currentState == PlayerState.None){
//					if(_p != null && _p.targetObjPools.Count < 2){
//						_p.setTargetObj(tmpObj);
//						
//						tmpObj.setTargetObj(_p);
//						tmpObj.setTargetRange();
//						tmpObj.setPlayerState(PlayerState.Move_target);
//
//						Debug.Log("#@#@#@# : "+tmpObj.name+",,,,,,,,,,"+_p.name);
//
//					}
//				}
//			}
//
//			Debug.Log("33 emptyGeneralSerch-----> "+_p.targetObjPools.Count+",,,,,,,,,,,"+emptyBuf.Count+", bNextSolider : "+bNextSolider+", general : "+targetParent.transform.childCount);
//			if(bNextSolider){
//				Debug.Log("RE Check ~!!!!!!");
//
//				if(targetParent.transform.childCount > 0){
//					Invoke("reCheck", 0.1f);
//				}
//				else{
//					bNextSolider = false;
//					for(int i=emptyBuf.Count-1;i>=0;i--){
//						emptyBuf.RemoveAt(i);
//					}
//					Debug.Log("44 emptyGeneralSerch-----> "+_p.targetObjPools.Count+",,,,,,,,,,,"+emptyBuf.Count+", bNextSolider : "+bNextSolider+", general : "+targetParent.transform.childCount);
//
//					//Todo :: Next General Check
//				}
//
//			}
//		}
//		else{
//			//Todo : 부대가 전멸이면 다음 부대를 찾는다.
//		}
//	}
//	void reCheck(){
//		bNextSolider = false;
//		emptyGeneralSerch();
//	}
//	void coliderNextEnd(){
//		emptyBuf.RemoveAt(0);
//		bNextSolider = false;
//		Debug.Log("coliderNextEnd ### ? "+emptyBuf.Count);
//		if(emptyBuf.Count > 0)
//			emptyGeneralSerch();
//	}
//	bool checkEqulTarget(Transform parent, Transform target){
//		for(int i=0;i<parent.childCount;i++){
//			Player _p = parent.GetChild(i).GetComponent<Player>();
//			if(_p.getTarget() == target)
//				return true;
//		}
//		return false;
//	}
//	IEnumerator emptyGeneralSerchImpl(Player _p, bool bAttack){
//		yield return new WaitForSeconds(0.1f);
//		if(_p.getTarget() != null){
//			Debug.Log("emptyGeneralSerch : "+_p.name+", "+_p.transform.parent.name+", ㅌㅏㄱㅔㅅ : "+_p.getTarget().name);
//			if(bAttack){
//				_p.checkFlip();
//				_p.setPlayerState(PlayerState.Attack);
//			}
//			else{
//				_p.setPlayerState(PlayerState.Move_target);
//			}
//		}
//	}
//	IEnumerator emptyGeneralSerchImpl2(Player _p, bool bAttack){
//		yield return new WaitForSeconds(0.1f);
//		if(_p.getTarget() != null){
//			if(bAttack){
//				_p.checkFlip();
//				_p.setPlayerState(PlayerState.Attack);
//			}
//			else{
//				_p.setPlayerState(PlayerState.Move_target);
//			}
//		}
//	}

	void renderingSortOrder(){
		if(gameSoldierPools.Count > 1)
		{
			float temp=0,len=0,order = 0;
			float[] tmpList = new float[gameSoldierPools.Count];
			string[] strList = new string[gameSoldierPools.Count];
			foreach(var key in gameSoldierPools.Keys){
				strList[(int)order] =  key;
				tmpList[(int)order] = gameSoldierPools[key].transform.localPosition.y;
				order++;
			}
			string stmp;
			len = gameSoldierPools.Count-1;
			for(int i=0;i<len;i++){
				for(int j=0;j<len-i;j++){
					if(tmpList[j] < tmpList[j+1]){
						temp = tmpList[j+1];
						tmpList[j+1] = tmpList[j];
						tmpList[j] = temp;
						
						stmp = strList[j+1];
						strList[j+1] = strList[j];
						strList[j] = stmp;
					}
				}
			}
			
			order = len = 0;
			for(int i=0;i<gameSoldierPools.Count;i++){
				if(i == 0){
					len = tmpList[i];
				}
				else{
					if(tmpList[i] != len){
						len = tmpList[i];
						order++;
					}
				}
				gameSoldierPools[strList[i]].GetComponent<SpriteRenderer>().sortingOrder = (int)order;
			}
		}
	}

	public void checkGeneralCollision(GameObject Parent, GameObject targetParent){

		General _g1 = Parent.GetComponent<General>();
		General _g2 = targetParent.GetComponent<General>();

		if(_g1.soldierTargetCheck == false){
			//GameState Change
			nowGameState = GameState.wait;

			_g1.soldierStateChange(PlayerState.None);
			_g2.soldierStateChange(PlayerState.None);

			_g1.soldiersTargetSettings();
			_g2.soldiersTargetSettings();
			StartCoroutine(targetSoliderCheckEnd(_g1, _g2));
		}
		else{
			Debug.Log("_g1.soldierTargetCheck ??? "+_g1.soldierTargetCheck);
		}

//		//부대 멈춤
//		soliderStateChange(Parent, PlayerState.None);
//		soliderStateChange(targetParent, PlayerState.None);
//
//		//타겟 설정
//		targetSolider(Parent, targetParent, false);
//		//Check End
//		StartCoroutine(targetSoliderCheckEnd(Parent, targetParent));
	}

//	public void soliderStateChange(GameObject general, PlayerState state){
//		for(int i=0;i<general.transform.childCount;i++){
//			Player _p = general.transform.GetChild(i).GetComponent<Player>();	
//			_p.bFirst = true;
//			if(state == PlayerState.Move_target){
//				if(_p.index == 0){
//					_p.setPlayerState(PlayerState.Attack);
//				}
//				else{
//					if(_p.getTarget() != null)
//						_p.setPlayerState(state);
//					else
//						_p.setPlayerState(PlayerState.None);
//				}
//			}
//			else{
//				_p.setPlayerState(state);
//			}
//		}
//	}

//	public bool checkSoliderTarget(GameObject general, bool frontAndRear){
//		for(int i=0;i<general.transform.childCount;i++){
//			Player _p = general.transform.GetChild(i).GetComponent<Player>();
//
////			if(frontAndRear){
////				if(!_p.getFront() || !_p.getRear()){
////					return true;
////				}
////			}
////			else{
////				if(_p.getTarget() == null){
////					return true;
////				}
////			}
//
//			if(frontAndRear){
//				if(_p.targetObjPools.Count < 2){
//					return true;
//				}
//			}
//			else{
//				if(_p.targetObjPools.Count == 0){
//					return true;
//				}
//			}
//
//		}
//		return false;
//	}

//	public bool searchTargetGeneral(GameObject general){
//		for(int i=0;i<general.transform.childCount;i++){
//			Player _p = general.transform.GetChild(i).GetComponent<Player>();
//			if(_p.index == 0){//장수
//				return true;
//			}
//		}
//		return false;
//	}

//	public void targetSolider(GameObject Parent, GameObject targetParent, bool checkNoneTarget){
//
//		bool bPass = false;
//
//		for(int i=0;i<Parent.transform.childCount;i++){
//			Player _p = Parent.transform.GetChild(i).GetComponent<Player>();
//			if(_p.getTarget() == null){
////				Debug.Log("=======> "+i);
//				if(!checkNoneTarget){
//					//장수체크
//					bool bInGeneral = false;
//					if(_p.index == 0 && searchTargetGeneral(targetParent)){
//						bInGeneral = true;
//					}
//
//					if(checkSoliderTarget(targetParent, false)){
//						bool check = true;
//						do{
//							int idx = Random.Range(0, targetParent.transform.childCount);
//							Player _t = targetParent.transform.GetChild(bInGeneral?0:idx).GetComponent<Player>();
//							if(_t.getTarget() == null){
//								_t.setTargetObj(_p);
//								_t.setTargetRange();
//
//								_p.setTargetObj(_t);
//								_p.setTargetRange();
//								check = false;
//							}
//						}
//						while(check);
//					}
//				}
//				else{
//					bool recheck = checkSoliderTarget(targetParent, true);
////					Debug.Log("Re Target Check : my -> "+_p.sKey+", general check->"+recheck);
//					if(recheck){
//						bool check = true;
//						do{
//							int idx = Random.Range(0, targetParent.transform.childCount);
//							Player _t = targetParent.transform.GetChild(idx).GetComponent<Player>();
//							Debug.Log("00 target check : "+_t.targetObjPools.Count);
//							if(_t.targetObjPools.Count < 2){
//								_t.setTargetObj(_p);
//								_t.setTargetRange();
//
//								_p.setTargetObj(_t);
//								_p.setTargetRange();
//								_p.targetPassMove = true;
//								check = false;
//							}
//
////							if(!_t.getFront() || !_t.getRear()){
//////								if(!_t.getFront()){
//////									_t.setFront(_p);
//////									Debug.Log("Front Save!");
//////								}
//////								else if(!_t.getRear()){
//////									_t.setRear(_p);
//////									Debug.Log("Rear Save!");
//////								}
////								_p.setTargetObj(_t.transform);
////								_p.setTargetRange(_t.transform.position);
////								_p.targetPassMove = true;
////								check = false;
////							}
//						}
//						while(check);
//					}
//					bPass = true;
//				}
//			}//if(_p.target == null)
//		}
//
//		if(bPass ==  false){
//			switch(checkGeneralTarget(Parent, targetParent)){
//			case TargetState.Complete:
//				break;
//			case TargetState.Parent:
//				targetSolider(Parent, targetParent, true);
//				return;
//			case TargetState.targetParent:
//				targetSolider(targetParent, Parent, true);
//				return;
//			}
//		}
//
//
//	}

//	public TargetState checkGeneralTarget(GameObject Parent, GameObject targetParent){
//		if(checkSoliderTarget(Parent, false)){
//			return TargetState.Parent;
//		}
//		else if(checkSoliderTarget(targetParent, false)){
//			return TargetState.targetParent;
//		}
//		return TargetState.Complete;
//	}

//	IEnumerator targetSoliderCheckEnd(GameObject obj_1, GameObject obj_2){
//		yield return new WaitForSeconds(0.02f);
//
//		soliderStateChange(obj_1, PlayerState.Move_target);
//		soliderStateChange(obj_2, PlayerState.Move_target);
//		nowGameState = GameState.idle;
//	}

	IEnumerator targetSoliderCheckEnd(General _g1, General _g2){
		yield return new WaitForSeconds(0.02f);
		
		_g1.soldierStateChange(PlayerState.Move_target);
		_g2.soldierStateChange(PlayerState.Move_target);

		nowGameState = GameState.idle;
	}

}

public class SoliderBuffer{
	public Player _p											{get; set;}
	public string targetGeneralName					{get; set;}
}
