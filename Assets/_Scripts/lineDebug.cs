using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineDebug : MonoBehaviour
{
    private LineRenderer line;

    public Transform punto1;
    public Transform punto2;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.SetPosition(0, punto1.position);
        line.SetPosition(1, punto2.position);
    }
}