using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshManger : MonoBehaviour {

    private void Awake()
    {
        Nglobal.refreshManger = this;
    }

    private void Start()
    {
        
    }
}
