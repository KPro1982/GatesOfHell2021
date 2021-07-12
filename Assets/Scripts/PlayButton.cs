using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    
}
