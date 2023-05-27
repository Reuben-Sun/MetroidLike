using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private static PlayerInfo instance;
    // 私有构造函数
    private PlayerInfo(){}
    
    private float health = 100.0f;

    public static PlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerInfo>();
            }

            return instance;
        }
    }

    public Vector3 PlayerPosition
    {
        get
        {
            return transform.position;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
    }
}
