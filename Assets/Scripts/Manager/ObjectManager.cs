using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public CollisionEventSc[] colEvents;
    public List<Enemy1> enemys = new List<Enemy1>();
    //public SelfMoveObs[] selfObs;

    public void ObsReset()
    {
        int i;

        for(i=0; i<colEvents.Length; i++)
        {
            colEvents[i].gameObject.SetActive(true);
            colEvents[i].ObsReset();
        }

        for(i=0; i<enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(true);
            enemys[i].ResetData();
        }
    }
}
