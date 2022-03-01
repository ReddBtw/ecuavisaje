using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    Vector3 target_Rot;
    Vector3 current_angle;
    //Jugador màs cercano
    GameObject closest = null;

    bool giroCompleto = false;
    Vector3 angulo_anterior;

    /* Personajes disponibles */
    int totalCharacters = 6;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* Busqueda de personaje */
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            giroCompleto = false;
            //Actualizo el ángulo de rotación actual
            current_angle = transform.eulerAngles;
            //Actualizo el ángulo de giro
            target_Rot += new Vector3(0, 360 / totalCharacters, 0);
            Debug.Log("---------------Tecla: Izquierda---------------");
            Debug.Log("target_Rot: " + target_Rot);
            Debug.Log("current_angle: " + current_angle);
            Debug.Log("giros completos: " + giroCompleto);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            giroCompleto = false;
            //Actualizo el ángulo de rotación actual
            current_angle = transform.eulerAngles;
            //Actualizo el ángulo de giro
            target_Rot -= new Vector3(0, 360 / totalCharacters, 0);
            Debug.Log("---------------Tecla: Derecha---------------");
            Debug.Log("target_Rot: " + target_Rot);
            Debug.Log("current_angle: " + current_angle);
            Debug.Log("giros completos: " + giroCompleto);
        }
        /* Actulizo el ángulo de rotación */
        current_angle = new Vector3(0, Mathf.LerpAngle(current_angle.y, target_Rot.y, Time.deltaTime * 2), 0);
        transform.eulerAngles = current_angle;
        //Marca al personaje más cercano a la cámara
        //TODO: Que el código se ejecute una sola vez por giro
        if (!giroCompleto)
        {
            GetClosestObject();
        }
        //Selecciona personaje
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("---------------Tecla: Espacio---------------");
            Debug.Log("closest: " + closest);
            if (closest != null)
            {
                closest.tag = "Player";
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Debug.Log("Seleccionando personaje: " + player.name);
                DontDestroyOnLoad(player);
                SceneManager.LoadScene("DevFun");
            }
        }
    }

    /* Objeto con distancia más corta a la cámara */
    public GameObject GetClosestObject()
    {
        //A) Comprobar si existio un cambio con el personaje
        GameObject goAuxiliar = closest;
        //Jugadores en el plano
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Personaje");
        float distance = Mathf.Infinity;
        /* Poscion de la cámara */
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 position = camera.transform.position;
        foreach (GameObject go in gos)
        {
            float distanciaAux = Vector3.Distance(position, go.transform.position);
            if (distanciaAux < distance)
            {
                distance = distanciaAux;
                closest = go;
            }
        }
        if (goAuxiliar != closest)
        {
            giroCompleto = true;
        }
        Debug.Log("Objeto mas cercano: " + closest.name);
        return closest;
    }

    /* Seleccionar personaje */
    int escena = 0;
    public void CambioDeEscena()
    {
        if (escena == 1)
        {
            SceneManager.LoadScene(1);
            escena--;
        }
        else
        {
            SceneManager.LoadScene(0);
            escena++;
        }
    }
}
