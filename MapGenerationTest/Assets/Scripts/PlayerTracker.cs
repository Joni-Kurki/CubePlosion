using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerTracker : MonoBehaviour {

    public Text playerScoreText;
    public Text playerTaggingSW;
    public MapGeneration map;

    public GameController gc;

	public Sprite life_sprite_orange;
	// pelaajan attribuutteja
    private int score;
    private bool tagging;
    private int lives;
	// elämä "laatikon" koko
	private int lifeSpriteSize = 16;
	// onko tekoäly pelaaja vai ei
	public bool isAi;

	public int playerNumber;

    // Use this for initialization
    void Start () {
        tagging = false;
        SetScoreText();
        SetTaggingText();
        lives = 3;
    }

	public int getPlayerNumber(){
		return playerNumber;
	}

    void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.A)) {
            TaggingSwitch();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            SetScoreText(map.DetonateTiles(1));
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) {
            gc.GameState = 2;
			gc.ToggleUIChange ();
        }
    }
    void Update() {
        SetTaggingText();
        SetScoreText();
    }

    // päivitetään pisteet
    void SetScoreText() {
        playerScoreText.text = "Score: " + score.ToString();    
    }
    void SetScoreText(int value) {
        score += value;
        playerScoreText.text = "Score: " + score.ToString();
    }
    // päivitetään täggayksen tila
    void SetTaggingText() {
        if (tagging)
            playerTaggingSW.text = "Tagging ON";
        else
            playerTaggingSW.text = "Tagging OFF";
    }
    // vaihdetaan tagaaminen true -> false, false -> true
    void TaggingSwitch() {
        tagging = !tagging;
    }
    // haetaan täggayksen arvo
    public bool GetTaggingSW() {
        return tagging;
    }
	// jos pelaaja kuolee
    public void Died() {
		if (lives > 1) { 
			lives--;
			Debug.Log ("-1 life. Remaining " + lives);
		} 
		else {
			score = 0;
			gc.GameState = 8;
			gc.ToggleUIChange ();
		}
    }
	// muutetaan elämien arvoa
    public void SetLives(int value) {
        lives = value;
    }
	// piirtää monta elämää jäljellä
	void OnGUI(){
		if (map.isGameStarted ()) {
			Texture t = life_sprite_orange.texture;
			Rect tr = life_sprite_orange.textureRect;
			for (int i = 0; i < lives; i++) {
				GUI.DrawTexture (new Rect (40 + (i * lifeSpriteSize+ 3), 65+(100*(playerNumber-1)), lifeSpriteSize, lifeSpriteSize), t);
			}
		}
	}
}