using System;
using UnityEngine;
using System.Collections;

namespace EasyDestuctibleWall {
    public class DestructionManager : MonoBehaviour {
        public PlayerController player1;
        public PlayerController player2;
        // The hitpoints of the object, when this value is below 1, the chunk will fracture
        [SerializeField]
        private float health = 100f;

        // These two variables are used to multiply damage based on velocity and torque respectively.
        [SerializeField]
        private float impactMultiplier = 2.25f;
        [SerializeField]
        private float twistMultiplier = 0.0025f;

        private Rigidbody cachedRigidbody;

        private void Awake() {
            cachedRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if(health <= 0f) { 
                foreach(Transform child in transform) { 
                    Rigidbody spawnRB = child.gameObject.AddComponent<Rigidbody>(); child.parent = null; // Transfer velocity
                    spawnRB.linearVelocity = GetComponent<Rigidbody>().GetPointVelocity(child.position); // Transfer torque
                    spawnRB.AddTorque(GetComponent<Rigidbody>().angularVelocity, ForceMode.VelocityChange); 
                } 
                Destroy(gameObject); // Destroy this now empty chunk object
            }        
        }

        void OnCollisionEnter(Collision collision) {
            var pc = collision.collider.GetComponent<PlayerController>();
            if (pc == null)
                return; // ไม่ใช่ player → ignore

            var cm = CharacterManager.Instance;

            bool sameLane = cm.LaneP1 == cm.LaneP2;

            if (!sameLane)
            {
                Debug.Log("DestructionManager: not same land → Game Over");
                GameManager.Instance.Fail();
                return;
            }

            // *** case ผ่านเงื่อนไข lane → ให้แตกตามระบบเดิม ***

            if (collision.rigidbody)
            {
                health -=  impactMultiplier * collision.rigidbody.mass;
            }
            else
                health -= impactMultiplier;
        }
    }
}