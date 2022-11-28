using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public int needPartsNum;
    
    public GameObject[] buildingParts;

    public GameObject endCanvas;
    
    private float countNum;
    private float changeValue;
    public int checkIndex;
    
    public GameObject HouseEffect;
    
    void Start()
    {
        countNum = 0;

        changeValue = (float)needPartsNum / buildingParts.Length;
        checkIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (buildingParts.Length > checkIndex)
        {
            while (countNum-changeValue>=0)
            {
                countNum -= changeValue;

                var go = Instantiate(buildingParts[checkIndex]);
                go.transform.localScale = this.transform.localScale;
                go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y,checkIndex * 0.1f);
                // go.transform.SetParent(this.transform);
                // go.transform.localPosition = new Vector3(0, 0,go.transform.position.z);

                checkIndex += 1;
                Vector3 effectPos = new Vector3(transform.position.x, transform.position.y, 0);
                Instantiate(HouseEffect, effectPos,quaternion.identity);
;            }
        }
        if(checkIndex==buildingParts.Length)
        {
            Time.timeScale = 0f;
            endCanvas.SetActive(true);
            SoundController.Instance.PlaySFXSound("건축 성공시 결과화면에서 나오는 소리");
        }


    }

    public void Building(GameObject hand)
    {
        GameObject playerItem;
        playerItem = hand.transform.GetChild(0).gameObject;

        if (playerItem.name == "Last_Parts" && buildingParts.Length > checkIndex)
        {
            playerItem.transform.SetParent(this.transform);
            //playerItem.SetActive(false);
            Destroy(playerItem);
            countNum += 1;
        }
    }
}
