using UnityEngine;

public class TrmInfo
{
    public Vector3 position;
    public Quaternion rotation;

    public TrmInfo(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
