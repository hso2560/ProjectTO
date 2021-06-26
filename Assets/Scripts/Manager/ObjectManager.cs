using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public CollisionEventSc[] colEvents;
    public List<Enemy1> enemys = new List<Enemy1>();
    public bool playerLoad=false;
    //public SelfMoveObs[] selfObs;

    public void ObsReset()
    {
        int i;

        for(i=0; i<colEvents.Length; i++)
        { 
            if (colEvents[i].isWork)
            {
                colEvents[i].gameObject.SetActive(true);
                colEvents[i].ObsReset();        //오브젝트 꺼져있어도 변수, 함수 접근이 됨(start,update는 안됨). 근데 혹시 모르니 걍 이렇게

                colEvents[i].gameObject.SetActive(colEvents[i].firstActive);
            }
            //colEvents[i].gameObject.SetActive(false);
        }

        for(i=0; i<enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(true);
            enemys[i].ResetData();
        }
    }

    public void EnemysOff()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            enemys[i].gameObject.SetActive(true);
            enemys[i].ResetData();
        }
    }

    public IEnumerator ObjInitData()
    {
        while (!AllObjSetting()) yield return null;
    }

    private bool AllObjSetting()
    {
        if (!playerLoad) return false;

        for(int i=0; i<colEvents.Length; i++)
        {
            if (!colEvents[i].bInitSet) return false;
        }
        //return playerLoad;
        return true;
    }
}
