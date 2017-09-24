using UnityEngine;

/// <summary>
/// Creating instance of particles from code with no effort
/// </summary>
public class SpecialEffectsHelper : MonoBehaviour
{

    public ParticleSystem fireEffect;
    private GameObject dragon;

    private void Start()
    {
        dragon = GameObject.FindGameObjectWithTag("Dragon");
        
    }

    /// <summary>
    /// Create an explosion at the given location
    /// </summary>
    /// <param name="position"></param>
    public void Explosion(Vector3 position)
    { 

        // Tu tu tu, tu tu tudu

        // Fire in the sky
        instantiate(fireEffect, position);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("FIRE!!");
            Explosion(dragon.transform.position);
        }
    }

    /// <summary>
    /// Instantiate a Particle system from prefab
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(
          prefab,
          position,
          Quaternion.identity
        ) as ParticleSystem;

        // Make sure it will be destroyed
        Destroy(
          newParticleSystem.gameObject,
          newParticleSystem.startLifetime
        );

        return newParticleSystem;
    }
}