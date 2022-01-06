using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestarAnimacoes : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            animator.SetTrigger("Abrir");
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            animator.SetTrigger("Fechar");
        }
    }
}
