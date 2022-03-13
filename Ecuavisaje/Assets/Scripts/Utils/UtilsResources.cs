using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsResources
{
    public static List<Character> getScriptableObjectsCharacters(){
        List<Character> characters = new List<Character>();
        string path_base = "Characters/";
        characters.Add(Resources.Load<Character>($"{path_base}CharacterConserje"));
        characters.Add(Resources.Load<Character>($"{path_base}CharacterLasso"));
        return characters;
    }
}
