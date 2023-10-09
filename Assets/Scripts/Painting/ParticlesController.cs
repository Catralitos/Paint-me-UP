// Code adapted from https://github.com/mixandjam/Splatoon-Ink
// Comments mine to help me understand/modify code

using System.Collections.Generic;
using UnityEngine;

namespace Painting
{
    public class ParticlesController: MonoBehaviour{
    
        public float minRadius = 0.001f;
        public float maxRadius = 0.005f;
        public float strength = 0.1f;
        public float hardness = 0.1f;
        [Space]
        private ParticleSystem _part;
        private List<ParticleCollisionEvent> _collisionEvents;

        private void Start(){
            _part = GetComponent<ParticleSystem>();
            _collisionEvents = new List<ParticleCollisionEvent>();
        }

        private void OnParticleCollision(GameObject other) {
            int numCollisionEvents = _part.GetCollisionEvents(other, _collisionEvents);

            Paintable p = other.GetComponent<Paintable>();
            if (p != null){         
                for (int i = 0; i< numCollisionEvents; i++){
                    Debug.Log("Colidiu");
                    Vector3 pos = _collisionEvents[i].intersection;
                    float radius = Random.Range(minRadius, maxRadius);
                    PaintManager.Instance.Paint(p, pos, radius, hardness, strength, SceneManager.Instance.currentColor);
                }
            }
        }
    }
}