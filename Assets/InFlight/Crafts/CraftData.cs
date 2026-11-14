using System;

namespace Crafts
{
    [Serializable]
    public class CraftData
    {
        public int id;
        public string name;
        public long timeOfLastUpdate;
        public float mass;
        public float[] position = new float[3];
        public float[] rotation = new float[4];
        public float[] velocity = new float[3];
        public float[] angularVelocity = new float[3];

        public CraftData(int id, string name, long timeOfLastUpdate, float mass, float[] position, float[] rotation, float[] linearVelocity, float[] angularVelocity)
        {
            this.id = id;
            this.name = name;
            this.timeOfLastUpdate = timeOfLastUpdate;
            this.mass = mass;
            this.position = position;
            this.rotation = rotation;
            this.velocity = linearVelocity;
            this.angularVelocity = angularVelocity;
        }
    }
}