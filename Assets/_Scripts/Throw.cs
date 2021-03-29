using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    public Text txt;
    public float velocidadLimit;
    //public Transform angle;
    public GameObject ball;
    public float distance_to_screen;
    public Vector3 pos_move;
    public Vector3 forceDirection;
    public float poderZ;
    private float velocidad;
    public float poder;
    public ForceMode fmode;
    private Vector3 lastPos;
    private Vector3 newPos;
    private bool tocando = false;
    private Vector3 transformDir;

    private Rigidbody rb;

    private void Awake()
    {
        rb = ball.GetComponent<Rigidbody>();

    }

    private void OnMouseDown()
    {
        tocando = true;
        rb.isKinematic = true;
        ball.transform.position = transform.position;
    }

    private void Update()
    {
        //angle.rotation = Camera.main.transform.rotation;
        if (tocando)
        {
            ball.transform.position = transform.position;
            lastPos = ball.transform.position;

            distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
            transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);

            newPos = transform.position;
            ball.transform.rotation = transform.rotation;
        }
    }

    public void t_back()
    {
        transformDir = Vector3.back;
    }
    public void t_down()
    {
        transformDir = Vector3.down;
    }
    public void t_forward()
    {
        transformDir = Vector3.forward;
    }
    public void t_left()
    {
        transformDir = Vector3.left;
    }
    public void t_one()
    {
        transformDir = Vector3.one;
    }
    public void t_rigth()
    {
        transformDir = Vector3.right;
    }
    public void t_up()
    {
        transformDir = Vector3.up;
    }
    public void t_zero()
    {
        transformDir = Vector3.zero;
    }

    private void OnMouseUp()
    {
        tocando = false;
        velocidad = Vector3.Distance(lastPos, newPos);
        //ball.transform.LookAt(newPos, ball.transform.up);
        velocidad *= (Time.deltaTime * 100);
        velocidad = Mathf.Clamp(velocidad, 0, velocidadLimit);
        forceDirection = new Vector3(-(lastPos.x - newPos.x), -(lastPos.y - newPos.y), -(lastPos.z - newPos.z));
        forceDirection.z = velocidad * poderZ;
        forceDirection.x = 0;
        forceDirection = (forceDirection * velocidad) * poder;
        //Debug.Log("angulo x: " + forceDirection.x + " y: " + forceDirection.y + " z: " + forceDirection.z);
        //forceDirection = transform.TransformVector(forceDirection);
        //Debug.Log("velocidad: " + velocidad);

        if (velocidad >= 0.06f)
        {
            rb.isKinematic = false;
            rb.AddRelativeForce(forceDirection, fmode);
        }
        txt.text = "vel: " + velocidad;
    }
}