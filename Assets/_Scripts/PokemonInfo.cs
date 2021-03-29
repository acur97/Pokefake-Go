using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonInfo : MonoBehaviour
{
    public static PokemonInfo _pokeInfo;

    public TextMeshProUGUI nombre;
    public TextMeshProUGUI numero;
    public TextMeshProUGUI[] tipo = new TextMeshProUGUI[2];
    public Image imagen;
    public TextMeshProUGUI peso;
    public TextMeshProUGUI altura;
    public TextMeshProUGUI experiencia;

    [Space]
    public GameObject Panel;

    private void Awake()
    {
        _pokeInfo = this;
    }

    private void Start()
    {
        Panel.SetActive(false);
    }

    public void AbrirInfo(string _nombre,
        string _numero,
        string[] _tipo,
        Sprite _imagen,
        string _peso,
        string _altura,
        string _experiencia)
    {
        Panel.SetActive(true);

        nombre.text = _nombre;
        numero.text = _numero;
        tipo[0].text = _tipo[0];
        tipo[1].text = _tipo[1];
        imagen.sprite = _imagen;
        peso.text = _peso;
        altura.text = _altura;
        experiencia.text = _experiencia;
    }

    public void CerrarInfo()
    {
        Panel.SetActive(false);
    }
}