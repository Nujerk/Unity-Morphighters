using UnityEngine;
using System.Collections;

namespace Morphighters
{
    public class Transformer : MonoBehaviour
    {
        public MorphShapeIndex MorphInto;

        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.layer == 8)
            {
                var player = col.gameObject.transform.parent.gameObject;
                player.GetComponent<Player>().MorphTo((int)MorphInto);
            }
        }
    }
}
