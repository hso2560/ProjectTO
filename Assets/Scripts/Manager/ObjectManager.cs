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
            colEvents[i].gameObject.SetActive(true);

            if (colEvents[i].isWork)
                colEvents[i].ObsReset();        //������Ʈ �����־ ����, �Լ� ������ ��(start,update�� �ȵ�). �ٵ� Ȥ�� �𸣴� �� �̷���

            colEvents[i].gameObject.SetActive(false);
        }

        for(i=0; i<enemys.Count; i++)
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
