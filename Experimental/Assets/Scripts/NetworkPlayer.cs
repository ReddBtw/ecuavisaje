using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] TMPro.TMP_Text displayBarName;

    // sync server value to client
    [SyncVar(hook = nameof(handleUpdateDisplayName))]
    [SerializeField]
    private string display_name = "Missing name";

    [Server]
    public void setDisplayName(string new_display_name){
        display_name = new_display_name;
        this.displayBarName.text = new_display_name;
    }


    private void handleUpdateDisplayName(string oldName, string newName)
    {
        display_name = newName;
        this.displayBarName.text = newName;
    }



}
