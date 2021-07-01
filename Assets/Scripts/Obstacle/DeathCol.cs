
using UnityEngine;

public class DeathCol : MonoBehaviour
{
    public string deathCause;
    public string deathCause_en;

    private string dc;

    private void Start()
    {
        dc = (int)GameManager.Instance.savedData.option.language == 0 ? deathCause_en : deathCause;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameManager.Instance.player.gameObject)
        {
            other.GetComponent<PlayerScript>().Die(dc);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            collision.transform.GetComponent<PlayerScript>().Die(dc);
        }
    }
}
