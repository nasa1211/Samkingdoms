using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	GameObject enemy;
	GameObject player;

//	GameObject player;
	// Use this for initialization
	void Start () {
//		GameObject soldier = Instantiate(Resources.Load("spine/walk")) as GameObject;
//		soldier.transform.parent = transform;
////		soldier.transform.position = new Vector3(Random.Range (-1000, 1000)/100f, Random.Range (-400, 400)/100f, 0f);
//		soldier.transform.localScale = new Vector3(60,60,0);
//		soldier.SetActive (true);
		enemy = transform.FindChild("Enemy").gameObject;
		player = transform.FindChild("Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
//		if(player.GetComponent<TweenPosition>())

	}
}
