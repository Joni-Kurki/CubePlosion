using UnityEngine;
using System.Collections;
using System;

public class MapGeneration : MonoBehaviour {

    /* gamearea title names
    0 = empty, destroyed
    1 = empty, still in the map , CubeBlue or SnowCube // eli tähän teeman mukainen perus cube set
    2 = p1, reserved tile
    3 = p2, reserved tile
    ...
    */

    // kentän leveys ja korkeus ja taso
    public int width, height, rows;

    // Playeriltä arvot tulee muodossa XZY = width, height, rows

    // pelialue! Eli missä mitä missäkin tilessä on.
    public int[,,] gamearea;
    // tänne kaikki eri cube tile tyypit mitä käytetään kentässä.
    public GameObject GameAreaCubeBlue;
    public GameObject GameAreaCubeSnow;
    public GameObject GameAreaCubeOrange;
    public GameObject MiddlePillarContainer;

    public GameController gc;

    private int Level;
    private bool gameStarted;
    private bool lvlComplete;
    private bool refreshMap;
	// kentän dimensio
    int currentW, currentH, currentR;

	private Vector3 [] spawnPoints = new Vector3[4];

    // Kentän alustus
    void Start () {
        gameStarted = false;
        lvlComplete = false;
        refreshMap = false;
    }
	// asetetaan aloituskenttä
	public int NextLevel(){
		return Level + 1;
	}

	public Vector3 FindInitialSpawnPoints(int optionalSpawnPoint){
		spawnPoints [0] = new Vector3 (0, currentR, 0);
		spawnPoints [1] = new Vector3 (currentW-1, currentR, 0);
		spawnPoints [2] = new Vector3 (0, currentR, currentH-1);
		spawnPoints [3] = new Vector3 (currentW-1, currentR, currentH-1);

		return spawnPoints [optionalSpawnPoint];
	}


	public void RestartGame (){
		gameStarted = false;
		lvlComplete = false;
		refreshMap = false;
	}

    public void setLevel(int lvl) {
        Level = lvl;
		//Debug.Log ("Map's level: " + Level);
    }

    public void setRefreshMap() {
        refreshMap = true;
		RefreshGameArea ();
    }

	public bool isLevelComplete(){
		return lvlComplete;
	}

    void Update(){
        //RefreshGameArea();
		//if (refreshMap)
			//RefreshGameArea ();

    }

    void FixedUpdate() {
        //RefreshGameArea();
		//if (refreshMap)
            //RefreshGameArea();
        
    }

    void LateUpdate() {
        //RefreshGameArea();
        //if (refreshMap)
            //RefreshGameArea();

    }

    void RefreshGameArea() {
        EmptyGameArea();
        if (isGameStarted())
            DrawGameArea();
        refreshMap = false;
    }

