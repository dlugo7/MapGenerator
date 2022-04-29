using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public enum Type
    {
        Ocean,
        ShallowWater,
        Water,
        Lava,
        Magma,
        Grass,
        Dirt,
        Mud,
        Sand,
        Rock
    }

    public Type tileType;

    void Awake(){}

    void FixedUpdate()
    {
        float num = Random.Range(1f, 250f);

        switch (tileType)
        {
            case Type.Ocean:
                //test
                if(num >= 210f && transform.position.y < 0.35f)
                    this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 24f, 0f), ForceMode.Impulse);
                break;
            case Type.ShallowWater:
                if (num >= 220f && transform.position.y < 0.5f)
                    this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 20f, 0f), ForceMode.Impulse);
                break;
            case Type.Water:
                if (num >= 220f && transform.position.y < 0.5f)
                    this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 22f, 0f), ForceMode.Impulse);
                break;
            case Type.Lava:
                if (num >= 249.90f && transform.position.y < 1f)
                    this.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 200f, 0f), ForceMode.Impulse);
                break;
            default:
                break;
        }
    }
}
