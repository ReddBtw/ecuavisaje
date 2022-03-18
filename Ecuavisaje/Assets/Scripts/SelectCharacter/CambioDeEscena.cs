using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public GameObject personaje;
    public Transform puntoInicial;
    public Camera camera = new Camera();
    private void Start()
    {
        personaje = GameObject.FindGameObjectWithTag("Player");
        puntoInicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        AddCameraPlayer();
        MoverAlPuntoInicial();
        EditarComponentes();
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
        if (Input.GetKeyDown(KeyCode.P))
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

    public void AddCameraPlayer()
    {
        GameObject newCamera = GameObject.FindGameObjectWithTag("MainCamera");
        newCamera.transform.parent = personaje.transform;
        newCamera.transform.localPosition = new Vector3(0, 2, -5);
        newCamera.transform.localEulerAngles = new Vector3(10, 0, 0);
    }

    public void EditarComponentes()
    {
        //Se carga desplazamiento
        personaje.AddComponent(typeof(SurfaceDisplacement));
        //Cargar animaciones
        /* Destroy(personaje.GetComponent<Animator>()); */
    }
}

