using UnityEngine;

public class BulletPickUp : MonoBehaviour {

    public PlayerScript playerScript;
    [SerializeField] private int bulletPickUpCount = 25;
    [SerializeField] private float rotateSpeed = 200f;

    private void Start() {

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.tag.Equals(playerScript.tag)) {
            playerScript.AddBullets(bulletPickUpCount);
            Destroy(gameObject);
        }

    }

    private void Update () {

        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);

    }

}
