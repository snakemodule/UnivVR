using System;
using UnityEngine;


/*Allows access to properties of the Transform of a GameObject 
 * but forbids changing any values
 */

class TransformWrapper
{
    private Transform transform;

    public TransformWrapper (Transform tr)
    {
        this.transform = tr;
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    public Quaternion getRotation()
    {
        return transform.rotation;
    }

}
