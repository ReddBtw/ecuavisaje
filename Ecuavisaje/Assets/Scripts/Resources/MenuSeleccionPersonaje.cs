using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuSeleccionPersonaje : MonoBehaviour
{
    /*
    private int index;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nombre;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        index = PlayerPrefs.GetInt("PersonajeIndex", 0);
        if (index > gameManager.personajes.Count - 1)
        {
            index = 0;
        }
    }

    private void CambiarPantalla()
    {
        PlayerPrefs.SetInt("PersonajeIndex", index);
        image.sprite = gameManager.personajes[index].personajeImagen;
        nombre.text = gameManager.personajes[index].personajeNombre;
    }

    public void SiguientePersoanjes()
    {
        if (index == gameManager.personajes.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        CambiarPantalla();
    }

    public void AnteriorPersoanjes()
    {
        if (index == 0)
        {
            index = gameManager.personajes.Count - 1;
        }
        else
        {
            index--;
        }

        CambiarPantalla();
    }

    public void Jugar()
    {
        SceneManager.LoadScene("DevFun");
    }
    */
}
