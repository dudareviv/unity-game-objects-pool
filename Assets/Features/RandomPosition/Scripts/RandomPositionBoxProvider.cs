using UnityEngine;

namespace Features.RandomPosition
{
    public class RandomPositionBoxProvider : RandomPositionProvider
    {
        public Vector3 bounds;

        public override Vector3 GetPosition()
        {
            var position = new Vector3()
            {
                x = bounds.x > 0f ? Random.Range(-bounds.x / 2f, bounds.x / 2f) : 0f,
                y = bounds.y > 0f ? Random.Range(-bounds.y / 2f, bounds.y / 2f) : 0f,
                z = bounds.z > 0f ? Random.Range(-bounds.z / 2f, bounds.z / 2f) : 0f,
            };

            position += transform.position;

            return position;
        }

#if UNITY_EDITOR

        public Color gizmoColor = Color.white;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            if (bounds == Vector3.zero)
            {
                Gizmos.DrawSphere(transform.position, 0.1f);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, bounds);
            }
        }

#endif
    }
}