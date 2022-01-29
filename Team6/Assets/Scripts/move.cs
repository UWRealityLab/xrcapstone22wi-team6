using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class move : MonoBehaviour
{
    private XRController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = this.gameObject.GetComponentInChildren<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool menuButton)) {
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10));
        }
    }

}
