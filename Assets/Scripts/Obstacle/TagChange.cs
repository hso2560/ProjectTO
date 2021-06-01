using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChange : MonoBehaviour
{
    public int id;
    [SerializeField] string targetTag;

    private void OnCollisionEnter(Collision collision)
    {
        if(tag!=targetTag)
        {
            Invoke("InvokeFunc", 0.2f);
        }
    }

    void InvokeFunc()=>gameObject.tag = targetTag;
    
}
