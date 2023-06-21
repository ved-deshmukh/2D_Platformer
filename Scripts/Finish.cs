using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{

    private Animator anim;
    private AudioSource finishSound;
    private int time = 0;
    void Start()
    {
        finishSound = GetComponent<AudioSource>(); 
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (time == 0)
            {
                finishSound.Play();
                time++;
            }  
            anim.SetTrigger("Finish");
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
