using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     PlayerScore.ChangeScore(50);
        // }
        scoreText.text = $"{PlayerScore.Score}";
    }

    // private IEnumerator UpdateScore()
    // {

    // }
}
