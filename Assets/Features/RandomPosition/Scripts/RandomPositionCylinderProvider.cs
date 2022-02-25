using UnityEngine;

namespace Features.RandomPosition
{
    public class RandomPositionCylinderProvider : RandomPositionProvider
    {
        public float radius;

        public float height;

        public override Vector3 GetPosition()
        {
            var position = (Vector3) Random.insideUnitCircle * radius;

            if (height > 0)
            {
                position.y = Random.Range(0, height);
            }

            position += transform.position;

            return position;
        }

#if UNITY_EDITOR

        public Color gizmoColor = Color.white;

        public Mesh gizmoMesh;

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            var position = transform.position;
            position.y += height;

            var scale = new Vector3(radius, height, radius);

            Gizmos.DrawWireMesh(gizmoMesh, position, Quaternion.identity, scale);
        }

#endif
    }
}