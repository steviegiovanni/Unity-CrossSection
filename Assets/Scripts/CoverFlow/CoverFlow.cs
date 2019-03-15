// author: Stevie Giovanni 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a simple cover flow
/// </summary>
public class CoverFlow : MonoBehaviour {
    public List<Texture2D> Images;
    public float Size = 1.0f;
    public GameObject CoverPrefab;
    public float FlipTime = 1.0f;

    bool IsFlipping = false;
    int IndexInFront = 0;
    float internalFlipTime = 1.0f;

    public void ClearCoverFlow()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }

    public void InitializeCoverFlow()
    {
        IndexInFront = 0;
        for(int i = 0; i < Images.Count; i++)
        {
            Instantiate(CoverPrefab, transform.position + i * Size / (Images.Count - 1) * transform.forward, transform.rotation, transform);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.C))
            ClearCoverFlow();

        if (Input.GetKeyUp(KeyCode.I))
            InitializeCoverFlow();

        if (Input.GetKeyUp(KeyCode.F) && !IsFlipping)
        {
            internalFlipTime = FlipTime;
            StartCoroutine(FlipNTimesCoroutine(1));
        }

        if (Input.GetKeyUp(KeyCode.M) && !IsFlipping)
        {
            internalFlipTime = FlipTime / 3;
            StartCoroutine(FlipNTimesCoroutine(3));
        }
    }

    public IEnumerator FlipNTimesCoroutine(int n)
    {
        IsFlipping = true;
        for (int i = 0; i < n; i++)
            yield return StartCoroutine(FlipCoroutine());
        IsFlipping = false;
        yield return 0;
    }

    public IEnumerator FlipCoroutine()
    {
        float StartTime = Time.time;
        float DeltaTime = Time.time - StartTime;
        while(DeltaTime < internalFlipTime)
        {
            for(int i = 0; i<transform.childCount; i++)
            {
                GameObject curChild = transform.GetChild(i).gameObject;
                if(i == IndexInFront)
                {
                    if(DeltaTime < internalFlipTime / 3){
                        //curChild.transform.position += curChild.transform.right * Time.deltaTime;
                        curChild.transform.position = Vector3.MoveTowards(curChild.transform.position, transform.position + transform.right, 3 / internalFlipTime * Time.deltaTime);// Time.deltaTime);
                    }else if(DeltaTime < 2 * internalFlipTime / 3){
                        //curChild.transform.position += curChild.transform.forward * Size * (DeltaTime - PrevDeltaTime) * 3 / FlipTime;
                        curChild.transform.position = Vector3.MoveTowards(curChild.transform.position,transform.position + transform.right + transform.forward * Size, Size * 3 / internalFlipTime * Time.deltaTime);
                    }else{
                        //curChild.transform.position -= curChild.transform.right * Time.deltaTime;
                        curChild.transform.position = Vector3.MoveTowards(curChild.transform.position, transform.position + transform.forward * Size, 3 / internalFlipTime * Time.deltaTime);
                    }
                }
                else
                {
                    //curChild.transform.position -= curChild.transform.forward * Size / Images.Count * (DeltaTime - PrevDeltaTime);
                    curChild.transform.position = Vector3.MoveTowards(curChild.transform.position,transform.position + (((i - 1 - IndexInFront) + Images.Count) % Images.Count) * transform.forward * Size / (Images.Count - 1), Size / (Images.Count - 1) / internalFlipTime * Time.deltaTime);
                }
            }
            DeltaTime = Time.time - StartTime;
            yield return 0;
        }
        IndexInFront++;
        if (IndexInFront == Images.Count)
            IndexInFront = 0;
        yield return 0;
    }
}
