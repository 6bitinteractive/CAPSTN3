using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneObjectController : MonoBehaviour
{
    [SerializeField] private GuidReference crossSceneObject;

    private bool GuidReferenceAvailable => crossSceneObject.gameObject != null;

    private CrossSceneObject crossScene;
    private CrossSceneObject CrossSceneObject => crossScene ?? crossSceneObject.gameObject.GetComponent<CrossSceneObject>();

    private Deliverable deliverable;
    private Deliverable Deliverable
    {
        get { return deliverable ?? crossSceneObject.gameObject.GetComponent<Deliverable>(); }
        set { deliverable = value; }
    }

    public void SetActive(bool value)
    {
        if (!GuidReferenceAvailable) return;
        crossSceneObject.gameObject.SetActive(value);
    }

    public void SetParent(Transform transform)
    {
        if (!GuidReferenceAvailable) return;
        crossSceneObject.gameObject.transform.SetParent(transform);
    }

    public void IsKinematic(bool value)
    {
        if (!GuidReferenceAvailable) return;

        Rigidbody rb = GetComponentRequirement<Rigidbody>();
        if (rb == null) return;

        rb.isKinematic = value;
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
        T component = crossSceneObject.gameObject.GetComponent<T>();

        //if (component == null)
        //    Debug.LogErrorFormat("{0} does not have a {1} component", crossSceneObject.gameObject.name, typeof(T).ToString());

        //Debug.LogFormat("{0} Has component {1}? {2}", crossSceneObject.gameObject.name, typeof(T).ToString(), "true");
        return component;
    }
}
