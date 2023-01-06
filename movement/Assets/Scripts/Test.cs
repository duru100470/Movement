using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator DoTest()
    {
        while(true)
        {
            Debug.Log("test");
            yield return null;
        }
    }
}
