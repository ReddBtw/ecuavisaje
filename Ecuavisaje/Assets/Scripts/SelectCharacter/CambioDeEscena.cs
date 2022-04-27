using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public GameObject personaje;
    public Transform puntoInicial;
    private void Start()
    {
        personaje = GameObject.FindGameObjectWithTag("Player");
        puntoInicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        MoverAlPuntoInicial();
    }

    private void MoverAlPuntoInicial()
    {
        personaje.transform.position = puntoInicial.position;
        personaje.transform.eulerAngles = puntoInicial.eulerAngles;
    }

    private void Update()
    {
        TestCambioDeEscena();
    }

    public void TestCambioDeEscena()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "SelectCharacter")
            {
                SceneManager.LoadScene("PrimeraEscena");
            }
            else if (SceneManager.GetActiveScene().name == "PrimeraEscena")
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Destroy(player);
                SceneManager.LoadScene("SelectCharacter");
            }
        }
    }
}

