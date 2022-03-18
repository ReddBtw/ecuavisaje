using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitSelection : MonoBehaviour
{
    public int rutina;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rutina = Random.Range(0, 5);
        switch (rutina)
        {
            /* Seleccionar animación de espera al azar */
            case 0:
                anim.SetInteger("wait", 0);
                break;
            case 1:
                anim.SetInteger("wait", 1);
                break;
            /* Enemigo se mueve hacia el punto indicado */
            case 2:
                anim.SetInteger("wait", 2);
                break;
            case 3:
                anim.SetInteger("wait", 3);
                break;
            case 4:
                anim.SetInteger("wait", 4);
                break;
        }
    }
}
