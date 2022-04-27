using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Se da un identificador a cada objeto dentro de la escena */
public class IdentificadorPersonaje : MonoBehaviour
{
    public string nombre;
    void Awake()
    {
        transform.gameObject.tag = "Personaje";
        nombre = gameObject.name;
    }
    void Start()
    {

    }
    void Update()
    {
    }
}
