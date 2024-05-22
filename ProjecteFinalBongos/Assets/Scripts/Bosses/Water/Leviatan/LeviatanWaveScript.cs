using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviatanWaveScript : MonoBehaviour
{
    [SerializeField] EstadoEvent changeEstado;
    [SerializeField] private float scaleSpeed = 3f;
    [SerializeField] private float fadeOutSpeed = 0.2f;
    private SpriteRenderer spriteRenderer;
    private Color fadeOut;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        changeEstado.Raise(EstadosAlterados.Mullat);
    }

    private void Update()
    {
        transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
        fadeOut = spriteRenderer.material.color;
        spriteRenderer.material.color = new Color(fadeOut.r, fadeOut.g, fadeOut.b, (fadeOut.a - (fadeOutSpeed * Time.deltaTime)));
        if (fadeOut.a <= 0)
        {
            Destroy(gameObject);

        }
    }
}