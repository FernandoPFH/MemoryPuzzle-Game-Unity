using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManterMusicaTocando : MonoBehaviour
{
    void Awake() {
        // Acha Quantos Objetos Estão Tocando Música De Fundo
        GameObject[] objetosTocandoMusicaDeFundo = GameObject.FindGameObjectsWithTag("Musica");

        // Se Existir Mais De Um, Serão Excluidos
        if (objetosTocandoMusicaDeFundo.Length > 1) {
            Object.Destroy(this.gameObject);
        }

        // Não Deixa O Objeto Ser Destruido Mesmo Quando A Cena Reinicia
        DontDestroyOnLoad(this.gameObject);
    }
}