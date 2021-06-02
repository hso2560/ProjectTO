using UnityEngine;

public abstract class CollisionEventSc : MonoBehaviour
{
    #region 건들지 말자
    public int id;
    public GameObject o;  
    public Vector3 pos, scl;  
    public float posT, sclT;
    #endregion

    protected Vector3 firScl, firPos;

    public abstract void CollisionFunc();
    public abstract void ObsReset();
}
