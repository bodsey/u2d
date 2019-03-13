using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour {

    public bool UseCoins = true;
    public int Coins = 0;
    public int Multiplier = 1;

    private Text txt; 

	void Start ()
    {
        txt = GameObject.Find("CoinText").GetComponent<Text>();
        if(!UseCoins)
        {
            txt.gameObject.SetActive(false);
        }
        else
        {
            txt.text = Coins.ToString();
        }
	}	
    public void GetCoin(int value)
    {
        Coins += value * Multiplier;
        txt.text = Coins.ToString();
    }
}
