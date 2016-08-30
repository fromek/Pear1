using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    public GameObject  explosion;        // Prefab of explosion effect.
    public ParticleSystem[] effects;
    public int Damage = 1;

    void Start()
    {
        // Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
        Destroy(gameObject, 2);
    }


    void OnExplode()
    {
        // Create a quaternion with a random rotation in the z-axis.
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

        // Instantiate the explosion where the rocket is with the random rotation.
        object obj = Instantiate(explosion, transform.position, randomRotation);
        //   foreach (var effect in effects)
        //   {
        //       effect.transform.parent = null;
        //       effect.Emit(1);
        ////       Destroy(effect.gameObject, 1.0f);
        //   }
        Destroy((GameObject)obj,1.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // If it hits an enemy...
        if (col.tag == "Enemy")
        {
            if (!col.GetComponent<Enemy>().IsFriend)
            {
                // ... find the Enemy script and call the Hurt function.
                col.gameObject.GetComponent<Enemy>().Hurt(Damage);

                // Call the explosion instantiation.
                OnExplode();

                // Destroy the rocket.
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.tag == "Obstacle" && col.gameObject.GetComponent<ObstacleHealth>() != null)
        {
            col.gameObject.GetComponent<ObstacleHealth>().Hurt(Damage);
            OnExplode();

            // Destroy the rocket.
            Destroy(gameObject);
        }
        // Otherwise if the player manages to shoot himself...
        else if (col.gameObject.tag != "Player")
        {
            // Instantiate the explosion and destroy the rocket.
            OnExplode();
            Destroy(gameObject);
        }
        
    }
}

