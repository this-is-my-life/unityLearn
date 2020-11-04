using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
  private int jumped = 0;
  private Rigidbody body;
  private Camera mainCamera;

  void Start () {
    body = GetComponent<Rigidbody>();
    mainCamera = Camera.main;
    mainCamera.enabled = true;
  }

  void Update () {
    if (jumped > 1) return;

    if (Input.GetKeyDown(KeyCode.Space)) {
      body.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
      jumped++;
    }

    if (Input.GetKeyDown(KeyCode.R)) respawn();

    float horizontalAxis = Input.GetAxis("Horizontal");
    float verticalAxis = Input.GetAxis("Vertical");
    body.velocity = new Vector3(horizontalAxis, body.velocity.y, verticalAxis);
  }

  void FixedUpdate () {
    mainCamera.transform.position = new Vector3(body.position.x, body.position.y, -7);

    if (body.position.y < 0) respawn();
  }

  void respawn () {
    body.transform.position = new Vector3(0, 4, 0);
    body.velocity = new Vector3(0, 0, 0);
  }

  void OnCollisionEnter (Collision collision) {
    switch (collision.gameObject.tag) {
      case "finish": {
        int curr = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++curr);
        break;
      }

      case "refuel": {
        jumped = 0;
        break;
      }

      case "killer": {
        respawn();
        break;
      }
    }
  }
}
