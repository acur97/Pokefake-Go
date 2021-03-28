using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throw : MonoBehaviour
{
    public Text txt;
    public float velocidadLimit;
    public Transform angle;
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
        if (tocando)
        {
            ball.transform.position = transform.position;
            lastPos = ball.transform.position;

            distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
            transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);

            newPos = transform.position;
        }
    }

    private void OnMouseUp()
    {
        tocando = false;
        velocidad = Vector3.Distance(lastPos, newPos);
        velocidad *= (Time.deltaTime * 100);
        velocidad = Mathf.Clamp(velocidad, 0, velocidadLimit);
        forceDirection = new Vector3(-(lastPos.x - newPos.x), -(lastPos.y - newPos.y), -(lastPos.z - newPos.z));
        forceDirection.z = velocidad * poderZ;
        angle.forward = forceDirection;
        Debug.Log("velocidad: " + velocidad);
        Debug.Log("angulo x: " + forceDirection.x + " y: " + forceDirection.y + " z: " + forceDirection.z);
        if (velocidad >= 0.06f)
        {
            rb.isKinematic = false;
            rb.AddForce(forceDirection * velocidad * poder, fmode);
        }
        txt.text = "vel: " + velocidad;
    }
}