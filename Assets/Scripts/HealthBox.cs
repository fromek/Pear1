using UnityEngine;
using System.Collections;

public class HealthBox : MonoBehaviour {

    public int HP;

    void OnCollisionEnter2D(Collision2D col)
    {
        // If the colliding gameobject is an Palyer...
        if (col.gameObject.tag == "Player")
        {
            HeroHealth layBombs = col.gameObject.GetComponent<HeroHealth>();
            if (layBombs != null)
            {
                layBombs.health += HP;
                Destroy();

            }

        }else if (col.gameObject.tag == "Enemy")
        {
            Destroy();
        }
    }

    private void Destroy()
    {
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
