﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node[] ConnectsTo;
    public Node[] ConnectsToProd;

    private void OnDrawGizmos()
    {
        foreach (Node n in ConnectsTo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (n.transform.position - transform.position).normalized * 2);
        }
        foreach (Node n in ConnectsToProd)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, (n.transform.position - transform.position).normalized * 2);
        }
    }
}
