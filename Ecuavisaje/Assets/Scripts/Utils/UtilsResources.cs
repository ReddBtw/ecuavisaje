using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsResources
{
    public static List<Character> getScriptableObjectsCharacters(){
        List<Character> characters = new List<Character>();
        string path_base = "Characters/";

        CharacterEnum[] values = (CharacterEnum[])CharacterEnum.GetValues(typeof(CharacterEnum));

        foreach (CharacterEnum item in values)
        {
            if(item == CharacterEnum.None){
                continue;
            }
            string fileScriptableObject = $"{path_base}Character{item}";
            // Debug.Log(fileScriptableObject);
            characters.Add(Resources.Load<Character>(fileScriptableObject));
        }
        return characters;
    }
}
