using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaDoJogo : MonoBehaviour
{
    public Animator animadorDoMenuInicial;
    public Animator animadorDoDeck;
    public FuncoesDoDeck deck;
    public FuncoesDoJogador jogador;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Função Chamada Pelo Botão
    public void comecarJogo() {
        StartCoroutine(this.comecarFuncoesJogo());
    }

    // Função Para Começar As Animações Do Jogo
    private IEnumerator comecarFuncoesJogo() {
        // Começa A Animação Do Menu Inicial E Do Deck
        animadorDoMenuInicial.SetTrigger("Fechar");
        animadorDoDeck.SetTrigger("IrParaACamera");

        // Espera Por Um Tempo
        yield return new WaitForSeconds(0.1f);

        // Espera A Animação Acabar
        while (animadorDoMenuInicial.GetCurrentAnimatorClipInfo(0)[0].clip.name == "MenuInicial_FadeOut" || animadorDoDeck.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Deck_IrParaACamera" ) {
            yield return new WaitForEndOfFrame();
        }

        // Espera Por Um Tempo
        yield return new WaitForSeconds(0.3f);

        // Começa Animação De Embaralhar As Cartas
        yield return StartCoroutine(deck.animacaoDeEmbaralharCartas());
        
        // Começa Animação De Voltar O Deck
        animadorDoDeck.SetTrigger("VoltarDaCamera");

        // Espera Por Um Tempo
        yield return new WaitForSeconds(0.1f);

        // Espera A Animação Acabar
        while (animadorDoDeck.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Deck_VoltarDaCamera" ) {
            yield return new WaitForEndOfFrame();
        }
        
        // Começar A Distribuir As Cartas Na Mesa
        yield return StartCoroutine(deck.moverTodasAsCartas());

        // Habilita O Jogador A Clicar Nas Cartas
        jogador.podeClicarEmUmaCarta = true;
    }
}
