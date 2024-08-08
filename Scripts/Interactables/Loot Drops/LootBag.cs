using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList =new List<Loot>();

    [SerializeField]
    float dropForce;

    /// <summary>
    /// Selects a random item to be dropped based on their drop chance.
    /// </summary>
    /// <returns>item that is selected to be dropped</returns>
    private Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //gets a random number between 1 - 100
        List<Loot> possibleItems = new List<Loot>(); //
        
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }

        }

        if (possibleItems.Count > 0) 
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)]; 
            return droppedItem;
        }

        return null;
    }

    /// <summary>
    /// Instantiates the selected loot item at the specified position with a random force applied to it.
    /// </summary>
    /// <param name="spawnPosition">The position where the loot item will be spawned</param>
    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();

        if (droppedItem != null) 
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            Collectable collectable = lootGameObject.GetComponent<Collectable>();
            collectable.lootData = droppedItem;


            Vector2 dropDirection = new Vector2(Random.Range(-1,1f), Random.Range(-1,1f));
            lootGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
        }
    }

}
