using UnityEngine;

public abstract class CollisionEventSc : MonoBehaviour
{
    public int id;
    public GameObject o;
    public Vector3 pos, scl;
    public float posT, sclT;

    public Vector3 firScl, firPos;

    public abstract void CollisionFunc();
    public abstract void ObsReset();
}
