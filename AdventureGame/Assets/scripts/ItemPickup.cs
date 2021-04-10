using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("made a change");
        Pickup();
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
            Destroy(this.gameObject);
    }
}
