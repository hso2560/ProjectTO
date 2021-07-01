using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order =int.MaxValue)]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;

    public string enemyName;

    public int maxHp;

    public float speed;
    public float traceSpeed;
    public float patrolDist;
    public float traceDist;
    public float attackRange;
    public float viewingAngle;
}

public enum EnemyType
{
    Normal
}
