using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerDirection{
	LEFT = 0,
	RIGHT,
	UP,
	DOWN,
};
public enum PlayerAnimation{
	IDLE = 0,
	WALK,
	ATTACK,
};
public enum PlayerState{
	None = 0,
	SearchSoldier,
	SearchGeneral,
	Move,
	Move_target,
	Attack,
	Damaged,
	Dead,
};
public enum EncounterState{
	None = 0,
	Front,
	Rear,
};
public class Player : MonoBehaviour, IDamageable {

	public Vector3 targetPosition;
	Transform groundCheck;
	bool grounded = false;
	bool bMove = false;

	Animator anim;
//	TweenPosition tweenPos;

	public PlayerPosition soldierFlag;

	/// <summary>
	/// The move speed.
	/// </summary>
//	public Transform target;
	public Vector3 direction;
	public float moveSpeed;
	public float velocity;
	public bool facingRight = true;
	public PlayerAnimation aniState = PlayerAnimation.IDLE;
	public PlayerState currentState = PlayerState.None;

	public float currentHP;
	protected float maxHP;

	protected bool enableAttack = true;
	protected float attackPower = 0.1f;
	protected float demagedPower;

	public UISlider hpBarSlider;
	public GameObject hpBarObj;
	public Camera uiCam;
	public UIPanel hpBarPanel;
	Vector3 hpBarCalVec3;

	public int positionIndex = 0;
	public int group;
	public int index;
	public bool bTarget = false;
	public float startDelayTime;
//	public string sKey;

//	public Dictionary<EncounterState, Player> targetObjPools;
	public List<Player> targetObjPools;

	public EncounterState m_key;
	public Vector3 savePos;
	public bool targetPassMove = false;
	public bool bFirst = false;

	public float targetRange;
	public float rangeSize;

	void Awake()
	{ 
		anim = GetComponent<Animator>();
//		targetObjPools = new Dictionary<EncounterState, Player>();
		targetObjPools = new List<Player>();
		savePos = Vector3.zero;
		m_key = EncounterState.None;

		groundCheck = transform.Find ("groundCheck");
		if(soldierFlag == PlayerPosition.Enemy){//enemy
			Vector3 theScale = transform.localScale;
			theScale.x = -1;
			transform.localScale = theScale;
			facingRight = false;
		}
		else
			facingRight = true;
	}

//	void FixedUpdate()
	void Update()
	{
		if(GameData.Instance.inGameMananger.nowGameState == GameState.wait)
			return;

		switch(currentState)
		{
		case PlayerState.None:
//			if(aniState != PlayerAnimation.IDLE)
//				setPlayerAnimation(PlayerAnimation.IDLE);
			break;
		case PlayerState.Move:
			MoveToTarget (false);
			break;
		case PlayerState.Move_target:

			if(getTarget()){
#if false
				bool change = false;
				if(!facingRight && transform.position.x < getTarget().position.x){
					if(targetRange >= /*2.15f*/2.06f){
						change = true;
					}
				}
				else if(facingRight && transform.position.x > getTarget().position.x){
					if(targetRange >= /*2.15f*/2.06f){
						change = true;
					}
				}

//				Debug.Log(this.name+" : change : "+change+", changetargetRange : "+targetRange);
				if(change){
//					Debug.Log(this.name+", targetRange : "+targetRange);
					targetPassMove = false;
					//targetRange = 0f;
					setTargetRange();
					//getTarget().GetComponent<Player>().setTargetRange();
				}

//				if(!targetPassMove && targetRange > 1f){
//					setTargetRange();
//				}
#endif
				MoveToTarget (true);
			}
			else
			{
//				Debug.Log(this.name+" : targetRange : "+targetRange+", targetCnt?"+targetObjPools.Count);
				if(this.targetObjPools.Count > 0 && this.targetObjPools[0] == null){
					this.targetObjPools.RemoveAt(0);
					setPlayerState(PlayerState.SearchSoldier);
				}
			}

			break;
		case PlayerState.Attack:
#if false
			if(getTarget()){
				CircleCollider2D c2t = getTarget().GetComponent<Player>().GetComponent<CircleCollider2D>();
				float size = rangeSize+ c2t.radius;
				if(targetRange > size){//rangeSize){
					//				targetRange = 0f;
					//				targetPassMove = true;
					setTargetRange();
					setPlayerState(PlayerState.Move_target);
				}
			}
			else{
				//TODO::
				setPlayerState(PlayerState.None);
			}
#endif
			break;
		case PlayerState.Damaged:
			break;
		case PlayerState.Dead:
			break;
		}

//		checkFlip();

		if (uiCam != null)
		{
			RepositionHPBar();
		}

		if(getTarget()){
			targetRange = Vector3.Distance(this.gameObject.transform.position, getTarget().position);
		}

		PlayerAnimation _ani = checkAniState();
		if(this.aniState != _ani){
			setPlayerAnimation(this.aniState);
		}
#if false
		if(currentState == PlayerState.Attack && this.facingRight == getTarget().GetComponent<Player>().facingRight){
			flipTarget();
		}
#endif
	}

