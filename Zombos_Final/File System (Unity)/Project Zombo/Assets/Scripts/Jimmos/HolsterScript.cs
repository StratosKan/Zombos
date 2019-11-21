using UnityEngine;

public class HolsterScript : MonoBehaviour
{
    public GameObject[] guns;
    private InputManager input;

    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (!gameObject.GetComponentInChildren<GunScript>().isReloading)
        {
            if (input.KeyOne)
            {
                guns[0].SetActive(true);
                guns[1].SetActive(false);
                guns[2].SetActive(false);
            }

            if (input.KeyTwo)
            {
                guns[0].SetActive(false);
                guns[1].SetActive(true);
                guns[2].SetActive(false);
            }

            if (input.KeyThree)
            {
                guns[0].SetActive(false);
                guns[1].SetActive(false);
                guns[2].SetActive(true);
            }
        }
    }
}