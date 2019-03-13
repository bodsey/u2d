using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