	PlayerAnimation checkAniState(){
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("T_idle")){
			return PlayerAnimation.IDLE;
		}
		else if(anim.GetCurrentAnimatorStateInfo(0).IsName("T_walk")){
			return PlayerAnimation.WALK;
		}
		else if(anim.GetCurrentAnimatorStateInfo(0).IsName("T_attack")){
			return PlayerAnimation.ATTACK;
		}
		return PlayerAnimation.IDLE;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
//		Debug.Log("0 Name:"+sKey+", idx :: "+index);
		Player _target = other.gameObject.GetComponent<Player>();
//		Debug.Log("0 KKK : "+this.sKey+" == "+_target.sKey);
		if(soldierFlag == _target.soldierFlag)
			return;
//		Debug.Log("1 KKK : "+this.sKey+" == "+_target.sKey);
		if(targetPassMove){
			if(_target.getTarget() != this.transform)
				return;
			else
				targetPassMove = false;
		}
//		Debug.Log("2 KKK : "+this.sKey+" == "+_target.sKey);
		if(currentState == PlayerState.Attack)
			return;
//		Debug.Log("3 KKK : "+this.sKey+" == "+_target.sKey);
		if(getTarget() && getTarget() != other.transform){
			return;
		}
//		Debug.Log("4 KKK : "+this.sKey+" == "+_target.sKey);
		if(this.bFirst != _target.bFirst)
			return;

//		if(index == 0 && GameData.Instance.inGameMananger.nowGameState == GameState.moveArmy){

//		if(GameData.Instance.inGameMananger.nowGameState == GameState.wait)
//			return;

