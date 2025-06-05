using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFlyCollision : MonoBehaviour
{  
    private void OnCollisionEnter2D(Collision2D other){
        if(other.transform.tag == "Obstacle"){
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }
}

