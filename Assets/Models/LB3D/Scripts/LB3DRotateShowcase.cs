using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LB3DRotateShowcase : MonoBehaviour
{

    public Slider sliderRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sliderRotate.value > 0.1f || sliderRotate.value < -0.1f)
            transform.Rotate(Vector3.up * sliderRotate.value);
    }
}
