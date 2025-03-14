using UnityEngine;

public class FootCollision : MonoBehaviour
{
    public string footName = "Left";
    public float rayDistance = 0.2f;

    public bool isGrounded = false;

    void Update()
    {
        CheckFootGrounding();
    }

    void CheckFootGrounding()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            Debug.Log("Pie: "+footName);
            if (!isGrounded)
            {
                isGrounded = true;

                Debug.Log($"[{footName}] Raycast tocó: {hit.collider.gameObject.name}");
                
                // Sonido jejeje
                // audioManager.instance.PlayAtPosition("EnemyStep", transform.position);
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);
    }


}
