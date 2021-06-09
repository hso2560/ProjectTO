
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public CollisionEventSc[] colEvents;
    //public SelfMoveObs[] selfObs;

    public void ObsReset()
    {
        int i;

        for(i=0; i<colEvents.Length; i++)
        {
            colEvents[i].gameObject.SetActive(true);
            colEvents[i].ObsReset();
        }

       
    }
}
