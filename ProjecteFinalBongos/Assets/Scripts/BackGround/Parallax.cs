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

    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
        scrollLeft = Random.Range(0, 2) == 0 ? false : true;
        if (scrollLeft) speed = -speed;
    }

    private void SetupTexture()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Scroll()
    {
        float delta = speed * Time.deltaTime;
        transform.position += new Vector3(delta, 0, 0);
    }

    void CheckReset()
    {
        if((Math.Abs(transform.position.x) - singleTextureWidth) > 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }
    private void Update()
    {
        Scroll();
        CheckReset();
    }
}
