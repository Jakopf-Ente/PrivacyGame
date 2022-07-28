using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public float waitToRespawn;
	public PlayerController thePlayer;
	public GameObject deathSplosion;

	public RuntimeAnimatorController dressedController;
	public RuntimeAnimatorController shirtController; 
	public RuntimeAnimatorController shortController;
	public RuntimeAnimatorController nakedController;

	public int coinCount;
	private int coinBonusLifeCount;
	public int bonusLifeTreshold;

	public Text coinText;
	public AudioSource coinSound;

	public Image heart1;
	public Image heart2;
	public Image heart3;

	public Sprite heartFull;
	public Sprite heartHalf;
	public Sprite heartEmpty;

	public int maxHealth;
	public int healtCount;

	private bool respawning;

	public ResetOnRespawn[] objectsToReset;

	public bool invincible;

	public Text livesText;
	public int startingLives;
	public int currentLives;

	public GameObject gameOverScreen;

	public AudioSource levelMusic;

	public AudioSource gameOverMusic;

	public AudioSource bossMusic;

	public bool respawnCoActive;


	// Use this for initialization
	void Start () {

		thePlayer = FindObjectOfType<PlayerController> ();





		healtCount = maxHealth;

		objectsToReset = FindObjectsOfType<ResetOnRespawn> ();


		if (PlayerPrefs.HasKey ("CoinCount")) 
		{

			coinCount = PlayerPrefs.GetInt ("CoinCount");
		}


		coinText.text = "x " + coinCount;


		if (PlayerPrefs.HasKey ("PlayerLives")) {

			currentLives = PlayerPrefs.GetInt ("PlayerLives");

		} else 
		{
			currentLives = startingLives;
		}

		livesText.text = "x " + currentLives;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (healtCount <= 0) 
		
		{
			Respawn ();

		}

		if (coinBonusLifeCount >= bonusLifeTreshold) 
		
		{
			currentLives += 1;
			livesText.text = "x " + currentLives;
			coinBonusLifeCount -= bonusLifeTreshold;
		}

	}

	public void Respawn ()

	{
		if (!respawning) {

			currentLives -= 1;
			livesText.text = "x " + currentLives;

			if (currentLives > 0) {
				respawning = true;
				StartCoroutine ("RespawnCo");
			} else {
				thePlayer.gameObject.SetActive (false);
				gameOverScreen.SetActive (true);
				//levelMusic.Stop ();

				levelMusic.volume = levelMusic.volume / 6f;

				gameOverMusic.Play ();

			}
		}
	}

	// Coroutine. makes a delay in a second world. doenst interrupt the normal code.
	public IEnumerator RespawnCo()

	{
		respawnCoActive = true;

		thePlayer.gameObject.SetActive (false);

		// create an object in the world.
		Instantiate (deathSplosion, thePlayer.transform.position, thePlayer.transform.rotation);


		yield return new WaitForSeconds (waitToRespawn);

		respawnCoActive = false;

		healtCount = maxHealth;
		respawning = false;
		UpdateHeartMeter ();

		coinCount = 0;
		coinText.text = "x " + coinCount;
		coinBonusLifeCount = 0;

		thePlayer.transform.position = thePlayer.respawnPosition;
		thePlayer.gameObject.SetActive (true);

		for (int i = 0; i < objectsToReset.Length; i++) 
		{
			
			objectsToReset[i].gameObject.SetActive(true);
			objectsToReset[i].ResetObject ();

		}
			

	}

	public void AddCoins(int coinsToAdd)

	{

		coinCount += coinsToAdd;
		coinBonusLifeCount += coinsToAdd;

		coinText.text = "x " + coinCount;

		coinSound.Play ();

	}

	public void HurtPlayer (int damageToTake)

	{
			if (!invincible) 
			{

				healtCount -= damageToTake;
				UpdateHeartMeter ();

				thePlayer.Knockback ();

				thePlayer.hurtSound.Play();

			}
	}

	public void GiveHealth(int healthToGive)
	{
		healtCount += healthToGive;

		if (healtCount > maxHealth) 
		{
			healtCount = maxHealth;
		}

		coinSound.Play ();

		UpdateHeartMeter ();
	}

	public void UpdateHeartMeter ()

	{
		switch (healtCount) 

		{
		case 6:
			heart1.sprite = heartFull;    //in the case the health count is 6 then ... 
			heart2.sprite = heartFull;
			heart3.sprite = heartFull;
			thePlayer.myAnim.runtimeAnimatorController = dressedController as RuntimeAnimatorController;
			
			return;


		case 5:
			heart1.sprite = heartFull;    //in the case the health count is 5 then ... 
			heart2.sprite = heartFull;
			heart3.sprite = heartHalf;
			thePlayer.myAnim.runtimeAnimatorController = dressedController as RuntimeAnimatorController;
			return;

		case 4:
			heart1.sprite = heartFull;    //in the case the health count is 4 then ... 
			heart2.sprite = heartFull;
			heart3.sprite = heartEmpty;
			thePlayer.myAnim.runtimeAnimatorController = shirtController as RuntimeAnimatorController;
			return;

		case 3:
			heart1.sprite = heartFull;    //in the case the health count is 3 then ... 
			heart2.sprite = heartHalf;
			heart3.sprite = heartEmpty;
			thePlayer.myAnim.runtimeAnimatorController = shirtController as RuntimeAnimatorController;
			return;

		case 2:
			heart1.sprite = heartFull;    //in the case the health count is 2 then ... 
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			thePlayer.myAnim.runtimeAnimatorController = shortController as RuntimeAnimatorController;
			return;

		case 1:
			heart1.sprite = heartHalf;    //in the case the health count is 1 then ... 
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			thePlayer.myAnim.runtimeAnimatorController = nakedController as RuntimeAnimatorController;
			return;

		case 0:
			heart1.sprite = heartEmpty;    //in the case the health count is 0 then ... 
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			thePlayer.myAnim.runtimeAnimatorController = nakedController as RuntimeAnimatorController;
			return;

		default: 
			heart1.sprite = heartEmpty;    
			heart2.sprite = heartEmpty;
			heart3.sprite = heartEmpty;
			
			return;
			
		}
	}

	public void AddLives(int livesToAdd)
	{
		coinSound.Play ();

		currentLives += livesToAdd;
		livesText.text = "x " + currentLives;
	}


}

	
