using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {


	public bool bossActive;

	public float timeBetweenDrops;
	private float timeBetweenDropStore;
	private float dropCount;

	public float waitForPlatforms;
	private float platformCount;

	public Transform lefPoint;
	public Transform rightPoint;
	public Transform dropSawSpawnPoint;

	public GameObject dropSaw;

	public GameObject theBoss;
	public bool bossRight;

	public GameObject rightPlatforms;

	public GameObject leftPlatforms;

	public bool takeDamage;

	public int startingHealth;

	private int currentHealth;

	public GameObject levelExit;

	private CameraController theCamera;

	private LevelManager theLevelManager;

	public bool waitingForRespawn;
	// Use this for initialization
	void Start () {

		timeBetweenDropStore = timeBetweenDrops;

		dropCount = timeBetweenDrops;
		platformCount = waitForPlatforms;
		currentHealth = startingHealth;

		theCamera = FindObjectOfType<CameraController> ();

		theBoss.transform.position = rightPoint.position;
		bossRight = true;

		theLevelManager = FindObjectOfType<LevelManager> ();


	}
	
	// Update is called once per frame
	void Update () {

		if (theLevelManager.respawnCoActive) 
		{

			bossActive = false;
			waitingForRespawn = true;
		}


		if (waitingForRespawn && !theLevelManager.respawnCoActive) 
		{

			theBoss.SetActive (false);
			leftPlatforms.SetActive (false);
			rightPlatforms.SetActive (false);

			timeBetweenDrops = timeBetweenDropStore;

			platformCount = waitForPlatforms;

			dropCount = timeBetweenDrops;

			theBoss.transform.position = rightPoint.position;
			bossRight = true;

			currentHealth = startingHealth;

			theCamera.followTarget = true;

			waitingForRespawn = false;

		}

		if (bossActive)
		{

			theCamera.followTarget = false;
			theCamera.transform.position = Vector3.Lerp (theCamera.transform.position, new Vector3 (transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z),theCamera.smoothing*Time.deltaTime);

			theBoss.SetActive (true);

			if (dropCount > 0) {
				dropCount -= Time.deltaTime;
			} else 
			
			{
				dropSawSpawnPoint.position = new Vector3 (Random.Range (lefPoint.position.x, rightPoint.position.x), dropSawSpawnPoint.position.y, dropSawSpawnPoint.position.z);

				Instantiate (dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
				dropCount = timeBetweenDrops;
			}

			if (bossRight) {
				if (platformCount > 0) {
					platformCount -= Time.deltaTime;
				} else {

					rightPlatforms.SetActive (true);
					}
			} else{
				if (platformCount > 0) {
					platformCount -= Time.deltaTime;
				} else {

					leftPlatforms.SetActive (true);
				}
			
			}

			if (takeDamage)
			{

				currentHealth -= 1;

				if (currentHealth <= 0) 
				{
					levelExit.SetActive (true);

					theCamera.followTarget = true;

					gameObject.SetActive (false);

					theLevelManager.bossMusic.Stop ();
					theLevelManager.levelMusic.Play ();
				}

				if (bossRight) {
					theBoss.transform.position = lefPoint.position;
				} else {

					theBoss.transform.position = rightPoint.position;

				}

				bossRight = !bossRight;

				rightPlatforms.SetActive (false);
				leftPlatforms.SetActive (false);

				platformCount = waitForPlatforms;

				timeBetweenDrops = timeBetweenDrops / 2f;


				takeDamage = false;

			}
		
		}

	}


	void OnTriggerEnter2D(Collider2D other)

	{
		if (other.tag == "Player") 
		{
			bossActive = true;
			theLevelManager.levelMusic.Stop ();
			theLevelManager.bossMusic.Play();

		}
	}

}
