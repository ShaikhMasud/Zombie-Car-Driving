using UnityEngine;
using UnityEngine.SceneManagement;

public class back : MonoBehaviour {
    public void RestartGame()
    {
        Debug.Log("You Win!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
