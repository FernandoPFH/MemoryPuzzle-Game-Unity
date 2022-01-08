using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Carta",menuName = "Cartas")]
public class Carta : ScriptableObject
{
    public Material imagemDaCarta;
    public int idDaCarta;
}