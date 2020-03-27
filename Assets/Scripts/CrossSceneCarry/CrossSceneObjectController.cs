using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneObjectController : MonoBehaviour
{
    [SerializeField] private GuidReference crossSceneGuidObject;

    private bool GuidReferenceAvailable => crossSceneGuidObject.gameObject != null;

    private CrossSceneObject crossSceneObject;
    private CrossSceneObject CrossSceneObject
    {
        get
        {
            crossSceneObject = crossSceneObject ?? crossSceneGuidObject.gameObject.GetComponent<CrossSceneObject>();
            return crossSceneObject;
        }
        set { crossSceneObject = value; }
    }

    private Deliverable deliverable;
    private Deliverable Deliverable
    {
        get
        {
            deliverable = deliverable ?? crossSceneGuidObject.gameObject.GetComponent<Deliverable>();
            return deliverable;
        }
        set { deliverable = value; }
    }

    public void SetActive(bool value)
    {
        if (!GuidReferenceAvailable) return;
        crossSceneGuidObject.gameObject.SetActive(value);
    }

    public void SetParent(Transform transform)
    {
        if (!GuidReferenceAvailable) return;
        crossSceneGuidObject.gameObject.transform.SetParent(transform);
    }

    public void IsKinematic(bool value)
    {
        if (!GuidReferenceAvailable) return;

        Rigidbody rb = GetComponentRequirement<Rigidbody>();
        if (rb == null) return;

        rb.isKinematic = value;
    }

    public void MoveToPersistentScene()
    {
        if (!GuidReferenceAvailable) return;

        CrossSceneObject = GetComponentRequirement<CrossSceneObject>();
        if (CrossSceneObject == null) return;

        CrossSceneObject.MoveToPersistentScene();
    }

    public void MoveToCurrentActiveScene(Transform parent = null)
    {
        if (!GuidReferenceAvailable) return;

        CrossSceneObject = GetComponentRequirement<CrossSceneObject>();
        if (CrossSceneObject == null) return;

        CrossSceneObject.MoveToCurrentActiveScene(parent);
    }

    #region For Deliverables only
    public void DeliverableEnable(bool updateData)
    {
        if (!GuidReferenceAvailable) return;

        Deliverable = GetComponentRequirement<Deliverable>();
        if (Deliverable == null) return;

        Deliverable.Enable(updateData);
    }

    public void DeliverableDisable(bool updateData)
    {
        if (!GuidReferenceAvailable) return;

        Deliverable = GetComponentRequirement<Deliverable>();
        if (Deliverable == null) return;

        Deliverable.Disable();
    }

    public void DeliverableActivate()
    {
        if (!GuidReferenceAvailable) return;

        Deliverable = GetComponentRequirement<Deliverable>();
        if (Deliverable == null) return;

        Deliverable.Activate();
    }
    #endregion

    private T GetComponentRequirement<T>()
    {
        T component = crossSceneGuidObject.gameObject.GetComponent<T>();

        //if (component == null)
        //    Debug.LogErrorFormat("{0} does not have a {1} component", crossSceneObject.gameObject.name, typeof(T).ToString());

        //Debug.LogFormat("{0} Has component {1}? {2}", crossSceneObject.gameObject.name, typeof(T).ToString(), "true");
        return component;
    }
}
