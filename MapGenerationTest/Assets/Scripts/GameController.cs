using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	private int gameState;
    // Tämä skripti hoitaa pelin aloituksen, kentän vaihdot, jne...
    // gameState = 
    //              0, main menu
    //              1, start new game
    //              2, paused
    //              3, continue game
    //              4, help
    //              5, credits
    //              6, exit
    //              7, level complete
    //              8, restart level
    //              9, game over
	// 				11, start new game menu
	// 				12, level complete

    // pidetään kirjaa mikä kenttä käynnissä
    private int level;
	// tarvittavat UI komponenti
    public GameObject MainMenu;
    public GameObject ContinueGameButton;
    public GameObject HelpMenu;
    public GameObject CreditsMenu;
	public GameObject LevelComplete;
	public GameObject player1UI;
	public GameObject player2UI;
	public GameObject player3UI;
	public GameObject player4UI;
	public GameObject StartNewGameMenu;
	// tarvittavat skriptit
    public MapGeneration map;
    public ObjectMover om;
    public PlayerTracker playerTracker;
	// muutetaan pelaajien lukumäärää
	int playerCount;

	private bool uiRepaint;
	// haetaan/muutetaan pelin tilaa
    public int GameState {
        get {
            return gameState;
        }

        set {
            gameState = value;
        }
    }

    // Use this for initialization
    void Start () {
		uiRepaint = true;
		playerCount = 0;
		activePlayers ();
        gameState = 0;
        level = 0;
	}
	public void ToggleUIChange(bool optional = true){
		uiRepaint = optional;
	}

	// funktio valikoiden vaihtamiseen, valitaan mitkä valikoista piirretään parametreilla
	// piirretään vain ne valikot mitkä tarvitaan
	void ShowUIElements(int mainMenu = 0, int helpMenu = 0, int creditsMenu = 0, int startNewGame = 0, int continueGame = 0, int lvlComplete = 0){
		if (uiRepaint) {
			MainMenu.SetActive (false);
			HelpMenu.SetActive (false);
			CreditsMenu.SetActive (false);
			StartNewGameMenu.SetActive (false);
			ContinueGameButton.SetActive (false);
			LevelComplete.SetActive (false);
			if (mainMenu == 1)
				MainMenu.SetActive (true);
			if (helpMenu == 1)
				HelpMenu.SetActive (true);
			if (creditsMenu == 1)
				CreditsMenu.SetActive (true);
			if (startNewGame == 1)
				StartNewGameMenu.SetActive (true);
			if (continueGame == 1)
				ContinueGameButton.SetActive (true);
			if (lvlComplete == 1)
				LevelComplete.SetActive (true);
			Debug.Log (mainMenu + " " + helpMenu + " " + creditsMenu + " " + startNewGame + " " + continueGame + " " + lvlComplete);
			ToggleUIChange (false);
		}
	}
		
	// Update is called once per frame
	void Update () {
		// pelin pääsilukka
        switch (gameState) {
		case 0: // main menu
				Time.timeScale = 0.0f;
				ShowUIElements (1);
                break;
			case 1: // start new menu
				ShowUIElements (0,0,0,1);
                break;
            case 2: // paused 
                Time.timeScale = 0.0f;
				ShowUIElements (1,0,0,0,1);
                break;
            case 3: // continue game
                Time.timeScale = 1.0f;
				ShowUIElements ();
                break;
			case 4: // help
				Time.timeScale = 0.0f;
				ShowUIElements (0, 1);
				if (Input.GetKeyDown (KeyCode.Escape))
					gameState = 0;
                break;
            case 5: // credits
                Time.timeScale = 0.0f;
				ShowUIElements (0,0,1);
				if (Input.GetKeyDown (KeyCode.Escape)) 
					gameState = 0;
                break;
            case 6: // exit
                Debug.Log("Exit pressed!");
                Application.Quit();
                break;
			case 7: // next level
				Debug.Log ("Level cleared!");
				Time.timeScale = 0.0f;
				ShowUIElements (0,0,0,0,0,1);
                break;
			case 8: // restart level
				map.RestartGame ();
				gameState = 9;
                break;
            case 9: // game over
				// joku spalsh image GAME OOVAA?
                Debug.Log("Game over!");
                gameState = 0;
                playerTracker.SetLives(3);
                break;
			case 11: // start new game
				Time.timeScale = 1.0f;
				ShowUIElements ();
					// jos peliä ei olla vielä aloitettu
				if (!map.isGameStarted ()) {
					map.StartGame ();
					om.player.transform.transform.position = map.FindInitialSpawnPoints (1);
				}
					// jos siirrytään seuraavaan kenttään
				if (map.isGameStarted () && map.isLevelComplete ()) {
					map.StartGame ();
					om.player.transform.transform.position = map.FindInitialSpawnPoints (1);
				} // monta pelaaja tulee peliin
				
				activePlayers ();
				break;
        }
	}
	// asetetaan pelaajien lukumäärä
	public void setPlayers(int value){
		playerCount = value;
	}
	// pelaajien lukumäärän mukaan piirretään pelaajien UI:t
	public void activePlayers(){
		
		if ((playerCount < 1 || playerCount > 4) && (gameState != 0 && gameState != 1 && gameState != 11))
			playerCount = 1;
		switch (playerCount) {
			case 1:
				player1UI.SetActive (true);
				player2UI.SetActive (false);
				player3UI.SetActive (false);
				player4UI.SetActive (false);
				break;
			case 2:
				player1UI.SetActive (true);
				player2UI.SetActive (true);
				player3UI.SetActive (false);
				player4UI.SetActive (false);
				break;
			case 3:
				player1UI.SetActive (true);
				player2UI.SetActive (true);
				player3UI.SetActive (true);
				player4UI.SetActive (false);
				break;
			case 4:
				player1UI.SetActive (true);
				player2UI.SetActive (true);
				player3UI.SetActive (true);
				player4UI.SetActive (true);
				break;
		}
	}
}