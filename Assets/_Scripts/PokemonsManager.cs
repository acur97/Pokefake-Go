using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using easyar;

public class PokemonsManager : MonoBehaviour
{
    private readonly string baseURL = "https://pokeapi.co/api/v2/";
    public List<int> validChoices;

    public SurfaceTrackerFrameFilter sTracker;
    public int randomIndex = 0;
    public TextMeshProUGUI estatus;
    public TextMeshProUGUI pokeNameText;
    public TextMeshProUGUI pokeNumText;
    public TextMeshProUGUI[] pokeTypeText;
    public UnityEngine.UI.Image pokeIMG2;

    private void Start()
    {
        GetPokemon();
    }

    public void ResetTarget()
    {
        StartCoroutine(ResetTargetS());
    }

    IEnumerator ResetTargetS()
    {
        sTracker.enabled = false;
        yield return new WaitForSecondsRealtime(0.25f);
        sTracker.enabled = true;
    }

    public void GetPokemon()
    {
        pokeIMG2.sprite = null;
        validChoices = PokemonsData._PokeData.IndexLibres;
        randomIndex = Random.Range(1, PokemonsData._PokeData.IndexLibres.Count);
        pokeNumText.text = randomIndex.ToString();

        estatus.text = "Cargando...";
        StartCoroutine(GetPokeIndex(randomIndex));
    }

    IEnumerator GetPokeIndex(int pokeindex)
    {
        ///get poke info

        string pokeURL = baseURL + "pokemon/" + pokeindex.ToString();

        UnityWebRequest pokeinforequest = UnityWebRequest.Get(pokeURL);

        yield return pokeinforequest.SendWebRequest();

        if (pokeinforequest.result == UnityWebRequest.Result.ConnectionError
            || pokeinforequest.result == UnityWebRequest.Result.DataProcessingError
            || pokeinforequest.result == UnityWebRequest.Result.ProtocolError)
        {
            estatus.text = "F";
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

        pokeIMG2.sprite = Sprite.Create(img2d, new Rect(0, 0, img2d.width, img2d.height), new Vector2(0.5f, 0.5f));

        pokeNameText.text = Mayuscula(pokeName);

        for (int i = 0; i < poketypesNames.Length; i++)
        {
            pokeTypeText[i].text = Mayuscula(poketypesNames[i]);
        }

        estatus.text = "";
    }

    private string Mayuscula(string txt)
    {
        return char.ToUpper(txt[0]) + txt.Substring(1);
    }
}