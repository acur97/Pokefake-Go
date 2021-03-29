using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Pokeball : MonoBehaviour
{
    private int accion = 0;
    private Rigidbody rb;
    private GameObject coll;

    public PokemonsManager PokeManager;
    public Transform puntoCentral;
    public float tiempoEsperaDecision = 3;
    public TextMeshProUGUI estatus;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        accion = Random.Range(0, 3);

        ///0 es atrapado
        ///1 no se atrapa
        ///2 se escapa
        
        Debug.Log(accion);

        coll = collision.gameObject;
        coll.SetActive(false);
        rb.isKinematic = true;
        transform.position = puntoCentral.position;
        transform.rotation = puntoCentral.rotation;
        estatus.text = "Capturando...";

        switch (accion)
        {
            case 0:
                StartCoroutine(BasicAnimation0());
                break;

            case 1:
                StartCoroutine(BasicAnimation1());
                break;

            case 2:
                StartCoroutine(BasicAnimation2());
                break;

            default:
                break;
        }
    }

    IEnumerator BasicAnimation0()
    {
        yield return new WaitForSecondsRealtime(tiempoEsperaDecision);
        estatus.text = "El Pokemon ha sido capturado!";
        PokemonsData._PokeData.IndexPokemon.Add(PokeManager.randomIndex);
        PokemonsData._PokeData.IndexLibres.Remove(PokeManager.randomIndex);
        Debug.Log("lista quedo con: " + PokemonsData._PokeData.IndexPokemon.Count);

        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(2);
        //escena pokedex
    }
    IEnumerator BasicAnimation1()
    {
        yield return new WaitForSecondsRealtime(tiempoEsperaDecision);
        estatus.text = "El Pokemon no fue capturado";

        rb.isKinematic = false;
        coll.SetActive(true);
        //intento de captura de nuevo
    }
    IEnumerator BasicAnimation2()
    {
        yield return new WaitForSecondsRealtime(tiempoEsperaDecision);
        estatus.text = "El Pokemon se escapó!";
        yield return new WaitForSecondsRealtime(1.5f);

        coll.SetActive(true);
        rb.isKinematic = false;
        //nuevo pokemon
        PokeManager.GetPokemon();
    }
}