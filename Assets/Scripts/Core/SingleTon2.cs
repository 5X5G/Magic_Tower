using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon2 : MonoBehaviour {

    public static SingleTon2 Instance
    {
        get
        {
            if (instance == null)
                instance = new SingleTon2();
            return instance;
        }
    }

    private static SingleTon2 instance;
}
