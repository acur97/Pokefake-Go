using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;

public class RestApiPokeTest : MonoBehaviour
{
    public Button btn;
    public Image pokeIMG2;
    public RawImage pokeIMG;
    public TextMeshProUGUI pokeNameText;
    public TextMeshProUGUI pokeNumText;
    public TextMeshProUGUI pokeTypesNum;
    public TextMeshProUGUI[] pokeTypeText;

    private readonly string baseURL = "https://pokeapi.co/api/v2/";

    private void Start()
    {
        pokeIMG2.sprite = null;
        pokeIMG.texture = Texture2D.blackTexture;

        pokeNameText.text = "";
        pokeNumText.text = "";
        pokeTypesNum.text = "";

        for (int i = 0; i < pokeTypeText.Length; i++)
        {
            pokeTypeText[i].text = "";
        }
    }

    public void RandomPokeIndex()
    {
        int randomIndex = Random.Range(1, 899);
        //int randomIndex = 899;

        pokeIMG2.sprite = null;
        pokeIMG.texture = Texture2D.blackTexture;

        pokeNameText.text = "cargando";
        pokeNumText.text = randomIndex.ToString();

        for (int i = 0; i < pokeTypeText.Length; i++)
        {
            pokeTypeText[i].text = "";
        }

        StartCoroutine(GetPokeIndex(randomIndex));
    }

    IEnumerator GetPokeIndex(int pokeindex)
    {
        btn.interactable = false;
        ///get poke info

        string pokeURL = baseURL + "pokemon/" + pokeindex.ToString();

        UnityWebRequest pokeinforequest = UnityWebRequest.Get(pokeURL);

        yield return pokeinforequest.SendWebRequest();

        if (pokeinforequest.result == UnityWebRequest.Result.ConnectionError
            || pokeinforequest.result == UnityWebRequest.Result.DataProcessingError
            || pokeinforequest.result == UnityWebRequest.Result.ProtocolError)
        {
            pokeNameText.text = "F";
            btn.interactable = true;
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
        pokeIMG.texture = img2d;

        pokeIMG2.sprite = Sprite.Create(img2d, new Rect(0, 0, img2d.width, img2d.height), new Vector2(0.5f, 0.5f));

        pokeNameText.text = Mayuscula(pokeName);
        pokeTypesNum.text = poketypes.Count.ToString();

        for (int i = 0; i < poketypesNames.Length; i++)
        {
            pokeTypeText[i].text = Mayuscula(poketypesNames[i]);
        }

        btn.interactable = true;
    }

    private string Mayuscula(string txt)
    {
        return char.ToUpper(txt[0]) + txt.Substring(1);
    }
}