using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour {
  private int jumped = 0;
  private Rigidbody body;
  private Camera mainCamera;
  private GameObject deathGUI;
  private int death = 0;

  void Start () {
    deathGUI = GameObject.Find("Canvas/Death");
    body = GetComponent<Rigidbody>();
    mainCamera = Camera.main;
    mainCamera.enabled = true;
  }

  void Update () {
    if (Input.GetKeyDown(KeyCode.R)) respawn();

    if (jumped > 1) GetComponent<Renderer>().material.color = new Color32(129, 13, 255, 255);
    else if (jumped > 0) GetComponent<Renderer>().material.color = new Color32(178, 12, 232, 255);
    else GetComponent<Renderer>().material.color = new Color32(255, 0, 242, 255);

    if (jumped > 1) return;

    if (Input.GetKeyDown(KeyCode.Space)) {
        body.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
        jumped++;
    }

    float horizontalAxis = Input.GetAxis("Horizontal") * (SceneManager.GetActiveScene().buildIndex == 0 ? 2 : 1);
    float verticalAxis = Input.GetAxis("Vertical");
    body.velocity = new Vector3(horizontalAxis, body.velocity.y, verticalAxis);
  }

  void FixedUpdate () {
    mainCamera.transform.position = new Vector3(body.position.x, body.position.y, -7);

    if (body.position.y < 0) respawn();
  }

  void respawn () {
    death++;
    deathGUI.GetComponent<TextMeshProUGUI>().SetText("Death: {0}", death);
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
