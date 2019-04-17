using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirbMovement : MonoBehaviour {

	public enum JumpState {
		Ready,
		Animating,
		Jumped
	}
	public bool isMoving;

	public ParticleSystem sparks;

	public float movementSpeed = 10.0f;
	public float jumpSpeed = 20.0f;
	public float sideJumpSpeed = 15.0f;
	public float sideMovementSpeed = 0.35f;

	public Transform foot;

	public Transform[] points;

	public Image radigullPicture;
	public Image dovelyPicture;
	public Image owlstandingPicture;


	public LayerMask ground;

	private int branch = 0;
	public float branchDistance = 1f;

	private int movingDirection = 0;
	// 0 = not, 1 = left, 2 = right

	private float desiredZ;

	private Rigidbody rb;

	public Animator anim;

	public JumpState jumpState;

	public Text scoreText;
	public Text gameOverText;

	public int score;

	private bool turnedOnPicture = false;


	void Awake() {
		rb = GetComponent<Rigidbody> ();
	}

	public bool CanJump {
		get {
			return jumpState == JumpState.Ready && !isMoving;
		}
	}

	// Use this for initialization
	void Start () {
		desiredZ = 0;
		isMoving = false;
		score = 0;
		radigullPicture.enabled = false;
		dovelyPicture.enabled = false;
		owlstandingPicture.enabled = false;
		InvokeRepeating("AddValue", 1, 1); // function string, start after float, repeat rate float
		InvokeRepeating("checkText", 1, 1); // function string, start after float, repeat rate float
		SetScoreText ();

	}

	void AddValue () {
		if (!GManager.Instance.IsGameOver) {
			score++;
		}

	}

	void checkText() {
		if (turnedOnPicture) {
			turnedOnPicture = false;
		} else if (radigullPicture.enabled == true) {
			radigullPicture.enabled = false;
		} else if (dovelyPicture.enabled == true) {
			dovelyPicture.enabled = false;
		} else if (owlstandingPicture.enabled == true) {
			owlstandingPicture.enabled = false;
		} else {} // all is well
	}

	// Update is called once per frame
	void Update () {
		SetScoreText ();
		if (Input.GetKey(KeyCode.UpArrow) && CanJump)
		{	
			jumpState = JumpState.Animating;
			StartCoroutine (Jump (0));
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			if ((branch == -1 || branch == 0) && CanJump) {
				jumpState = JumpState.Animating;
				StartCoroutine (Jump (1));
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			if ((branch == 0 || branch == 1) && CanJump) {
				jumpState = JumpState.Animating;
				StartCoroutine (Jump (-1));
			}
		}
	}

	void FixedUpdate() {
		rb.AddForce(new Vector3 (movementSpeed, 0, 0), ForceMode.Impulse);

		if (Physics.OverlapSphere (foot.position, 0.2f, ground).Length > 0) {
			if (jumpState == JumpState.Jumped) {
				jumpState = JumpState.Ready;
				anim.SetTrigger ("isLanding");
				sparks.Play();
				StartCoroutine (screenShake ());
			}
		} else if(jumpState != JumpState.Jumped) {
			jumpState = JumpState.Jumped;
			sparks.Stop();
		}

		if (isMoving) {
			
			if (Mathf.Abs (transform.position.z - desiredZ) < 0.3) {
				isMoving = false;
				movingDirection = 0;
				rb.MovePosition (new Vector3 (this.transform.position.x, this.transform.position.y, desiredZ));
			} else {
				if (movingDirection == 1) { // move left
					rb.AddForce (new Vector3(0, 0, -1 * sideMovementSpeed), ForceMode.Impulse);
				} else {
					rb.AddForce (new Vector3(0, 0, sideMovementSpeed), ForceMode.Impulse);
				}
			}
		} else {
			rb.velocity = new Vector3 (rb.velocity.x, rb.velocity.y, 0);
			rb.MovePosition (new Vector3 (this.transform.position.x, this.transform.position.y, desiredZ));
		}

		rb.velocity = Vector3.ClampMagnitude (rb.velocity, 12);
			
	}

	IEnumerator Jump(int direction){
		anim.SetTrigger ("isJumping");
		for (int i = 0; i < 6; i++) {
			yield return null;
		}

		if (Physics.OverlapSphere (points [0].position, 0.2f, ground).Length == 0) {
			turnedOnPicture = true;
			radigullPicture.enabled = true;
			score += 20;
			if (owlstandingPicture.enabled == true || dovelyPicture.enabled == true) {
				owlstandingPicture.enabled = false;
				dovelyPicture.enabled = false;
			}
			// Radigull!
		} else if (Physics.OverlapSphere (points [1].position, 0.2f, ground).Length == 0) {
			turnedOnPicture = true;
			owlstandingPicture.enabled = true;
			score += 10;
			if (dovelyPicture.enabled == true || radigullPicture.enabled == true) {
				dovelyPicture.enabled = false;
				radigullPicture.enabled = false;
			}
			// Owlstanding!
		} else if (Physics.OverlapSphere (points [2].position, 0.2f, ground).Length == 0) {
			turnedOnPicture = true;
			dovelyPicture.enabled = true;
			score += 5;
			if (owlstandingPicture.enabled == true || radigullPicture.enabled == true) {
				owlstandingPicture.enabled = false;
				radigullPicture.enabled = false;
			}
			// Dovely!
		} else {
			// Laaaaaaaaaaaaaaaaaaame
		}


		if (direction == -1) {
			branch--;
			movingDirection = 1;
			isMoving = true;
			rb.AddForce(new Vector3 (0, sideJumpSpeed, 0), ForceMode.Impulse);
			desiredZ = branch * branchDistance;
			
		} else if (direction == 0) {
			rb.AddForce(new Vector3 (0, jumpSpeed, 0), ForceMode.Impulse);
			isMoving = true;
			movingDirection = 0;
			
		} else if (direction == 1) {
			branch++;
			movingDirection = 2;
			isMoving = true;
			rb.AddForce(new Vector3 (0, sideJumpSpeed, 0), ForceMode.Impulse);
			desiredZ = branch * branchDistance;

		}
	}

	void SetScoreText ()
	// -2.05 = y
	// 22.7 = x
	// 5.67
	{
		float d = -this.transform.position.x / Mathf.Tan (80 * Mathf.Deg2Rad);
		if (this.transform.position.y < d - 4 && !GManager.Instance.IsGameOver) {
			//gameOverText.text = "Game Over!";
			GManager.Instance.IsGameOver = true;
		} else {
			scoreText.text = "Score: " + score.ToString ();
		}
	}

	IEnumerator screenShake() {
		ScreenShake.ScreenShakeIntensity = 0.15f;
		while (ScreenShake.ScreenShakeIntensity > 0) {
			yield return null;
			ScreenShake.ScreenShakeIntensity -= 0.05f;
		}
	}

}
