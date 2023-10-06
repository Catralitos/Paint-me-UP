// Code adapted from https://github.com/mixandjam/Splatoon-Ink
// Comments mine to help me understand/modify code

using System.Collections.Generic;
using UnityEngine;

namespace Painting
{
    public class ParticlesController: MonoBehaviour{
        public Color paintColor;
    
        public float minRadius = 0.05f;
        public float maxRadius = 0.2f;
        public float strength = 1;
        public float hardness = 1;
        [Space]
        ParticleSystem part;
        List<ParticleCollisionEvent> collisionEvents;

        void Start(){
            part = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
            //var pr = part.GetComponent<ParticleSystemRenderer>();
            //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
            //paintColor = c;
        }

        void OnParticleCollision(GameObject other) {
            Debug.Log("Hit " + other.name);
            int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

            Paintable p = other.GetComponent<Paintable>();
            if(p != null){
                for  (int i = 0; i< numCollisionEvents; i++){
                    Vector3 pos = collisionEvents[i].intersection;
                    float radius = Random.Range(minRadius, maxRadius);
                    PaintManager.Instance.Paint(p, pos, radius, hardness, strength, paintColor);
                }
            }
        }
    }
}