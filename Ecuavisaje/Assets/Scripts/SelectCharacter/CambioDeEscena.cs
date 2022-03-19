using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public GameObject personaje, enemy;
    public Transform puntoInicial;
    public Vector3 cameraStart;
    private void Start()
    {
        personaje = GameObject.FindGameObjectWithTag("Player");
        enemy.gameObject.SetActive(true);
        puntoInicial = GameObject.FindGameObjectWithTag("PuntoInicial").transform;
        cameraStart = GameObject.FindGameObjectWithTag("CameraStart").transform.position;
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
        if (Input.GetKeyDown(KeyCode.Return))
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
    /*  private void AddEnemy()
     {
         GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
         enemy.gameObject.SetActive(true);
     } */

    public void AddCameraPlayer()
    {
        //Agregar cámara al personaje
        GameObject newCamera = GameObject.FindGameObjectWithTag("MainCamera");
        newCamera.transform.parent = personaje.transform;
        //Escala del personaje
        Vector3 escala = personaje.transform.lossyScale;
        newCamera.transform.localPosition = new Vector3(0 / escala.x, 7 / escala.y, -10 / escala.z);
        newCamera.transform.localEulerAngles = new Vector3(7, 0, 0);
        //Agregar luz
        GameObject lightPlayer = GameObject.Find("Directional Light");
        lightPlayer.transform.parent = newCamera.transform;
        lightPlayer.transform.localPosition = new Vector3(0 / escala.x, 5 / escala.y, -3 / escala.z);
        lightPlayer.transform.localEulerAngles = new Vector3(20, 0, 0);
    }

    public void EditarComponentes()
    {
        //Se carga desplazamiento
        personaje.AddComponent(typeof(SurfaceDisplacement));
        //Cargar animaciones
        Animator animator = personaje.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("SurfaceDesplacement") as RuntimeAnimatorController;
        //Encontrar los pies para activar eventos de salto
        foreach (GameObject foot in GameObject.FindGameObjectsWithTag("Foot"))
        {
            foot.AddComponent(typeof(LogicFeet));
        }
        Destroy(personaje.GetComponent<WaitSelection>());
    }
}

