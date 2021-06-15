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

        IDamageable dobj = other.GetComponent<IDamageable>();
        if (dobj != null)
        {
            dobj.Damaged(player.damage, other.transform.position, -player.transform.forward);
        }
    }
}
