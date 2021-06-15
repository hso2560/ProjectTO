
using UnityEngine;

public class DeathCol : MonoBehaviour
{
    public string deathCause;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.player.gameObject)
        {
            other.GetComponent<PlayerScript>().Die(deathCause);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            collision.transform.GetComponent<PlayerScript>().Die(deathCause);
        }
    }
}
