using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public float threshold;
    private bool doReset;
    // Start is called before the first frame update
    void Start()
    {
        doReset = Input.GetButton("Fire3");
    }

    // Update is called once per frame
    void Update()
    {
        DoReset();

    }
    private void DoReset()
    {
        if ((transform.position.y < threshold) || doReset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
