using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Digable : MonoBehaviour, IInteractable
{
    public UnityEvent OnDig;
    public UnityEvent OnEndDig;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int ObjectToSpawnJumpSpeed = 300;
    [SerializeField] float DespawnTimer = 10f;
    [SerializeField] Vector3 scaleIncrease;

    private Collider collider;
    private int currentHp;
    private static EventManager eventManager;
    private Vector3 originalScale;

    public GameObject ObjectToSpawn { get => objectToSpawn; set => objectToSpawn = value; }

    public void DisplayInteractability()
    {

    }

    public void Interact(Interactor source, IInteractable target)
    {
        if (!enabled) return;
        if (currentHp <= 0) return;
        {
            OnDig.Invoke();
            TakeDamage(source, source.GetComponent<Dig>().DigPower);
        }
    }

    void Awake()
    {
        collider = gameObject.GetComponent<Collider>();
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        currentHp = maxHealth;
        collider.enabled = true;
        eventManager = eventManager ?? SingletonManager.GetInstance<EventManager>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        Dig digSource = collider.GetComponent<Dig>();

        // Player ignores digable  terrain collisions
        if (digSource != null) Physics.IgnoreLayerCollision(8, 9, true);
    }

    private void OnTriggerExit(Collider collider)
    {
        Dig digSource = collider.GetComponent<Dig>();

        if (digSource != null)
        {
            StartCoroutine(DespawnDigable());
            Physics.IgnoreLayerCollision(8, 9, false); // Player can collide with digable terrain
        }
    }

    public void TakeDamage(Interactor source, int damageValue)
    {
        currentHp -= damageValue;
        currentHp = Mathf.Clamp(currentHp, 0, maxHealth);

        if (currentHp <= 0)
        {
            DugUp(source);
            return;
        }
        transform.localScale += scaleIncrease;
    }

    public void DugUp(Interactor source)
    {
        Dig sourceDig = source.GetComponent<Dig>();
        if (sourceDig != null)
        {
            Physics.IgnoreLayerCollision(8, 9, false); // Player can collide with digable terrain
            collider.enabled = false;
            SpawnObject();
            StartCoroutine(DespawnDigable());
            OnEndDig.Invoke();
        }
    }

    public void SpawnObject()
    {
        if (ObjectToSpawn != null)
        {
            //GameObject newObjectToSpawn = Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
            //newObjectToSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * ObjectToSpawnJumpSpeed); // Make newly spawned object jump up

            // Temporary
            // If it's a deliverable, show the model
            Deliverable deliverable = ObjectToSpawn.GetComponent<Deliverable>();
            if (!deliverable.Delivered)
                deliverable.Enable();
            else if (deliverable.Delivered) // If already delivered, don't respawn
                return;

            // Reposition and make sure gameObject is active
            ObjectToSpawn.transform.position = transform.position;
            ObjectToSpawn.SetActive(true);

            // Make newly spawned object jump up
            ObjectToSpawn.GetComponent<Rigidbody>().AddForce(Vector3.up * ObjectToSpawnJumpSpeed);

            // Broadcast that an object has been found
            eventManager.Trigger<DigableEvent, Digable>(this);
        }
        else
        {
            //Spawn or display some sort of indicator that there is nothing to dig here
        }
    }

    public IEnumerator DespawnDigable()
    {
        yield return new WaitForSeconds(DespawnTimer);
        transform.localScale = originalScale;
        gameObject.SetActive(false);
    }

    public void HideInteractability()
    {

    }
}