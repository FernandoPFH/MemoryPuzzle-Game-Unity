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
    public List<Carta> possiveisCartas;

    // Variáveis Acessiveis De Dentro Da Classe
    private List<Transform> cartasDoDeck = new List<Transform>();
    private List<Vector3> posicoesDasCartasNoDeck = new List<Vector3>();
    private List<Vector3> novasPosicoesDasCartasNoDeck = new List<Vector3>();
    private List<Vector3> posicoesDasCartas = new List<Vector3>();
    private Vector3 posicaoFinalDasCartasDeVoltaParaODeck = Vector3.zero; 

    // Start is called before the first frame update
    void Start()
    {
        // Chama A Função Para Criar A Torre De Cartas
        this.criarTorreDeCartas();

        this.novasPosicoesDasCartasNoDeck = this.embaralharListaDePosicoesDasCartasNoDeck(this.posicoesDasCartasNoDeck);

        StartCoroutine(this.animacaoDeEmbaralharCartas());
    }

    // Distribuir As Cartas Na Mesa
    private IEnumerator moverTodasAsCartas() {
        // Inicia A Lista De Cartas De Cima Para Baixo Em Relação A Torre De Cartas
        List<Transform> cartasCimaParaBaixo = new List<Transform>();

        // Pega Todas As Cartas Da Torre De Cartas
        foreach (Transform item in this.cartasDoDeck)
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
            posicaoInicial.x + posicaoFinal.x / 2,
            posicaoInicial.y + 0.4f,
            posicaoInicial.z + posicaoFinal.z / 2
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

        // Duplica E Embaralha As Possiveis Cartas
        List<Carta> cartasEmbaralhadas = this.embaralharListaDePossiveisCartas(this.duplicarListaDePossiveisCartas(this.possiveisCartas));

        // Inicia A Variavel
        GameObject cartaIniciada;

        for (int indice = 0; indice < numeroDeCartas; indice++)
        {
            // Instancia O Prefab Da Carta
            cartaIniciada = Instantiate(prefabCarta,posicaoDaCarta,Quaternion.identity) as GameObject;

            // Adiciona A Carta Ao Deck
            cartaIniciada.transform.parent = this.transform.Find("Cartas");

            // Adiciona Um ID A Carta
            cartaIniciada.transform.Find("Carta").GetComponent<ValoresDaCarta>().idDaCarta = cartasEmbaralhadas[indice].idDaCarta;

            // Muda O Visual Da Carta
            cartaIniciada.transform.Find("Carta").GetComponentInChildren<MeshRenderer>().material = cartasEmbaralhadas[indice].imagemDaCarta;

            // Adiciona As Cartas À Lista
            this.cartasDoDeck.Add(cartaIniciada.transform);

            // Adiciona A Posição Das Cartas À Lista
            this.posicoesDasCartasNoDeck.Add(cartaIniciada.transform.localPosition);

            // Pega A Posição Da Carta Criada
            posicaoDaCarta = cartaIniciada.transform.Find("Carta").Find("ParteDeTras").position;

            // Adiciona Um Espaçamento Entre Cartas
            posicaoDaCarta.y += offSetEntreCartas;
        }
    }

    // Função Para Mover Cartas De Volta Para O Deck
    public IEnumerator moverCartasDeVoltaParaODeck(Transform[] cartas) {
        // Move As Cartas De Volta Para O Deck
        foreach (Transform carta in cartas)
        {
            IEnumerator coroutine = moverCarta(carta, this.posicaoFinalDasCartasDeVoltaParaODeck);

            StartCoroutine(coroutine);

            this.posicaoFinalDasCartasDeVoltaParaODeck.y += carta.gameObject.GetComponentInChildren<BoxCollider>().size.y + this.offSetEntreCartas;
            
            yield return new WaitForSeconds(this.tempoEntreDistribuicaoDeCartas);
        }
    }

    // Duplica A Lista De Possiveis Cartas
    private List<Carta> duplicarListaDePossiveisCartas(List<Carta> cartas) {
        List<Carta> novaListaDuplicada = new List<Carta>();

        for (int indice = 0; indice < cartas.Count * 2; indice++)
        {
            novaListaDuplicada.Add(cartas[indice % cartas.Count]);
        }

        return novaListaDuplicada;
    }

    // Embaralha A Lista De Possiveis Cartas
    private List<Carta> embaralharListaDePossiveisCartas(List<Carta> cartas) {
        List<Carta> novaListaEmbaralhada = new List<Carta>();

        int tamanhoDaLista = cartas.Count;

        for (int indice = 0; indice < tamanhoDaLista; indice++)
        {
            int posicaoParaPegar = Random.Range(0,cartas.Count);
            novaListaEmbaralhada.Add(cartas[posicaoParaPegar]);
            cartas.RemoveAt(posicaoParaPegar);
        }

        return novaListaEmbaralhada;
    }

    // Embaralha A Lista De Posições Das Cartas No Deck
    private List<Vector3> embaralharListaDePosicoesDasCartasNoDeck(List<Vector3> posicoesCartasNoDeck) {
        List<Vector3> novaListaEmbaralhada = new List<Vector3>();

        int tamanhoDaLista = posicoesCartasNoDeck.Count;

        for (int indice = 0; indice < tamanhoDaLista; indice++)
        {
            int posicaoParaPegar = Random.Range(0,posicoesCartasNoDeck.Count);
            novaListaEmbaralhada.Add(posicoesCartasNoDeck[posicaoParaPegar]);
            posicoesCartasNoDeck.RemoveAt(posicaoParaPegar);
        }

        return novaListaEmbaralhada;
    }

    // Distribui Cada Carta Da Torre
    private IEnumerator moverCartaParaEmbaralhar(Transform carta, Vector3 posicaoFinal, int indice) {
        // Pega A Posição Da Carta
        var posicaoInicial = carta.localPosition;

        // Calcula O Ponto De Controle
        var pontoDeControle = new Vector3(
            posicaoInicial.x + (indice % 2 == 0? 0.2f:-0.2f),//0.4f,//(0.4f * Random.Range(0,2) == 1? 1 : -1),
            posicaoInicial.y + posicaoFinal.y / 2,
            posicaoInicial.z
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

    // Gera A Animação De Embaralhar As Cartas
    private IEnumerator animacaoDeEmbaralharCartas() {
        List<Coroutine> coroutines = new List<Coroutine>();

        // Inicializa As Corotinas Para Mover As Cartas No Baralho
        for (int indice = 0; indice < this.cartasDoDeck.Count; indice++)
        {
            IEnumerator coroutine = this.moverCartaParaEmbaralhar(this.cartasDoDeck[indice],this.novasPosicoesDasCartasNoDeck[indice],indice);
            coroutines.Add(StartCoroutine(coroutine));

            yield return new WaitForSeconds(0.1f);
        }

        // Espera Todas As Coroutinas Acabarem
        foreach (Coroutine coroutine in coroutines)
        {
            yield return coroutine;
        }

        this.ordenarCartasDoDeckDeCimaParaBaixo();

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

    // Usa O Algoritmo De Bubble Sort Para Ordenar As Cartas
    private void ordenarCartasDoDeckDeCimaParaBaixo() {
        for (int indice = 0; indice < this.cartasDoDeck.Count; indice++)
        {
            for (int indiceInterno = 0; indiceInterno < this.cartasDoDeck.Count - 1; indiceInterno++) {
                if (this.cartasDoDeck[indiceInterno].localPosition.y > this.cartasDoDeck[indiceInterno + 1].localPosition.y) {
                    Transform temp = this.cartasDoDeck[indiceInterno];
                    this.cartasDoDeck[indiceInterno] = this.cartasDoDeck[indiceInterno + 1];
                    this.cartasDoDeck[indiceInterno + 1] = temp;
                }
            }
        }
    }
}