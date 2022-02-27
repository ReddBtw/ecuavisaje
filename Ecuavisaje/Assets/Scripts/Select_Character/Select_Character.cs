using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_Character : MonoBehaviour
{
    
    Vector3 target_Rot;
    Vector3 current_angle;
    /* Personaje seleccionado */
    int currentSelection;

    /* Personajes disponibles */
    int totalCharacters=6;

    // Start is called before the first frame update
    void Start()
    {
        /* Personaje Seleccionado por defecto */
        currentSelection=1;        
    }

    // Update is called once per frame
    void Update()
    {
        /* Comprobar si se presiono una tecla */
        if(Input.GetKeyDown(KeyCode.LeftArrow)&&currentSelection<totalCharacters){
            //Actualizo el ángulo de rotación actual
            current_angle=transform.eulerAngles;
            //Actualizo el ángulo de giro
            target_Rot+=new Vector3(0,360/totalCharacters,0);
            currentSelection++;
            Debug.Log("Seleccion actual " + currentSelection);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)&&currentSelection>1){
            //Actualizo el ángulo de rotación actual
            current_angle=transform.eulerAngles;
            //Actualizo el ángulo de giro
            target_Rot-=new Vector3(0,360/totalCharacters,0);
            currentSelection--;
            Debug.Log("Seleccion actual " + currentSelection);
        }
        
        /* Actulizo el ángulo de rotación */
        current_angle=new Vector3(0,Mathf.LerpAngle(current_angle.y,target_Rot.y,Time.deltaTime*2),0);
        transform.eulerAngles=current_angle;
    }
}
