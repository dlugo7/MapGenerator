using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    [SerializeField] public enum Theme
    {
        DeepOcean = 0,
        Beach     = 1,
        Swamp     = 2,
        Forest    = 3,
        Magma     = 4,
        Desert    = 5
    }
    [SerializeField] public Theme regionType;

    // Possibilities of materials for regions
    [SerializeField] Material grass;
    [SerializeField] Material deepOcean;
    [SerializeField] Material shallowWater;
    [SerializeField] Material water;
    [SerializeField] Material mud;
    [SerializeField] Material magma;
    [SerializeField] Material sand;

    // Start is called before the first frame update
    void Awake()
    {
        // 
        regionType = (Theme)Random.Range(0, System.Enum.GetValues(typeof(Theme)).Length);

        switch (regionType)
        {
            case Theme.Forest:
                GetComponent<MeshRenderer>().material = grass;
                break;
            case Theme.DeepOcean:
                GetComponent<MeshRenderer>().material = deepOcean;
                break;
            case Theme.Beach:
                GetComponent<MeshRenderer>().material = shallowWater;
                break;
            case Theme.Swamp:
                GetComponent<MeshRenderer>().material = mud;
                break;
            case Theme.Magma:
                GetComponent<MeshRenderer>().material = magma;
                break;
            case Theme.Desert:
                GetComponent<MeshRenderer>().material = sand;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
