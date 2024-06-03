using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour
{

    Vector3 endPointCoordinate;
    public enum ObjectName
    {
        Hook,
        Slide,
        LongSlide,
        Walls,
        Tiles
    }

    [SerializeField]
    private ObjectName objectName;



    public void MakeGeometricChanges(GameObject priorGameObject)
    {
        if(priorGameObject!=null)
        Debug.Log(priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate);

        if (objectName == ObjectName.Walls)
        {

            endPointCoordinate = transform.position;// + new Vector3(0,0,80f);

            if(priorGameObject != null)
            {
                if(priorGameObject.GetComponent<ObjectInfo>().objectName ==  ObjectName.Hook)
                {
                    transform.position += priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0,0,25f);
                }
                else
                {
                    transform.position = priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0,0,3f);
                }
            }
        }
        else if (objectName == ObjectName.Tiles)
        {

            endPointCoordinate = transform.position;// + new Vector3(0,0,8.25f);

            if(priorGameObject != null)
            {
                if (priorGameObject.GetComponent<ObjectInfo>().objectName == ObjectName.Hook)
                {
                    transform.position += priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate;// + new Vector3(0, 10f, 10f);
                }
                else
                {
                    transform.position = priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate;// + new Vector3(0, 0, 3f);
                }

            }
        }

        else if (objectName == ObjectName.Slide)
        {
            transform.Rotate(new Vector3(30, transform.rotation.y, transform.rotation.z));
            endPointCoordinate = transform.position;// + new Vector3(0,-71,125f);

            if (priorGameObject != null)
            {
                if (priorGameObject.GetComponent<ObjectInfo>().objectName == ObjectName.Hook)
                {
                    transform.position += priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate;// + new Vector3(0, -10f, 21f);
                }
                else
                {
                    transform.position = priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0, 0, 1f);
                }

            }
        }
        else if(objectName == ObjectName.LongSlide)
        {
            transform.Rotate(new Vector3(30, transform.rotation.y, transform.rotation.z));
            endPointCoordinate = transform.position;// + new Vector3(0,-116,202f);

            if (priorGameObject != null)
            {
                if (priorGameObject.GetComponent<ObjectInfo>().objectName == ObjectName.Hook)
                {
                    transform.position += priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0, -10f, 21f);
                }
                else
                {
                    transform.position = priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0, 0, 1f);
                }

            }
        }
        else if(objectName == ObjectName.Hook)
        {
            endPointCoordinate = transform.position;
            if (priorGameObject != null)
            {
                if (priorGameObject.GetComponent<ObjectInfo>().objectName == ObjectName.Hook)
                {
                    transform.position += priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate; //+ new Vector3(0, 0f, 10f);
                }
                else
                {
                    transform.position = priorGameObject.GetComponent<ObjectInfo>().endPointCoordinate;// + new Vector3(0, -12f, 3f);
                }

            }
        }

        

    }
}