		if(bFirst){

			if(getTarget() != null){
				if(getTarget() == other.transform){
					if(currentState != PlayerState.Attack){
//						Debug.Log("Attack : "+this.name);
						setPlayerState(PlayerState.Attack);
					}
				}
			}
			else{
				return;
			}
		}
		else{
			GameData.Instance.inGameMananger.checkGeneralCollision(transform.parent.gameObject, other.transform.parent.gameObject);
		}

	}

	void OnTriggerExit2D(Collider2D other){
		Player _target = other.gameObject.GetComponent<Player>();
		if(soldierFlag == _target.soldierFlag)
			return;
//		if(targetPassMove)
//			return;
//		if(currentState == PlayerState.Attack)
//			return;


//		Debug.Log("00 ################### ("+this.name+", "+other.name+")");
		if(otherTarget(_target)){
//			Debug.Log("11 ################### ("+this.name+", "+other.name+")");
		}

//		if(targetPassMove){
//			Debug.Log("00 ################### ("+this.name+", "+other.name+")");
//			if(getTarget() == other.transform){
//				Debug.Log("11 ################### ("+this.name+", "+other.name+")");
//				Invoke("attackPlayer", 0.5f);
//			}
//		}

//		if(getTarget() == other.transform){
//			if(targetPassMove){
//				//targetPassMove  = false;
//				Debug.Log("11 ################### ("+this.name+", "+other.name+")");
//				Invoke("attackPlayer", 0.5f);
//			}
//		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		Player _target = other.gameObject.GetComponent<Player>();
		if(soldierFlag == _target.soldierFlag)
			return;
		if(targetPassMove)
			return;
		if(currentState == PlayerState.Attack)
			return;
		if(getTarget() && getTarget() != other.transform){
				return;
		}
		if(!this.bFirst == _target.bFirst)
			return;

		if(currentState != PlayerState.Attack){
			setPlayerState(PlayerState.Attack);
		}
	}

	void attackPlayer(){
		if(targetPassMove){
			Debug.Log("attackPlayer!!!!!!!!!!!!!!!!");
			setTargetRange();
			targetPassMove  = false;
		}
	}
	void waitPlayer(){
		setPlayerState(PlayerState.None);
	}

	public void setTargetObj(Player _target){
		//Debug.Log("setTargetObj ::: "+_target+", this : "+this+", "+this.gameObject);
		targetObjPools.Add(_target);

//	public bool setTargetObj(Transform _target){
#if false
		if(getTarget() != null)
			return false;
#endif
//		bool check = false;
//		if(facingRight){
//			check = setFront(_target.GetComponent<Player>());
//		}
//		else{
//			check = setRear(_target.GetComponent<Player>());
//		}
//		return check;
	}

	public void setMoveStartDelay(float delay)
	{
		Invoke("onMove", Random.Range(startDelayTime+(delay-0.1f), startDelayTime+(delay+0.1f)));
	}
	void onMove()
	{
		setPlayerState(PlayerState.Move);
	}

	public void setPlayerState(PlayerState _state)
	{

//		if(currentState == _state)
//			return;

		switch(_state)
		{
		case PlayerState.None: case PlayerState.SearchSoldier:
			setPlayerAnimation(PlayerAnimation.IDLE);
			break;
		case PlayerState.Move: case PlayerState.Move_target:
			setPlayerAnimation(PlayerAnimation.WALK);
			break;
		case PlayerState.Attack:
//			transform.localPosition = transform.localPosition;
			setPlayerAnimation(PlayerAnimation.ATTACK);
			break;
		case PlayerState.Damaged:
			break;
		case PlayerState.Dead:
			setPlayerAnimation(PlayerAnimation.IDLE);
			break;
		}

		currentState = _state;
	}

	public void setPlayerAnimation(PlayerAnimation _ani)
	{
//		if(aniState == _ani)
//			return;

		aniState = _ani;

		switch(aniState){
		case PlayerAnimation.IDLE:
			anim.Play("T_idle");
			break;
		case PlayerAnimation.WALK:
			anim.Play("T_walk");
			break;
		case PlayerAnimation.ATTACK:
			anim.Play("T_attack");
			break;
		}
	}

	void MoveToTarget (bool bTarget) 
	{
		if(bTarget)
		{
			if(getTarget() == null)
				return;

			direction = (getTarget().position - transform.position).normalized;
			Vector3 v3tmp = direction * (Time.deltaTime * moveSpeed);

			v3tmp = this.transform.position + v3tmp;

			float now = Vector3.Distance(v3tmp, this.transform.position);
			float dest = Vector3.Distance(this.transform.position, getTarget().position);

			Player tp = getTarget().GetComponent<Player>();

//			if(tp.getTarget() != this.transform){
//				targetPassMove = (tp.facingRight == this.facingRight) ? true : false;
//			}

			if(now >= dest){
				if(targetPassMove){
//					Debug.Log("moveTarget : "+now+" >= "+dest+", size : "+this.GetComponent<SpriteRenderer>().bounds.size.x);
//
//					bool bUp = false;
//					if(this.transform.position.y < getTarget().position.y){
//						bUp = true;
//					}

					float size = this.GetComponent<SpriteRenderer>().bounds.size.x / 2 ;
					float sizeR = (this.facingRight) ? size : -size;
//					Vector3 tmp = new Vector3(getTarget().position.x+sizeR, getTarget().position.y+(bUp?size:-size), getTarget().position.z);
					Vector3 tmp = new Vector3(getTarget().position.x+sizeR, getTarget().position.y, getTarget().position.z);
					this.transform.position = tmp;
					setPlayerState(PlayerState.Attack);
					targetPassMove = false;
				}
			}
			else{
				this.transform.position = v3tmp;
			}


//			if(targetPassMove == false)
//			{
//				direction = (getTarget().position - transform.position).normalized;
//				velocity =  (velocity + moveSpeed * Time.deltaTime);
//				transform.position = new Vector3 (transform.position.x + (direction.x * velocity),
//				                                  transform.position.y + (direction.y * velocity),
//				                                  transform.position.z);
//				velocity = 0.0f;
//			}
//			else{
//				float move = (moveSpeed/2 * Time.deltaTime);
//				this.transform.position = new Vector3(
//					transform.position.x-(savePos.x * move),
//					transform.position.y-(savePos.y * move),
//					transform.position.z
//					);
//			}

//			// Player의 위치와 이 객체의 위치를 빼고 단위 벡터화 한다.
//			direction = (getTarget().position - transform.position).normalized;
//			// 가속도 지정 (추후 힘과 질량, 거리 등 계산해서 수정할 것)
//			//moveSpeed = 1.0f;
//			// 초가 아닌 한 프레임으로 가속도 계산하여 속도 증가
//			velocity =  (velocity + moveSpeed * Time.deltaTime);
//#if true
//			transform.position = new Vector3 (transform.position.x + (direction.x * velocity),
//			                                  transform.position.y + (direction.y * velocity),
//			                                  transform.position.z);
//			velocity = 0.0f;
//#else
//			// Player와 객체 간의 거리 계산
//			float distance = Vector3.Distance (target.position, transform.position);
//			// 일정거리 안에 있을 시, 해당 방향으로 무빙
//			if (distance <= 10.0f) {
//				transform.position = new Vector3 (transform.position.x + (direction.x * velocity),
//				                                  transform.position.y + (direction.y * velocity),
//				                                  transform.position.z);
//			}
//			// 일정거리 밖에 있을 시, 속도 초기화 
//			else {
//				velocity = 0.0f;
//			}
//#endif

////			velocity =  (velocity+(moveSpeed * Time.deltaTime));
//			float move = (moveSpeed * Time.deltaTime);
//
//			this.transform.position = new Vector3(
//				transform.position.x-(savePos.x * move),
//				transform.position.y-(savePos.y * move),
//				transform.position.z
//				);
//			velocity = 0.0f;

		}
		else
		{
//			velocity =  velocity + (moveSpeed * Time.deltaTime);
//			transform.position = new Vector3 (transform.position.x + (facingRight?velocity:-velocity),
//			                                  transform.position.y ,
//			                                  transform.position.z);

			velocity =  velocity + (moveSpeed * Time.deltaTime);
			transform.position = new Vector3 (transform.position.x + (facingRight?velocity:-velocity),
			                                  transform.position.y ,
			                                  transform.position.z);

//			testX += velocity;

//			if(testX >= 10f){
//				testX = 0;
//				facingRight = !facingRight;
//			}

//			Debug.Log(testX);

			velocity = 0.0f;
		}

		checkFlip();
		if (uiCam != null)
		{
			RepositionHPBar();
		}

//		if(aniState != PlayerAnimation.WALK){
//			setPlayerAnimation(PlayerAnimation.WALK);
//		}
	}

	public void setTargetRange(){
		if(getTarget() == null)
			return;

//		Debug.Log("setTargetRange :: "+this+", target :: "+targetObjPools.Count+", "+targetObjPools[0]);

		savePos.x = transform.position.x - targetObjPools[0].transform.position.x;
		savePos.y = transform.position.y - targetObjPools[0].transform.position.y;

//		savePos.x = transform.position.x - targetPos.x;
//		savePos.y = transform.position.y - targetPos.y;
	}

	public void checkFlip()
	{
		if(getTarget() == null || targetPassMove)
			return;

		if(transform.position.x < getTarget().position.x && !facingRight){
			if(this.name.Equals("Hero_15_0"))
				Debug.Log("######## 00");
			Flip();
		}
		else if(transform.position.x > getTarget().position.x && facingRight){
			if(this.name.Equals("Hero_15_0"))
				Debug.Log("######## 11");
			Flip();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		if(getTarget() != null){
			theScale.x *=  -1;
		}
		transform.localScale = theScale;
	}

	public void InitPlayer(float setupMaxHP, 
	                      float setupAttackPower, 
	                      float setupMoveSpeed)
	{
		// walk 애니메이션을 재생하도록 한다. 
		//animator.SetTrigger("isAlive");
		// HP와 공격력, 이동속도를 설정한다.
		maxHP = setupMaxHP;
		currentHP = setupMaxHP;
		attackPower = setupAttackPower;
		moveSpeed = setupMoveSpeed;
		// 캐릭터 상태를 변경하여 이동을 시작하도록 한다.
		currentState = PlayerState.None;
		// isAlive 트리거를 초기화해서 dead 애니메이션 종료 후 walk 애니메이션 바로 전환되는 것을 방지.
		//animator.ResetTrigger("isAlive");
	}

	public void InitHPBar(UISlider targetHPBar,
	                      UIPanel targetPanel, 
	                      Camera targetCam)
	{
		// 멤버 필드 할당.
		hpBarSlider = targetHPBar;
		hpBarObj = hpBarSlider.gameObject;
		hpBarPanel = targetPanel;
		uiCam = targetCam;
		// 오브젝트 풀에서 제외되도록 초기값 임시 수정.
		hpBarObj.transform.localPosition = Vector3.left * 1000;
		// hpbar를 켠다.
		TurnOnOffHPBar(true);
//		TurnOnOffHPBar(index==0?true:false);
	}

	protected void RepositionHPBar()
	{
		if(hpBarObj == null)
			return;
		// 적 위치가 카메라 상에서 어느 위치인지 계산.
		hpBarCalVec3 = uiCam.WorldToScreenPoint(transform.position);
		hpBarCalVec3.z = 0;
		
		if(GameData.Instance.targetWidth == 0)
		{
			GameData.Instance.targetWidth = hpBarPanel.width*
				(GameData.Instance.targetHeight/hpBarPanel.height);
//			Debug.Log(GameData.Instance.targetWidth);
		}
		
		// UIPanel의 크기를 고려하여 상대적인 위치를 적용. 
		hpBarCalVec3.x = 
			(hpBarCalVec3.x / Screen.width) * GameData.Instance.targetWidth;
		hpBarCalVec3.y = 
			(hpBarCalVec3.y / Screen.height) * GameData.Instance.targetHeight;
		Vector3 cc = new Vector3(hpBarCalVec3.x, hpBarCalVec3.y+20, hpBarCalVec3.z);
		hpBarObj.transform.localPosition =  cc;
	}
	
	public void TurnOnOffHPBar(bool isTurnOn = false)
	{
		// hpbar를 끄고 켠다.
		hpBarObj.SetActive(isTurnOn);
	}

	public Transform getTarget(){
		if(targetObjPools.Count > 0){
			if(targetObjPools[0] != null)
				return targetObjPools[0].transform;				
		}
		return null;
	}

	public bool otherTarget(Player _tp){
		foreach(Player save in targetObjPools){
			if(save != null && _tp != null){
				if(save.name.Equals(_tp.name)){
					return true;
				}
			}
		}
		return false;
	}
	
	public void delTarget(){
//		if(facingRight){
//			if(getFront())
//				targetObjPools.Remove(EncounterState.Front);
//		}
//		else{
//			if(getRear())
//				targetObjPools.Remove(EncounterState.Rear);
//		}

		if(getTarget()){
			targetObjPools.RemoveAt(0);
		}

	}

	public virtual void Attack()
	{

		if(getTarget() == null){
			setPlayerState(PlayerState.SearchSoldier);
		}
		else{
			if(getTarget().GetComponent<Player>().currentHP == 0){
				setPlayerState(PlayerState.SearchSoldier);
			}
			else{
				IDamageable damageTaget  = (IDamageable)getTarget().GetComponent(typeof(IDamageable));
				damageTaget.Damage(attackPower);
			}

			//dir check
			Player tp = getTarget().GetComponent<Player>();
			if(this.transform.position.x < getTarget().position.x)
			{
				if(tp.getTarget() == this.transform && tp.facingRight == true)
				{
					tp.Flip();
				}
				if(this.facingRight == false)
				{
					Flip();
				}
			}
			else
			{
				if(tp.getTarget() == this.transform && !tp.facingRight)
				{
					tp.Flip();
				}
				if(this.facingRight)
				{
					Flip();
				}
			}

		}


//		Player _tp = (getTarget() != null)? getTarget().GetComponent<Player>() : null;
//
//		if(_tp == null){
//			setPlayerState(PlayerState.None);
//			GameData.Instance.inGameMananger.nextTarget(this);
//		}
//		else{
//			IDamageable damageTaget  = (IDamageable)_tp.GetComponent(typeof(IDamageable));
//			damageTaget.Damage(attackPower);
//
//			if(_tp.currentState == PlayerState.Dead && currentState == PlayerState.Attack){
////				setPlayerState(PlayerState.None);
//				GameData.Instance.inGameMananger.nextTarget(this);
//			}
//
////			if(getTarget() == null || getTarget().GetComponent<Player>().currentHP == 0){
////				setPlayerState(PlayerState.None);
////				GameData.Instance.inGameMananger.nextTarget(this);
////			}
//		}
	}

	public void Damage(float damageTaken)
	{
		if(currentState == PlayerState.Dead)
			return;
//		Debug.Log("Damage : "+damageTaken);

		// currentHP를 소진한다.
		currentHP -= damageTaken;
		// 체력 표시를 감소시킨다.
		hpBarSlider.value = (float)currentHP/(float)maxHP;
		// 현재 체력이 0과 같거나 작다면 
		if (currentHP <= 0)
		{
			currentHP = 0;
			// 체력 표시를 모두 제거한다.
			hpBarSlider.value = 0;
			
			enableAttack = false;
//			setPlayerState(PlayerState.Dead);
			//TEST CODE
//			Destroy(hpBarSlider.gameObject);
//			GameData.Instance.inGameMananger.playerDead(this);

			// dead 애니메이션 재생
//			animator.SetTrigger("isDead");
//			
//			if( IsInvoking("ChangeStateToMove") )
//			{
//				CancelInvoke("ChangeStateToMove");
//			}
			
			// 점수 증가.
			//GameData.Instance.gamePlayManager.AddScore(10);
			// 적 보스가 사망하면 다시 적을 생성할 수 있도록 처리한다.
//			if (gameObject.tag == "boss")
//			{
//				GameData.Instance.gamePlayManager.SetupGameStateToIdle();
//			}
		}
		else
		{
			//animator.SetTrigger("damaged");
		}
	}

	public void flipTarget(){
		Flip();
		setPlayerState(PlayerState.Attack);
	}
}
