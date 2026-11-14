using UnityEngine;

namespace Crafts
{
    public class Craft
    {
        public int id;
        public string name;
        public GameObject craftGameObject;

        public Craft(CraftData craftData, GameObject craftGameObject)
        {
            this.id = craftData.id;
            this.name = craftData.name;
            craftGameObject.name = "craft" + craftData.id;
            this.craftGameObject = craftGameObject;
        }

        public void updateCraft(CraftData craftData)
        {
            craftGameObject.transform.position = new Vector3(craftData.position[0], craftData.position[1], craftData.position[2]);
            craftGameObject.transform.rotation = new Quaternion(craftData.rotation[0], craftData.rotation[1], craftData.rotation[2], craftData.rotation[3]);

            Rigidbody craftRb = craftGameObject.GetComponent<Rigidbody>();
            craftRb.mass = craftData.mass;
            craftRb.linearVelocity = new Vector3(craftData.velocity[0], craftData.velocity[1], craftData.velocity[2]);
            craftRb.angularVelocity = new Vector3(craftData.angularVelocity[0], craftData.angularVelocity[1], craftData.angularVelocity[2]);
        }
    }
}
