using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;

    public int startIndex;
    public int endIndex;

    public Transform[] sprites;

    float viewHeight;

    void Awake() {
        //실제 뷰의 높이 구하기
        viewHeight = Camera.main.orthographicSize * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Scrolling();        
    }

    void Move() {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
        transform.position = curPos + nextPos;
    }

    void Scrolling() {
        if(sprites[endIndex].position.y < viewHeight*(-1)) {
            //sprite 재사용
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * viewHeight;

            //이동이 완료된 후 EndIndex, StartIndex 갱신하기
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexSave-1 == -1) ? (sprites.Length-1) : (startIndexSave-1);
        }
    }
}
