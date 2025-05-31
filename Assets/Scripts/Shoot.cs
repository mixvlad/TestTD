using UnityEngine;

public class Shoot : MonoBehaviour
{
    GameObject currentTarget;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            currentTarget = other.gameObject;
        }


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
