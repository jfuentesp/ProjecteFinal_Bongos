using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DañoAAA : MonoBehaviour
{
    [SerializeField]
    private DañoEnemigoEvent daño;

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = -Vector3.right * 2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        daño.Raise(1f);
    }
}
