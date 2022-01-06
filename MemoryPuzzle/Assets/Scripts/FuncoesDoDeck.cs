using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncoesDoDeck : MonoBehaviour
{
    // Variáveis Acessiveis De Fora Da Classe
    public int numeroDeCartas = 10;
    public float offSetEntreCartas = 0.1f;
    public GameObject prefabCarta;

    // Start is called before the first frame update
    void Start()
    {
        this.criarTorreDeCartas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Função Para Criar Uma Torre De Cartas
    private void criarTorreDeCartas() {
        // Pega A Posição Da Base Da Torre De Cartas
        var posicaoDaCarta = this.transform.position;

        GameObject cartaIniciada;

        for (int indice = 0; indice < numeroDeCartas; indice++)
        {
            // Instancia O Prefab Da Carta
            cartaIniciada = Instantiate(prefabCarta,posicaoDaCarta,Quaternion.identity) as GameObject;

            // Adiciona A Carta Ao Deck
            cartaIniciada.transform.parent = this.transform;

            // Pega A Posição Da Carta Criada
            posicaoDaCarta = cartaIniciada.transform.Find("Carta").Find("ParteDeTras").position;

            // Adiciona Um Espaçamento Entre Cartas
            posicaoDaCarta.y += offSetEntreCartas;
        }
    }
}