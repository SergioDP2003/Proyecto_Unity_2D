using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WalkAnimation : MonoBehaviour
{
    Animator animator;
    Colisiones_Cap1 colisiones;
    Mover_Cap1 mover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colisiones = GetComponent<Colisiones_Cap1>();
        mover = GetComponent<Mover_Cap1>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        animator.SetBool("Grounded", colisiones.Grounded());
        animator.SetFloat("VelocityX", Mathf.Abs(mover.rb2D.linearVelocity.x));  
    }

}
