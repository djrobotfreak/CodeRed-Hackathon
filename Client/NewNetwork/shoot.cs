using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class shoot : MonoBehaviour {
	// Rocket Prefab
	public GameObject rocketPrefab;
	public GameObject enemyPrefab;
	public GameObject e;
	public Vector3 cursor_main;
	public Quaternion movement_main;
	public Vector3 enemy_cursor;
	public Quaternion enemy_movement;
	public Networking network = new Networking();
	bool is_game = false;

	void Start () {
		network.Start ();
		network.Start ();
		e = (GameObject)Instantiate(enemyPrefab,
		                                       transform.position,
		                                       transform.parent.rotation);
		is_game = network.isGame;
	}
	// Update is called once per frame
	void Update () {
		// left mouse clicked?
		if (Input.GetMouseButtonDown(0)) {
			if (is_game) {
				enemy_cursor = e.GetComponent<Rigidbody>().transform.localPosition;
				enemy_movement = e.GetComponent<Rigidbody>().transform.localRotation;
			}
			GameObject g = (GameObject)Instantiate(rocketPrefab,
			                                       transform.position,
			                                       transform.parent.rotation);
			// make the rocket fly forward by simply calling the rigidbody's
			// AddForce method
			// (requires the rocket to have a rigidbody attached to it)
			float force = g.GetComponent<rocket>().speed;
			g.GetComponent<Rigidbody>().AddForce(g.transform.forward * force);

			cursor_main = g.GetComponent<Rigidbody>().transform.localPosition;
			movement_main = g.GetComponent<Rigidbody>().transform.localRotation;
		
			network.Shoot(cursor_main, movement_main, enemy_cursor, enemy_movement);

			Debug.Log(cursor_main);
			Debug.Log(movement_main);
			Debug.Log(enemy_cursor);
			Debug.Log(enemy_movement);
		}
	}
}