using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InputHandler : MonoBehaviour
{
    [SerializeField]
    CarHa carHandler;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input=Vector2.zero;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        carHandler.SetInput(input);
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
