using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecharOJogo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Quando Apertar "ESC" Fecha O Jogo
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
