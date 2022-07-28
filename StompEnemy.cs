﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour {

	public GameObject deathSplosion;  // andere für jeden gegner machen!!!!

	private Rigidbody2D playerRigidbody;

	public float bounceForce;

	// Use this for initialization
	void Start () {

		playerRigidbody = transform.parent.GetComponent<Rigidbody2D> ();

		
	}
	
	// Update is called once per frame
	void Update () {



		
	}

	void OnTriggerEnter2D(Collider2D other)

	{
		if (other.tag == "Enemy") 
		{
			//Destroy (other.gameObject);

			other.gameObject.SetActive (false);

			Instantiate (deathSplosion, other.transform.position, other.transform.rotation);

			playerRigidbody.velocity = new Vector3 (playerRigidbody.velocity.x, bounceForce, 0f);
		}

		if (other.tag == "Boss") 
		{

			playerRigidbody.velocity = new Vector3 (playerRigidbody.velocity.x, bounceForce, 0f);
			other.transform.parent.GetComponent<Boss> ().takeDamage = true;
		}

	}



}
