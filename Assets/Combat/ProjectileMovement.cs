using UnityEngine;

namespace Combat
{
    public class ProjectileMovement : AttackMovement
    {       
        [SerializeField] private float speed;
        private Vector2 startPoint;
        private float timeSpent;
        private float totalTime;

        protected override void LaunchImpl()
        {
            startPoint = transform.position;
            timeSpent = 0;
            float distance = Vector2.Distance(startPoint, target.transform.position);
            totalTime = distance / speed;
        }

        // Update is called once per frame
        void Update()
        {
            if(!target)
            {
                GameObject.Destroy(gameObject);
                return;
            }
            timeSpent += Time.deltaTime;
            float progress = timeSpent / totalTime;
            if(progress >= 1)
            {
                attack.Hit(damageSource, target);
                GameObject.Destroy(gameObject);
                return;
            }
            Vector2 lastPos = transform.position;
            Vector2 newPos = Vector2.Lerp(startPoint, target.transform.position, progress);
            transform.position = newPos;
            Vector2 change = newPos - lastPos;
            if(change.magnitude > 0.00001f)
            {
                transform.right = newPos - lastPos;
            }
        }
    }
}
