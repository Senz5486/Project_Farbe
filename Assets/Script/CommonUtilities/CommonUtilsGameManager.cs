using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtilsGameManager : MonoBehaviour
{
    [SerializeField] private string gameVersion;

    private void Awake()
    {
        gameVersion = CommonUtilsMessageConstants.GAMEVERSION_TEXT + Application.version;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
