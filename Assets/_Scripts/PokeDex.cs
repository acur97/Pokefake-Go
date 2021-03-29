using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using SimpleJSON;

public class PokeDex : MonoBehaviour
{
    public GameObject btn_prefab;
    public Transform padre;
    private GameObject obj;
    private ButtonInfoPokemon btn_info;
    public int indexPaginas = 1;
    private ButtonInfoPokemon[] btnBasuras;
    public TextMeshProUGUI estatus;

    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void OnEnable()
    {
        estatus.text = "";
        for (int i = 0; i < 9; i++)
        {
            obj = Instantiate(btn_prefab, padre);
            btn_info = obj.GetComponent<ButtonInfoPokemon>();
            btn_info.index = indexPaginas + i;
            btn_info.Preparar();
        }
    }

    public void Siguiente()
    {
        estatus.text = "";
        if (indexPaginas < 890)
        {
            indexPaginas += 9;
            btnBasuras = padre.GetComponentsInChildren<ButtonInfoPokemon>();
            for (int i = 0; i < btnBasuras.Length; i++)
            {
                Destroy(btnBasuras[i].gameObject);
            }

            for (int i = 0; i < 9; i++)
            {
                obj = Instantiate(btn_prefab, padre);
                btn_info = obj.GetComponent<ButtonInfoPokemon>();
                btn_info.index = indexPaginas + i;
                btn_info.Preparar();
            }
        }
    }

    public void Atras()
    {
        estatus.text = "";
        if (indexPaginas > 1)
        {
            indexPaginas -= 9;
            btnBasuras = padre.GetComponentsInChildren<ButtonInfoPokemon>();
            for (int i = 0; i < btnBasuras.Length; i++)
            {
                Destroy(btnBasuras[i].gameObject);
            }

            for (int i = 0; i < 9; i++)
            {
                obj = Instantiate(btn_prefab, padre);
                btn_info = obj.GetComponent<ButtonInfoPokemon>();
                btn_info.index = indexPaginas + i;
                btn_info.Preparar();
            }
        }
    }

    public void Buscar(string txt)
    {
        if (txt != "")
        {
            StartCoroutine(BuscarPokemon(txt));
        }
        else
        {
            btnBasuras = padre.GetComponentsInChildren<ButtonInfoPokemon>();
            for (int i = 0; i < btnBasuras.Length; i++)
            {
                Destroy(btnBasuras[i].gameObject);
            }

            for (int i = 0; i < 9; i++)
            {
                obj = Instantiate(btn_prefab, padre);
                btn_info = obj.GetComponent<ButtonInfoPokemon>();
                btn_info.index = indexPaginas + i;
                btn_info.Preparar();
            }
        }
    }

    IEnumerator BuscarPokemon(string nombre)
    {
        string pokeURL = baseURL + "pokemon/" + nombre;

        UnityWebRequest pokeinforequest = UnityWebRequest.Get(pokeURL);

        yield return pokeinforequest.SendWebRequest();

        if (pokeinforequest.result == UnityWebRequest.Result.ConnectionError
            || pokeinforequest.result == UnityWebRequest.Result.DataProcessingError
            || pokeinforequest.result == UnityWebRequest.Result.ProtocolError)
        {
            btnBasuras = padre.GetComponentsInChildren<ButtonInfoPokemon>();
            for (int i = 0; i < btnBasuras.Length; i++)
            {
                Destroy(btnBasuras[i].gameObject);
            }
            estatus.text = "No hay resultados de la busqueda";
            Debug.Log(pokeinforequest.error);
            yield break;
        }

        ///info gettet

        JSONNode pokeinfo = JSON.Parse(pokeinforequest.downloadHandler.text);

        string pokeID = pokeinfo["id"];

        estatus.text = "";
        btnBasuras = padre.GetComponentsInChildren<ButtonInfoPokemon>();
        for (int i = 0; i < btnBasuras.Length; i++)
        {
            Destroy(btnBasuras[i].gameObject);
        }

        obj = Instantiate(btn_prefab, padre);
        btn_info = obj.GetComponent<ButtonInfoPokemon>();
        btn_info.index = int.Parse(pokeID);
        btn_info.Preparar();
    }
}