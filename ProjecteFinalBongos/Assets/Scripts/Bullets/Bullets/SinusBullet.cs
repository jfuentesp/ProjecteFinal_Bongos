using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusBullet : Bullet
{
    private float rotation;
    private bool sentidoHorario;

    public override void Init(Vector2 direction)
    {
        base.Init(direction);
        rotation = 0; 
        switch (Random.Range(0, 2))
        {
            case 0:
                sentidoHorario = true; break;
            case 1:
                sentidoHorario = false; break;
        }
       
    }
    private void FixedUpdate()
    {
        if (sentidoHorario)
        {

            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + 15f);
            rotation += 15f;
            if (rotation >= 90)
            {
                sentidoHorario = false;
            }
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - 15f);
            rotation -= 15f;
            if (rotation <= -90)
            {
                sentidoHorario = true;
            }
        }
        m_Rigidbody.velocity = transform.up * m_Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled)
            return;
        if(collision.gameObject.CompareTag("MechanicObstacle"))
            DisableBullet();
        if(collision.gameObject.CompareTag("Player"))
            DisableBullet();
    }

}