using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenerestart : MonoBehaviour
{

    public void Restart(){
        SceneManager.LoadScene("level 1");
    }
}

