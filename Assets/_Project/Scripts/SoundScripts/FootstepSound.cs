using System;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public string footName = "Left";
    
    public float rayDistance = 0.2f;
    
    private float _volume = 1.0f;
    private float _timeBetweenSteps = 1f; 
    private AudioSource _enemyAudioSource;
    private float _stepCooldown = 0f;
    private bool _wasGroundedLastFrame = false;

    private AudioClip[] stepClips;
    private void Start()
    {
        stepClips  = Resources.LoadAll<AudioClip>("Sounds/Kharisiri_footsteps");
        
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        _volume = enemy.GetComponent<KharasiriSounds>().stepVolume;
        _enemyAudioSource = enemy.GetComponent<AudioSource>();
        
    }

    void Update()
    {
        _stepCooldown += Time.deltaTime;
        CheckFootGrounding();
    }

    void CheckFootGrounding()
    {
        RaycastHit hit;
        bool currentlyGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance);

        if (currentlyGrounded)
        {
            if (!_wasGroundedLastFrame && _stepCooldown > _timeBetweenSteps)
            {
                _stepCooldown = 0f;
                Debug.Log($"[{footName}] Paso");
                int randomIndex = UnityEngine.Random.Range(0, stepClips.Length);
                _enemyAudioSource.PlayOneShot(stepClips[randomIndex], _volume);
            }
        }

        _wasGroundedLastFrame = currentlyGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }


}
