using UnityEngine;
public interface IDamageable
{
    public void Damaged(int damage, Vector3 hitPos, Vector3 hitNormal);
}
