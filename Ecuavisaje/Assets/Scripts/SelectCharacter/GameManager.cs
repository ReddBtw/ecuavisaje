using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] personajes;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Iniciando el juego");
        Debug.Log(personajes.Length);
        foreach (GameObject personaje in personajes)
        {
            personaje.gameObject.SetActive(true);
            Debug.Log("Personaje Activado: " + personaje.name);
            Debug.Log("Personaje Tag: " + personaje.tag);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
