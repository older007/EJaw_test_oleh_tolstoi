using UnityEngine;

namespace Core
{
    public class EnemyMove : Mover
    {
        protected override void Move()
        {
            var direction = EndPos - transform.position;
            var offset = speed * Time.deltaTime * direction.normalized;
            
            transform.position += offset;
        }
    }
}