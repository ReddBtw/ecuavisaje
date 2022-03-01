using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CambioDeEscena : MonoBehaviour
{
    public GameObject personaje;
    public Transform puntoInicial = null;
    private void Start()
    {
        /* Se ubica al personaje en un punto especificado en la nueva escena */
        personaje = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Personaje: " + personaje);
        puntoInicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        Debug.Log("PuntoInicial: " + puntoInicial);
        personaje.transform.position = puntoInicial.position;
    }

    private void update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SelectCharacter");
        }
    }
}

