using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool scrollLeft;
    [SerializeField] int reset;
    float singleTextureWidth;

    void Start(){
        SetupTexture();
        if(scrollLeft) moveSpeed = -moveSpeed;
    }

    void SetupTexture(){
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }
    void Scroll(){
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta * Mathf.Pow(Spawner.instance.timeAlive, Spawner.instance.obstacleSpeedFactor), 0f, 0f);
    }

    void CheckReset(){
        if((Mathf.Abs(transform.position.x) - singleTextureWidth) > reset){
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
    void Update()
    {
        if (GameManager.Instance.isPlaying == true)
        {
            Scroll();
            CheckReset();
        }
    }
}
