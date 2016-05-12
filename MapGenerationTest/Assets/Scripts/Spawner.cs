using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	//public GameObject whatToSpawn;
	//public Vector3 whereToSpawn;

	public GameObject playerPrefab;
	private GameObject player1;

	ArrayList playerArrayList = new ArrayList();

	// Use this for initialization
	void Start () {
		//player1 = player1.GetComponent<ObjectMover> ().player;
		//player1.transform.position = new Vector3 (0, 6, 0);
		//Instantiate (playerPrefab, player1.transform.position, player1.transform.rotation);
	}

	public void AddPlayer(GameObject go, int playerNumber){
		Vector3 spawnLocation = new Vector3 (0, 6, 0);
		Quaternion rotation = new Quaternion();
		playerArrayList.Add (Instantiate (go, spawnLocation, rotation));
		Debug.Log (playerArrayList [0]);
	}

	public void IntanstiatePlayers(){
		foreach(GameObject go in playerArrayList){
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
