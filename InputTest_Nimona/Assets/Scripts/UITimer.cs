using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{	
    [SerializeField] private Text TimerText; 
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        TimerText.text = timer.ToString("00");
    }
}