using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pickupable))]

public class Food : Consumable
{
    public override void Consume(GameObject interactor)
    {
        base.Consume(interactor);

        StartCoroutine(OnConsume(interactor));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    Transform thisTransform;
    private IEnumerator OnConsume(GameObject interactor)
    {
        // For temporary visual feedback, the food shrinks
        thisTransform = transform;
        float duration = 3f;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            float originalSize = thisTransform.localScale.x;
            Vector3 value = Vector3.one * 0.005f;
            thisTransform.localScale -= value;
            value = thisTransform.localScale;
            value.x = Mathf.Clamp(value.x, 0, originalSize);
            thisTransform.localScale = Vector3.one * value.x;

            yield return null;
        }

        interactor.GetComponent<PlayerController>().Bite(); // Release food
        gameObject.SetActive(false);
    }
}
