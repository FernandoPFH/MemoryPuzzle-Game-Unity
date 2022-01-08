using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncoesDoJogador : MonoBehaviour
{
    public GameObject deckDeCartas;

    // Variáveis Acessiveis De Dentro Da Classe
    private GameObject primeiraCartaSelecionada = null;
    private bool podeClicarEmUmaCarta = true;
    private int pontuacaoDoJogador = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checa Se O Botão Esquerdo Do Mouse Foi Clicado
        if (Input.GetMouseButtonDown(0)){
            this.clicarNaTela();
        }
    }

    // Converte O Click Na Tela
    void clicarNaTela()
    {
        if (podeClicarEmUmaCarta) { 
            RaycastHit raycastHit;
            // Converte A Posição Do Mouse Para Um Raio Dentro Da Cena Da Unity
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Checa Se O Raio Bateu Em Algo
            if (Physics.Raycast(ray, out raycastHit, 100f)){
                // Checa Se O Colisor Tem Um Componente De "Transform"
                if (raycastHit.collider.transform != null){
                    // Checa Se A Tag Do Colisor É "Carta"
                    if (raycastHit.collider.transform.tag == "Carta") {
                        // Checa Se A Primeira Carta Já Foi Escolhida
                        if (primeiraCartaSelecionada == null) {
                            // Seta A Primeira Carta Para O Colisor
                            primeiraCartaSelecionada = raycastHit.collider.gameObject;
                            // Começa A Animação De Abir A Carta
                            primeiraCartaSelecionada.GetComponent<Animator>().SetTrigger("Abrir");
                        } else {
                            // Checa Se O Colisor E A Primeira Carta Não São A Mesma
                            if (primeiraCartaSelecionada != raycastHit.collider.gameObject) {
                                // Começa A Coroutine De Abrir A Segunda Carta
                                IEnumerator coroutine = abrirSegundaCarta(raycastHit.collider.gameObject);
                                StartCoroutine(coroutine);
                            }
                        }
                    }
                }
            }
        }
    }

    // Função Para Abrir A Segunda Carta
    private IEnumerator abrirSegundaCarta(GameObject cartaAAbrir) {
        // Impede O Jogador De Clicar Em Outra Carta
        podeClicarEmUmaCarta = false;

        // Começa A Animação De Abir A Carta
        cartaAAbrir.GetComponent<Animator>().SetTrigger("Abrir");

        // Espera Um Tempo
        yield return new WaitForSeconds(0.1f);

        // Espera A Animação Acabar
        while (cartaAAbrir.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Carta_Abrir") {
            yield return new WaitForEndOfFrame();
        }


        // Avalia Se As Duas Cartas São Iguais
        if (primeiraCartaSelecionada.GetComponent<ValoresDaCarta>().idDaCarta == cartaAAbrir.GetComponent<ValoresDaCarta>().idDaCarta) {
            // Espera Um Tempo
            yield return new WaitForSeconds(0.2f);

            // Pula As Cartas
            primeiraCartaSelecionada.GetComponent<Animator>().SetTrigger("Pular");
            cartaAAbrir.GetComponent<Animator>().SetTrigger("Pular");

            // Espera Um Tempo
            yield return new WaitForSeconds(0.1f);

            // Espera A Animação Acabar
            while (cartaAAbrir.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Carta_Pular") {
                yield return new WaitForEndOfFrame();
            }

            pontuacaoDoJogador += 1;

            // Retira A Tag Do Objeto
            primeiraCartaSelecionada.tag = "Untagged";
            cartaAAbrir.tag = "Untagged";

            // Cria Uma Lista Das Cartas Para Mover
            Transform[] cartas = new Transform[2];
            cartas[0] = primeiraCartaSelecionada.transform.parent;
            cartas[1] = cartaAAbrir.transform.parent;

            // Espera Um Tempo
            yield return new WaitForSeconds(0.5f);

            // Fecha As Cartas
            primeiraCartaSelecionada.GetComponent<Animator>().SetTrigger("Fechar");
            cartaAAbrir.GetComponent<Animator>().SetTrigger("Fechar");
            

            // Espera Um Tempo
            yield return new WaitForSeconds(0.1f);

            // Espera A Animação Acabar
            while (cartaAAbrir.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Carta_Fechar") {
                yield return new WaitForEndOfFrame();
            }

            // Começa A Corotina Para Devolver As Cartas Para O Deck
            IEnumerator coroutine =deckDeCartas.GetComponent<FuncoesDoDeck>().moverCartasDeVoltaParaODeck(cartas);
            StartCoroutine(coroutine);

            // Deseta A Primeira Carta
            primeiraCartaSelecionada = null;

            // Permite O Jogador A Clicar Em Outra Carta
            podeClicarEmUmaCarta = true;
        } else {
            // Espera Um Tempo
            yield return new WaitForSeconds(0.2f);

            // Balança As Cartas
            primeiraCartaSelecionada.GetComponent<Animator>().SetTrigger("Balancar");
            cartaAAbrir.GetComponent<Animator>().SetTrigger("Balancar");

            // Espera Um Tempo
            yield return new WaitForSeconds(0.1f);

            // Espera A Animação Acabar
            while (cartaAAbrir.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.name == "Carta_Balancar") {
                yield return new WaitForEndOfFrame();
            }

            // Espera Um Tempo
            yield return new WaitForSeconds(0.5f);

            // Fecha As Cartas
            primeiraCartaSelecionada.GetComponent<Animator>().SetTrigger("Fechar");
            cartaAAbrir.GetComponent<Animator>().SetTrigger("Fechar");

            // Deseta A Primeira Carta
            primeiraCartaSelecionada = null;

            // Permite O Jogador A Clicar Em Outra Carta
            podeClicarEmUmaCarta = true;
        }
    }
}