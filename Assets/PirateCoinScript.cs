using UnityEngine;

public class PirateCoinScript : MonoBehaviour
{
    public float rotationSpeedX = 50f; // Speed for spinning on the X-axis
    public float rotationSpeedY = 50f; // Speed for spinning on the Y-axis

    private AudioSource pickupSound; // Reference to the AudioSource

    void Start()
    {
        // Get the AudioSource component attached to the coin
        pickupSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Make the coin spin on the X and Y axes
        transform.Rotate(Vector3.right, rotationSpeedX * Time.deltaTime); // X-axis spin
        transform.Rotate(Vector3.up, rotationSpeedY * Time.deltaTime);    // Y-axis spin
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player touched the coin
        if (other.CompareTag("Player"))
        {
            // Play the pickup sound
            pickupSound.Play();

            // Notify the Game Manager that a coin was collected
            GameManager.instance.CollectCoin();

            // Disable the coin visuals and collider while the sound plays
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Destroy the coin GameObject after the sound finishes playing
            Destroy(gameObject, pickupSound.clip.length);
        }
    }
}