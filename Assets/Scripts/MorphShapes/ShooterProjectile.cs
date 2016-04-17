using UnityEngine;
using System.Collections;

namespace Morphighters.MorphShapes
{
    public class ShooterProjectile : MonoBehaviour
    {
        public float Damages { get; set; }
        public GameObject Owner { get; set; }

        void Update()
        {
            if (transform.position.x > 10 || transform.position.x < -10)
                Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if(col.gameObject.layer == 8 && col.gameObject != Owner)
            {
                Destroy(gameObject);
                col.gameObject.GetComponent<MorphShape>().ReceiveDamage(Damages);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == 8 && col.gameObject != Owner)
            {
                Destroy(gameObject);
                col.gameObject.GetComponent<MorphShape>().ReceiveDamage(Damages);
            }
        }
    }
}

