using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncoesDoDeck : MonoBehaviour
{
    // Variáveis Acessiveis De Fora Da Classe
    public int numeroDeCartas = 40;
    public float offSetEntreCartas = 0.001f;
    public float velocidadeDeDistribuicaoDeCartas = 0.8f;
    public float tempoEntreDistribuicaoDeCartas = 0.15f;
    public GameObject prefabCarta;

    // Variáveis Acessiveis De Dentro Da Classe
    private List<Vector3> posicoesDasCartas = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Chama A Função Para Criar A Torre De Cartas
        this.criarTorreDeCartas();

        // Inicia A Lista De Posições Das Cartas
        posicoesDasCartas = new List<Vector3>();

        // Pega Todas As Posições Para As Cartas Irem
        foreach (Transform children in this.transform.Find("Possiveis Posicoes Das Cartas"))
        {
            posicoesDasCartas.Add(children.localPosition);
        }

        // Começar A Distribuir As Cartas Na Mesa
        StartCoroutine(this.moverTodasAsCartas());
    }

    // Distribuir As Cartas Na Mesa
    private IEnumerator moverTodasAsCartas() {
        // Inicia A Lista De Cartas De Cima Para Baixo Em Relação A Torre De Cartas
        List<Transform> cartasCimaParaBaixo = new List<Transform>();

        // Pega Todas As Cartas Da Torre De Cartas
        foreach (Transform item in this.transform.Find("Cartas"))
        {
            cartasCimaParaBaixo.Add(item);
        }

        // Reverse A Lista
        cartasCimaParaBaixo.Reverse();

        // Começa A Distribuir Cada Carta
        for (int indice = 0; indice < this.numeroDeCartas; indice++)
        {
            IEnumerator coroutine = moverCarta(cartasCimaParaBaixo[indice], posicoesDasCartas[indice]);
            StartCoroutine(coroutine);

            yield return new WaitForSeconds(this.tempoEntreDistribuicaoDeCartas);
        }
    }

    // Distribui Cada Carta Da Torre
    private IEnumerator moverCarta(Transform carta, Vector3 posicaoFinal) {
        // Pega A Posição Da Carta
        var posicaoInicial = carta.localPosition;

        // Calcula O Ponto De Controle
        var pontoDeControle = new Vector3(
            posicaoInicial.x + posicaoFinal.x,
            posicaoInicial.y + 0.4f,
            posicaoInicial.z + posicaoFinal.z
        );

        // Inicia A Posição Da Carta
        Vector3 posicaoCarta = Vector3.zero;

        // Inicia O Parametro De Interpolação
        float tParametro = 0f;

        while(tParametro < 1) {
            // Incrementa O Parametro De Interpolação
            tParametro += Time.deltaTime * this.velocidadeDeDistribuicaoDeCartas;

            // Calcula A Posição Atual Da Curva
            posicaoCarta = Mathf.Pow((1-tParametro),2) * posicaoInicial + 
                            2 * tParametro * (1 - tParametro) * pontoDeControle +
                            Mathf.Pow(tParametro,2) * posicaoFinal;

            // Seta A Posição Da Carta
            carta.localPosition = posicaoCarta;

            // Espera O Próximo Frame Para Continuar
            yield return new WaitForEndOfFrame();
        }

        // Ajusta A Posição Final, Se Houver Algum Erro        
        if (posicaoCarta != posicaoFinal) {
            carta.localPosition = posicaoFinal;
        }
    }

    // Desenha Um Cubo Vazado Da Posição Do Deck
    void OnDrawGizmos()
    {
        var tamanhoDoCubo = prefabCarta.GetComponentInChildren<BoxCollider>().size;
        var posicaoDoCentroDoCubo = this.transform.position;
        posicaoDoCentroDoCubo.y += tamanhoDoCubo.y / 2;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(posicaoDoCentroDoCubo, tamanhoDoCubo);
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
            cartaIniciada.transform.parent = this.transform.Find("Cartas");

            // Pega A Posição Da Carta Criada
            posicaoDaCarta = cartaIniciada.transform.Find("Carta").Find("ParteDeTras").position;

            // Adiciona Um Espaçamento Entre Cartas
            posicaoDaCarta.y += offSetEntreCartas;
        }
    }
}