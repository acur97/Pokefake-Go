using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonsData : MonoBehaviour
{
    public static PokemonsData _PokeData;
    private object[] single;

    public List<int> IndexPokemon;
    public List<int> IndexLibres;

    private void Awake()
    {
        single = FindObjectsOfType(typeof(PokemonsData));
        int len = single.Length;
        if (len == 2)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        _PokeData = this;
        IndexPokemon = new List<int>();

        IndexLibres = new List<int>();
        for (int i = 0; i < 899; i++)
        {
            IndexLibres.Add(i);
        }
    }

}