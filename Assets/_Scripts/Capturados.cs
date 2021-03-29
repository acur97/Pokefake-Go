using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Capturados : MonoBehaviour
{
    public GameObject btn_prefab;
    public Transform padre;
    private GameObject obj;
    private ButtonInfoPokemon btn_info;

    private void Awake()
    {
        for (int i = 0; i < PokemonsData._PokeData.IndexPokemon.Count; i++)
        {
            obj = Instantiate(btn_prefab, padre);
            btn_info = obj.GetComponent<ButtonInfoPokemon>();
            btn_info.index = PokemonsData._PokeData.IndexPokemon[i];
            btn_info.Preparar();
        }
    }

    public void VolverScena()
    {
        SceneManager.LoadScene(1);
    }
}