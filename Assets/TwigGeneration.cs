using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwigGeneration : MonoBehaviour {

	public GameObject twig;
	public float length = 4f;
	public float movementZ = 1f;
	public float distance = 40f;

	private int location = -5;

	public GameObject birb;

	private int currentRoute = 0;

	private List<GameObject> myTwigs;

	// Use this for initialization
	void Start () {
		myTwigs = new List<GameObject> ();
		for (int i = 0; i < 10; i++) {
			GameObject t = createTwig ();
			placeTwig (t);
		}
		StartCoroutine (TwigGenerator ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject createTwig () {
		GameObject twigg = Instantiate (twig);
		myTwigs.Add (twigg);
		return twigg;
	}

	GameObject moveTwig (){
		GameObject twigg = myTwigs [0];
		myTwigs.RemoveAt (0);
		myTwigs.Add (twigg);
		return twigg;
	}

	void placeTwig(GameObject twigg) {
		int branch = 0;
		if (currentRoute == 0) {
			branch = Random.Range (-1, 2);
		} else if (currentRoute == -1) {
			branch = Random.Range (-1, 1);
		} else {
			branch = Random.Range (0, 2);
		}
		currentRoute = branch;

		twigg.transform.position = new Vector3 (length * location * Mathf.Cos (Mathf.Deg2Rad * 10), -length * location * Mathf.Sin (Mathf.Deg2Rad * 10), branch * movementZ);
		location++;
	}

	IEnumerator TwigGenerator() {
		while (true) {
			GameObject twig = null;
			//myTwigs[0].transform.position.x > birb.transform.position.x
			if (myTwigs.Count < 15 || myTwigs [0].transform.position.x + distance > birb.transform.position.x) {
				if (Mathf.Abs(length * location * Mathf.Cos (Mathf.Deg2Rad * 10) - birb.transform.position.x) < distance) {
						twig = createTwig ();
				}
			} else {
					twig = moveTwig ();
			}
			if (twig != null) {
				placeTwig (twig);
			}
			yield return new WaitForSeconds (0.1f);
		}
	}
}
