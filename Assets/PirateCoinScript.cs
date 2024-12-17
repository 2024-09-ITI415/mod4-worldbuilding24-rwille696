using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCoinScript : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed for spinning

    void Update()
    {
        // Make the coin spin slowly over time
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the player
        if (other.CompareTag("Player"))
        {
            // Notify the Game Manager that a coin was collected
            GameManager.instance.CollectCoin();

            // Destroy the coin
            Destroy(gameObject);
        }
    }
}