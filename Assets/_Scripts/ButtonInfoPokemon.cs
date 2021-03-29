using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class ButtonInfoPokemon : MonoBehaviour
{
    public int index;
    public Image img;

    /*public TextMeshProUGUI pokeNameText;
    public TextMeshProUGUI pokeNumText;
    public TextMeshProUGUI[] pokeTypeText;*/
    private Button btn;

    private string nombre;
    private string numero;
    private string[] tipo = new string[2];
    private Sprite imagen;
    private string peso;
    private string altura;
    private string experiencia;

    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.interactable = false;
        img.sprite = null;
    }

    public void Preparar()
    {
        StartCoroutine(GetPokeIndex());
    }

    public void clic()
    {
        //Debug.Log(index);
        //pantalla info
        PokemonInfo._pokeInfo.AbrirInfo(nombre, numero, tipo, imagen, peso, altura, experiencia);
    }

    IEnumerator GetPokeIndex()
    {
        ///get poke info

        string pokeURL = baseURL + "pokemon/" + index.ToString();

        UnityWebRequest pokeinforequest = UnityWebRequest.Get(pokeURL);

        yield return pokeinforequest.SendWebRequest();

        if (pokeinforequest.result == UnityWebRequest.Result.ConnectionError
            || pokeinforequest.result == UnityWebRequest.Result.DataProcessingError
            || pokeinforequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(pokeinforequest.error);
            yield break;
        }

        ///info gettet

        JSONNode pokeinfo = JSON.Parse(pokeinforequest.downloadHandler.text);

        string pokeName = pokeinfo["name"];
        string pokeIMGurl = pokeinfo["sprites"]["front_default"];

        JSONNode poketypes = pokeinfo["types"];
        string[] poketypesNames = new string[poketypes.Count];

        for (int i = 0, j = poketypes.Count - 1; i < poketypes.Count; i++, j--)
        {
            poketypesNames[j] = poketypes[i]["type"]["name"];
        }

        ///get poke img

        UnityWebRequest pokeIMGrequest = UnityWebRequestTexture.GetTexture(pokeIMGurl);

        yield return pokeIMGrequest.SendWebRequest();

        if (pokeIMGrequest.result == UnityWebRequest.Result.ConnectionError
            || pokeIMGrequest.result == UnityWebRequest.Result.DataProcessingError
            || pokeIMGrequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(pokeIMGrequest.error);
            yield break;
        }

        //set infos

        Texture2D img2d = DownloadHandlerTexture.GetContent(pokeIMGrequest);
        img2d.filterMode = FilterMode.Point;
        //pokeIMG.texture = img2d;

        img.sprite = Sprite.Create(img2d, new Rect(0, 0, img2d.width, img2d.height), new Vector2(0.5f, 0.5f));

        /*if (pokeNameText != null)
        {
            pokeNameText.text = Mayuscula(pokeName);
        }

        if (pokeTypeText[0] != null)
        {
            for (int i = 0; i < poketypesNames.Length; i++)
            {
                pokeTypeText[i].text = Mayuscula(poketypesNames[i]);
            }
        }*/

        nombre = Mayuscula(pokeName);
        numero = index.ToString();
        for (int i = 0; i < poketypesNames.Length; i++)
        {
            tipo[i] = Mayuscula(poketypesNames[i]);
        }
        //tipo[0] = Mayuscula(poketypesNames[0]);
        //tipo[1] = Mayuscula(poketypesNames[1]);
        imagen = img.sprite;
        peso = pokeinfo["weight"];
        altura = pokeinfo["height"] + "´";
        experiencia = pokeinfo["base_experience"];
        btn.interactable = true;
    }

    private string Mayuscula(string txt)
    {
        return char.ToUpper(txt[0]) + txt.Substring(1);
    }
}