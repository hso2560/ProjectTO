using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    [SerializeField] PlayerScript player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&other.gameObject!=player.gameObject)
        {
            NetManager.instance.HitPlayer(player.playerId,other.GetComponent<PlayerScript>().playerId, player.damage);
        }
    }
}