using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject cut;

    public void Slash()
    {
        Debug.DrawLine(target.transform.position, cut.transform.position);

        (GameObject copyNormalside, _) = MeshCutter.Cut(target, target.transform.position, cut.transform.position);
        
        //copyNormalside.transform.position = target.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
