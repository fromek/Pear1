using UnityEngine;
using System.Collections;

public class BombBox : MonoBehaviour {

    public int BombCount;

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the colliding gameobject is an Palyer...
        if (col.gameObject.tag == "Player")
        {
            LayBombs layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
            if (layBombs != null)
            {
                layBombs.bombCount += BombCount;
            }


            // Find all of the sprite renderers on this object and it's children.
            SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

            // Disable all of them sprite renderers.
            foreach (SpriteRenderer s in otherRenderers)
            {
                s.enabled = false;
            }

            // Find all of the colliders on the gameobject and set them all to be triggers.
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = true;
            }

        }
    }
}
