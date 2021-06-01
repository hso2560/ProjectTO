using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public CollisionEventSc[] colEvents;

    public void ObsReset()
    {
        for(int i=0; i<colEvents.Length; i++)
        {
            colEvents[i].gameObject.SetActive(true);
            colEvents[i].ObsReset();
        }
    }
}