    void EmptyGameArea(){
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("AllCubes");
        for(int d = 0; d < gameObjects.Length; d++) {
            Destroy(gameObjects[d]);
        }
        gameObjects = GameObject.FindGameObjectsWithTag("TaggedCubeOrange");
        for (int d = 0; d < gameObjects.Length; d++) {
            Destroy(gameObjects[d]);
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Pillar");
        for (int d = 0; d < gameObjects.Length; d++) {
            Destroy(gameObjects[d]);
        }
    }
    public void StartGame() {
        gameStarted = true;
        // piirretään kenttä
        GenerateGameArea();
        DrawGameArea();
    }
    public bool isGameStarted() {
        if (gameStarted)
            return true;
        else
            return false;
    }

    // mikä kenttä piirretään
    void GenerateGameArea() {
        switch (Level) {
			case 0:
				EmptyGameArea ();
				currentW = 4;
				currentH = 4;
				currentR = 1;
				GenerateTestLevel();
				break;
			case 1:
				EmptyGameArea ();
                currentW = 12;
                currentH = 12;
                currentR = 3;
                GenerateLevel1();
                break;
			case 2:
				EmptyGameArea ();
				currentW = 12;
				currentH = 12;
				currentR = 3;
				GenerateLevel2();
				break;
            default:
                GenerateDefaultGameArea();
                break;
        }
    }
    // pelialueen muodostaminen ennaltamääriteteilly parametreilla
    void GenerateDefaultGameArea() {
        gamearea = new int[width, height, rows];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < rows; z++) {
                    gamearea[x, y, z] = 1;
                }
            }
        }
    }
    // kenttä 1 prototyyppi, tiedostosta lukeminen fiksumpaa
    void GenerateLevel1() {
        gamearea = new int[currentW, currentH, currentR];
        for (int x = 0; x < currentW; x++) {
            for (int y = 0; y < currentH; y++) {
                for (int z = 0; z < currentR; z++) {
                    if(z == 0) {
                        if((x >= 4 && x <= 7)&&( y >= 4 && y <= 7 )) 
                            gamearea[x, y, z] = 1;
                        else
                            gamearea[x, y, z] = 0;
                    }
                    else if(z == 1) {
                        if ((x >= 2 && x <= 9) && (y >= 4 && y <= 7))
                            gamearea[x, y, z] = 1;
                        else if ((x >= 4 && x <= 7) && (y >=2 && y <= 9))
                            gamearea[x, y, z] = 1;
                        else
                            gamearea[x, y, z] = 0;
                    }
                    else if(z == 2) {
                        // keskialue
                        if ((x >= 2 && x <= 9) && (y >= 2 && y <= 9))
                            gamearea[x, y, z] = 1;
                        // oikea yläkulma
                        else if ((x >= 0 && x <= 3) && (y == 0))
                            gamearea[x, y, z] = 1;
                        else if ((x >= 0 && x <= 3) && (y == 1))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 2 && y <= 3) && (x == 0))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 2 && y <= 3) && (x == 1))
                            gamearea[x, y, z] = 1;
                        // vasen yläkulma
                        else if ((x >= 8 && x <= 11) && (y == 0))
                            gamearea[x, y, z] = 1;
                        else if ((x >= 8 && x <= 11) && (y == 1))
                            gamearea[x, y, z] = 1;
                        else if ((x >= 10 && x <= 11) && (y == 2))
                            gamearea[x, y, z] = 1;
                        else if ((x >= 10 && x <= 11) && (y == 3))
                            gamearea[x, y, z] = 1;
                        // oikea alakulma 
                        else if ((y >= 8 && y <= 11) && (x == 0))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 8 && y <= 11) && (x == 1))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 10 && y <= 11) && (x == 2))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 10 && y <= 11) && (x == 3))
                            gamearea[x, y, z] = 1; 
                        // vasen alakulma
                        else if ((y >= 8 && y <= 11) && (x == 11))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 8 && y <= 11) && (x == 10))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 10 && y <= 11) && (x == 9))
                            gamearea[x, y, z] = 1;
                        else if ((y >= 10 && y <= 11) && (x == 8))
                            gamearea[x, y, z] = 1;
                        else
                            gamearea[x, y, z] = 0;
                    }
                    else
                        gamearea[x, y, z] = 0;
                }
            }
        }
    }
	void GenerateTestLevel(){
		gamearea = new int[currentW, currentH, currentR];
		for (int x = 0; x < currentW; x++) {
			for (int y = 0; y < currentH; y++) {
				for (int z = 0; z < currentR; z++) {
					gamearea[x, y, z] = 1;
				}
			}
		}
	}

	void GenerateLevel2() {
		gamearea = new int[currentW, currentH, currentR];
		for (int x = 0; x < currentW; x++) {
			for (int y = 0; y < currentH; y++) {
				for (int z = 0; z < currentR; z++) {
					if (z == 0) {
						if((x >= 0 && x <=11)&&(y == 0 || y == 5 || y == 6|| y == 11))
							gamearea[x, y, z] = 1;
						else if((x == 0 || x == 11) && ((y > 0 && y < 5) || (y > 6 && y < 11)))
							gamearea[x, y, z] = 1;
						
					}
					else if (z == 1) {
						if((x >= 0 && x <= 11) && ( y == 1 || y == 10))
							gamearea[x, y, z] = 1;
						else if((x == 1 || x == 2 || x == 9 || x == 10) && (y == 0 || y == 11))
							gamearea[x, y, z] = 1;
						else if((x == 1 || x == 2 || x == 5 || x == 6 || x == 9 || x == 10)&&( y >=2 && x <= 10))
							gamearea[x, y, z] = 1;
					}
					else if (z == 2) {
						if((x == 0 || x == 1 || x == 10 || x == 11) && ( y == 0 || y == 11))
							gamearea[x, y, z] = 1;
						if((x == 1 || x == 2 || x == 9 || x == 10) && ( y == 1 || y == 10))
							gamearea[x, y, z] = 1;
						if((x == 2 || x == 3 || x == 8 || x == 9) && ( y == 2 || y == 9))
							gamearea[x, y, z] = 1;
						if((x == 3 || x == 4 || x == 7 || x == 8) && ( y == 3 || y == 8))
							gamearea[x, y, z] = 1;
						if((x >= 4 && x <= 7 ) && ( y >= 4 && y <= 7))
							gamearea[x, y, z] = 1;
					}
					else
						gamearea[x, y, z] = 0;
				}
			}
		}
	}
    public void DrawGameArea() {
        int arraySize = 0, tempCount = 0;
        // jos ollaan onnistuneesti luotu pelialue, piirretään gamearea muuttujan sisältö
        if (gamearea != null){
            for (int x = 0; x < currentW; x++){
                for (int y = 0; y < currentH; y++){
                    for (int z = 0; z < currentR; z++){
                        arraySize++;
                        if (gamearea[x, y, z] == 1){
                            tempCount++;
                            // palikan sijainti x,y,z koordinaatistossa 
                            Vector3 position = new Vector3(x, z, y);
                            // ei anneta palikoille yhtään kiertoa
                            Quaternion rotation = new Quaternion();
                            // luodaan palikka kohtaan x,y,z - 0 kierrolla.
                            Instantiate(GameAreaCubeSnow, position, rotation);
                        }
                        else if (gamearea[x, y, z] == 2){
                            tempCount++;
                            // palikan sijainti x,y,z koordinaatistossa 
                            Vector3 position = new Vector3(x, z, y);
                            // ei anneta palikoille yhtään kiertoa
                            Quaternion rotation = new Quaternion();
                            // luodaan palikka kohtaan x,y,z - 0 kierrolla.
                            Instantiate(GameAreaCubeOrange, position, rotation);
                        }
                        else {
                        }
                    }
                }
            }
            Debug.Log("GameArea drawn successfully. " +arraySize + " loops. Total cubes left: " + tempCount);
            // palikan sijainti x,y,z koordinaatistossa 
            Vector3 p = new Vector3(currentW / 2 , -5.5f, currentH / 2 );
            // ei anneta palikoille yhtään kiertoa
            Quaternion r = new Quaternion();
            // keskipilari
            Instantiate(MiddlePillarContainer, p, r);
            // jos kaikki palikat KABOOM
			if (tempCount == 0) {
				lvlComplete = true;
				Level++;
				//Debug.Log("Level Complete!");
				gc.GameState = 7;
				gc.ToggleUIChange ();
			} 
			else {
				lvlComplete = false;
			}
        }
        else{
            Debug.Log("GameArea creating failed!");
        }
    }
    // tägätään tällä tile
    public void SetMapTile(int x, int y, int z, int value) {
        //Debug.Log("Got from player: "+x+", "+y+", "+z);
        try {
            gamearea[x, y, z] = value;
            //RefreshGameArea();
            Debug.Log("Created tagged tile: "+ x + ", " + y + ", " + z+" value:" +value);
        } catch(Exception e) {
            Debug.Log("Error: " + e);
            Debug.Log("Tried: " + x + ", " + y + ", " + z+ " value:" + value);
        }
    }
    // palauttaa yhden kerroksen alla olevan tilen statuksen
    public int GetTileStatus(int x, int y, int z) {
        if (x >= 0 && x < currentW && y >= 0 && y < currentH && z >= 1 && z <= currentR)
            return gamearea[x, y, z - 1];
        else
            return 0;

    }

    public Vector3 FindSpawnTile() {
        Vector3 temp = new Vector3();
        for (int z = currentR-1; z >= 0; z--) {
            for (int x = 0; x < currentW; x++) {
                for (int y = 0; y < currentH; y++) {
                    //Debug.Log("Trying@ " + x + "," + z + "," + y);
                    if (gamearea[x, y, z] != 0) {
                        temp = new Vector3(x, z+1, y);
                        Debug.Log("Falling -> Found free tile@ " + x + "," + z + ","+y);
                        return temp;
                    }
                    else
						Debug.Log("Falling -> Could not find cubes!");
                }
            }
        }
        return temp;
    }

    // räjäyttää valitun pelaajan tagatut tilet ja palauttaa monta pistettä pelaaja saa
    public int DetonateTiles(int player) {
        int temp = 0;
        for (int x = 0; x < currentW; x++) {
            for (int y = 0; y < currentH; y++) {
                for (int z = 0; z < currentR; z++) {
                    if(gamearea[x,y,z] == player + 1) {
                        gamearea[x,y,z] = 0;
                        temp++;
                    }
                }
            }
        }
        RefreshGameArea();
        //Debug.Log("Detonating " + temp + " tiles. Adding to player " + player);
        return temp;
    }

    int CheckConnectedTiles() {
        int temp = 0;
        return temp;
    }

    public Vector3 ResetPlayerPosition() {
        return new Vector3(0, rows, 0);
    }
    
    public int GetWidth() {
        return currentW;
    }
    public int GetHeight() {
        return currentH;
    }
    // palauttaa kerroksien lukumäärän
    public int GetRows() {
        return currentR;
    }
    // tarkistetaan onko palikka taulokon kohdasta XYZ vapaana
    public bool IsTileFree(int x, int y, int z) {
        if (x < currentW && y < currentH && z < currentR && x >=0 && y >= 0 && z >= 0) {
            if (gamearea[x, y, z] == 0)
                return true;
            else
                return false;
        }
        else 
            return true;
    }
}
