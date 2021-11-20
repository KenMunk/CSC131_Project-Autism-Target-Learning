using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetCarrierManager
{
    public static GameObject setCarrier;

    public static GameObject setCarrierTemplate;

    public static void updateCarrier(GameObject newCarrier)
    {
        GameObject.Destroy(setCarrier);
        setCarrier = newCarrier;
    }


}
