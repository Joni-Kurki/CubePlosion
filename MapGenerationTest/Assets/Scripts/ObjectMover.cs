using UnityEngine;
using System.Collections;

public class ObjectMover : MonoBehaviour {

    public GameObject player;
    public MapGeneration map;
    public PlayerTracker playerTracker;
    
    // SmoothMovement muuttujat
    // kuinka pitkään menee kun liikutaan sijainnista toiseen
    public float movementTime;
    // kuinka paljon liikutaan
    public int mov;
    // onko objekti vielä liikkeessä
    private bool isObjectMoving;
    // millon liikkuminen aloitettiin
    private float movementStartTime;
    // liikkumisen aloituspiste
    private Vector3 startPosition;
    // liikkumisen lopetuspiste
    private Vector3 endPosition;

	private bool isAI;
    // Use this for initialization
    void Start() {
        // laitetaan pelaajan sijainti pelialueen päälle
        // tähän spawneri sitten
        //player.transform.position = new Vector3(0, map.GetRows(), 0);
    }

    // liikkuminen tiettyyn ruutuun
    void MoveToLocation(int x, int y, int z) {
        int tX = (int)player.transform.position.x + x;
        int tY = (int)player.transform.position.y + y;
        int tZ = (int)player.transform.position.z + z;
        // koitetaan ensiksi ollaanko samalla tasolla.
        if (map.IsTileFree(tX,tZ,tY)) {
            player.transform.Translate(x,y,z);
        }
		/*
		// jos ei koitetaan ylempää tasoa
        else if (map.IsTileFree(tX, tZ, tY+1)) {
            player.transform.Translate(x, y+1, z);
        }// muutoin koitetaan 2 tasoa ylempää
        else if (map.IsTileFree(tX, tZ, tY + 2)) {
            player.transform.Translate(x, y + 2, z);
        }*/
    }

	public void setAI(bool ai = false){
		isAI = ai;
	}
    // parametreissä liikkeen suunta
    void SmoothMovement(int x, int y, int z) {
        isObjectMoving = true;
        movementStartTime = Time.time;
        startPosition = player.transform.position;
        if (map.IsTileFree((int)player.transform.position.x + x, (int)player.transform.position.z + z, (int)player.transform.position.y + y)) {
            endPosition = startPosition + (new Vector3(x, y, z) * mov);
        }
        else if (map.IsTileFree((int)player.transform.position.x + x, (int)player.transform.position.z + z, (int)player.transform.position.y + y + 1)) {
            endPosition = startPosition + (new Vector3(x, y + 1, z) * mov);
        }
        else if (map.IsTileFree((int)player.transform.position.x + x, (int)player.transform.position.z + z, (int)player.transform.position.y + y + 2)) {
            endPosition = startPosition + (new Vector3(x, y + 2, z) * mov);
        }
        //endPosition = startPosition + (new Vector3(x, y, z)*mov);
    }
    // Update is called once per frame
    void Update() {
		// jos ei ole AI pelaaja
		if (!isAI) {
			if (Input.GetKeyDown (KeyCode.Keypad9)) {
				if (!isObjectMoving)
					SmoothMovement (+mov, 0, +mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad8)) {
				if (!isObjectMoving)
					SmoothMovement (0, 0, +mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad7)) {
				if (!isObjectMoving)
					SmoothMovement (-mov, 0, +mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad6)) {
				if (!isObjectMoving)
					SmoothMovement (+mov, 0, 0);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad4)) {
				if (!isObjectMoving)
					SmoothMovement (-mov, 0, 0);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad3)) {
				if (!isObjectMoving)
					SmoothMovement (+mov, 0, -mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad2)) {
				if (!isObjectMoving)
					SmoothMovement (0, 0, -mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			} else if (Input.GetKeyDown (KeyCode.Keypad1)) {
				if (!isObjectMoving)
					SmoothMovement (-mov, 0, -mov);
				//Debug.Log("Update: moving with lerp" + startPosition + " -> " + endPosition);
			}
			if (playerTracker.GetTaggingSW ())
				MarkTile (1);
		}
    }

    void LateUpdate() {/*
        if (playerTracker.GetTaggingSW())
            MarkTile(1);*/
    }
	// pelaaja menettää elämän
    void playerDies() {
        player.transform.position = map.FindSpawnTile();
        playerTracker.Died();
    }

    void FixedUpdate() {
        // tiputtaa pelaajan yhden ruudun alemmaksi jos leijuu
        if (IsObjectFloating((int)player.transform.position.x, (int)player.transform.position.y, (int)player.transform.position.z)){
            MoveToLocation(0, -1, 0);
            //rb.AddForce(0, -10f, 0);
        }
        if (player.transform.position.y <= -5)
            playerDies();
            //player.transform.position = new Vector3(map.GetWidth()/2,map.GetRows(),map.GetHeight()/2);
        	// liikkumis komennot
        if (isObjectMoving) {
            //SmoothMovement(0, 0, +mov);

            float timeSinceStarted = Time.time - movementStartTime;
            float movPercentComplete = timeSinceStarted / movementTime;

            player.transform.position = Vector3.Lerp(startPosition, endPosition, movPercentComplete);
            //Debug.Log("Moving to :" + startPosition + " -> " + endPosition + ", " + movementStartTime + "% compelete");
            // tägätään tile 
            //MarkTile(1);
            // jos ollaan suoritettu liikkuminen loppuun
            if (movPercentComplete >= 1.0f)
                isObjectMoving = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            //MoveToLocation(+mov, 0, +mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8)) {
            //MoveToLocation(0, 0, +mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7)) {
            //MoveToLocation(-mov, 0, +mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6)) {
            //MoveToLocation(mov, 0, 0);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5)) {
            //player.transform.position = map.ResetPlayerPosition();
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4)) {
            //MoveToLocation(-mov, 0, 0);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3)) {
            //MoveToLocation(+mov, 0, -mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2)) {
            //MoveToLocation(0, 0, -mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1)) {
            //MoveToLocation(-mov, 0, -mov);
            if (playerTracker.GetTaggingSW())
                MarkTile(1);
        }
    }
    // tägätään tile, jonka päällä pelaaja on
    void MarkTile(int pNumber) {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.position.z;
        // jos ei oo tyhjä, eli ilmaa
        if (map.GetTileStatus((int)x, (int)z, (int)y) != 0 && (int)x >= 0 && (int)z >= 0) {
            map.SetMapTile((int)x, (int)z, (int)y - 1, pNumber + 1);
            map.setRefreshMap();
        }       
    }
    
    // tarkastetaan leijuuko objekti, jos objektin alla on tyhjää
    bool IsObjectFloating(int x, int y, int z) {
        if (map.isGameStarted())
            if (x >= 0 && x < map.GetWidth() && z >= 0 && z < map.GetHeight() && y >= 1 && y < map.GetRows())
                if (map.gamearea[x, z, y - 1] == 0) {
                    return true;
                }
                else
                    return false;
            else
                return true;
        else
            return false;
    }
}
