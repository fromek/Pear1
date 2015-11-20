using UnityEngine;
using System.Collections;

public class DerstroyOnContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D col)
    {
        // If the player hits the trigger...
        if (col.gameObject.tag == "Player")
        {
            // .. stop the camera tracking the player
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;
            // ... destroy the player.
            Destroy(col.gameObject);
            //showPanels.ShowMenu();
            StartCoroutine("ReloadGame");
        }
        else
        {

            // Destroy the enemy.
            Destroy(col.gameObject);
        }
    }
    IEnumerator ReloadGame()
    {
        // ... pause briefly
        yield return new WaitForSeconds(2);
        // ... and then reload the level.
        Application.LoadLevel("MainMenu");
    }
}
