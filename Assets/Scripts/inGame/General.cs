using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class General : MonoBehaviour {

	public Transform target = null;
	public List<Player> soldierObj = new List<Player>();
	public bool soldierTargetCheck = false;
	public PlayerPosition generalPosition;
	public int index;

	void Awake(){
	}
	void OnDestroy(){
	}
	void OnEnable(){
	}
	void Update(){
	}

	public void setTarget(General _target){
		this.target = _target.transform;
	}

	public void delTarget(){
		Destroy(target);
		target = null;
	}

	public Player getSoldier(){
		foreach(Player _p in soldierObj){
			if(_p.currentState == PlayerState.None){
				return _p;
			}
		}
		return null;
	}

	public Player getNearingSoldier(Player soldier){
		if(this.target == null || soldier == null)
			return null;

		float tmpDist = 0, saveDist = 10000;
		Player nearPlayer = null;
		foreach(Player _p in soldierObj){
//			if(_p.currentState == PlayerState.SearchSoldier || _p.currentState == PlayerState.None)
			{
				tmpDist = Vector3.Distance(_p.transform.position, soldier.transform.position);
				if(tmpDist < saveDist && _p.targetObjPools.Count < 2){
					saveDist = tmpDist;
					nearPlayer = _p;
				}
			}
		}
		return nearPlayer;
	}

	public void soldierStateChange(PlayerState _state){
		foreach(Player _p in soldierObj){
			_p.bFirst = true;
			if(_state == PlayerState.Move_target){
				if(_p.index == 0){
					_p.setPlayerState(PlayerState.Attack);
				}
				else{
					if(_p.getTarget() != null)
						_p.setPlayerState(_state);
					else
						_p.setPlayerState(PlayerState.SearchSoldier);
				}
			}  
			else{
				_p.setPlayerState(_state);
			}
		}
	}

	public void soldiersTargetSettings(){

		if(getTargetGeneral() == null || getTargetGeneral().soldierObj.Count == 0)
			return;

//		//죽은 장수 체크
//		for(int i=getTargetGeneral().soldierObj.Count-1; i>=0;i--){
//			Player _p = getTargetGeneral().soldierObj[i];
//			if(_p.currentState == PlayerState.Dead){
//				getTargetGeneral().soldierObj.Remove(_p);
//				Destroy(_p.hpBarSlider.gameObject);
//				Destroy(_p.gameObject);
//			}
//		}

		//Target Search
		foreach(Player _p in soldierObj){
			if(_p != null && _p.getTarget() == null){

				if(_p.index == 0){
					if(getTargetSoldier(0) != null && getTargetSoldier(0).getTarget() == null){
						_p.setTargetObj(getTargetSoldier(0));
						_p.setTargetRange();
						getTargetSoldier(0).setTargetObj(_p);
						getTargetSoldier(0).setTargetRange();
						continue;
					}
				}

				List<Player> _list = new List<Player>();
				foreach(Player _tp in getTargetGeneral().soldierObj){
					if(_tp != null && _tp.targetObjPools.Count == 0){
						_list.Add(_tp);
					}
				}

				if(_list.Count == 0){//ReCheck
					foreach(Player _tp in getTargetGeneral().soldierObj){
						if(_tp != null && _tp.targetObjPools.Count == 1){
							_list.Add(_tp);
						}
					}
				}

				if(_list.Count > 0){
					int idx = Random.Range(0, _list.Count);
					_list[idx].setTargetObj(_p);
					_list[idx].setTargetRange();
					if(_p.targetObjPools.Count > 1)
						_list[idx].targetPassMove = true;

					_p.setTargetObj(_list[idx]);
					_p.setTargetRange();
					if(_list[idx].targetObjPools.Count > 1 && (_list[idx].facingRight != _p.facingRight))
						_p.targetPassMove = true;

//					Debug.Log("???? "+_p.name+", "+_p.targetObjPools.Count+" || "+_list[idx].name+", "+_list[idx].targetObjPools.Count+" ## "+_p.targetPassMove);
				}
				else{
					//Waiting Soldier
					//_p.setPlayerState(PlayerState.None);
					_p.setPlayerState(PlayerState.SearchSoldier);
//					Debug.Log("SSSSSSSSSS :::   "+_p.name);
				}
			}
		}
		soldierTargetCheck = true;
	}

	public General getTargetGeneral(){
		if(this.target)
			return this.target.GetComponent<General>();
		return null;
	}

	Player getTargetSoldier(int index){
		if(getTargetGeneral()){
			if(index < getTargetGeneral().soldierObj.Count){
				return getTargetGeneral().soldierObj[index];
			}
		}
		return null;
	}

	int targetSoldierAttackCount(int index){
		return getTargetSoldier(index).targetObjPools.Count;
	}

	public Player getTargetEmptySoldier(){
		if(getTargetGeneral()){
			foreach (var item in soldierObj) {
//				if(item.currentState == PlayerState.SearchSoldier)
				if(item.targetObjPools.Count < 2)
				{
					return item;
				}
			}
		}
		return null;
	}

}
