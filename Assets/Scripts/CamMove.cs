using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public Transform target;
    public Transform rotTarget;
    public GlitchEffect camGlich;
    public PlayerScript player;
    public MainManager mainManager;

    public float xSpeed = 220f, ySpeed = 100f;
    public float x, y;
    public float yMinLimit = -20f, yMaxLimit = 80f;

    public Vector3 rOffset, offset;
    private Vector3 position;
    private Quaternion rotation;

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;

        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        if (target != null)
            if(!player.isDie && !player.bCompulsoryIdle && !mainManager.IsGoal)
                Move();
    }

    private void Move()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.015f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.015f;
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * rOffset + target.position + offset;

        transform.position = position;
        transform.rotation = rotation;
        rotTarget.rotation = Quaternion.Euler(0, x, 0);
    }
}
