using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "GameClear";

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.speed = Mathf.Clamp(animator.speed, 1, 30);

        if(animator.speed > 1)
        {
            animator.speed -= Time.deltaTime * 5;
        }
        else if(animator.speed < 1)
        {
            animator.speed = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
        if(collision.transform.tag == "Star")
        {
            animator.speed += 3;
        }
    }
}
