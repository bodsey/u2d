using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatRules : MonoBehaviour {

    public bool UseCheckpoints = true;
    public bool HasLives = true;
    public int Lives = 3;

    private GameObject player;
    private Vector3 checkpointPos;
    private Text livesTxt;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        checkpointPos = player.transform.position;
        livesTxt = GameObject.Find("LivesText").GetComponent<Text>();
        if (!HasLives)
        {
            livesTxt.gameObject.SetActive(false);
        }
        else
            livesTxt.text = Lives.ToString();
	}
	public void SetCheckpointPos(Vector3 pos)
    {
        if(UseCheckpoints)
            checkpointPos = pos;
    }
    public void Death()
    {
        if (HasLives)
        {
            Lives--;
            livesTxt.text = Lives.ToString();
            if (Lives <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                return;
            }
        }
        if(UseCheckpoints)
            player.transform.position = checkpointPos;
    }
	
}
