using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Parallax : MonoBehaviour
{
    private float singleTextureWidth;
    bool scrollLeft;
    [SerializeField] private float speed;
    private Camera m_Camara;

    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
        scrollLeft = Random.Range(0, 2) == 0 ? false : true;
        if (scrollLeft) speed = -speed;
    }

    private void SetupTexture()
    {
        m_Camara = Camera.main;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Scroll()
    {
        float delta = speed * Time.deltaTime;
        transform.localPosition += new Vector3(delta, 0, 0);
    }

    void CheckReset()
    {
        if((Math.Abs(transform.localPosition.x) - singleTextureWidth) > 0)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
    }
    private void Update()
    {
        Scroll();
        CheckReset();
    }
}
