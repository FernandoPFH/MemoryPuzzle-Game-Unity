using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarPosicaoDaCarta : MonoBehaviour
{
    // Desenha Um Cubo Vazado Da Possivel Posição Da Carta
    void OnDrawGizmos()
    {
        var tamanhoDoCubo = GetComponent<BoxCollider>().size;
        var posicaoDoCentroDoCubo = this.transform.position;
        posicaoDoCentroDoCubo.y += tamanhoDoCubo.y / 2;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(posicaoDoCentroDoCubo, tamanhoDoCubo);
    }
}
