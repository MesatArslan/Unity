using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Controller
{
    public class MonsterAI : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float wayPointWaitTime = 3f;
        [SerializeField] PathController patrolPath;
        [SerializeField] float wayPointTolerans = 1f;

        //* devriye atarkenki hızı
        [Range(0, 10)]
        [SerializeField] float patrolSpeedFraction = 5f;
        public NavMeshAgent agent;
        public Animator animator;
        GameObject player;

        Vector3 enemyLocation;
        float timeSinceArrivedWayPoint;
        float timeSinceLastSawPlayer;

        int currentWayPointIndex = 0;

        void Start()
        {

            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player");
            enemyLocation = transform.position;
        }

        void Update()
        {
            if (DistanceToPlayer() < chaseDistance)
            {
                ChasePlayer();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void ChasePlayer()
        {
            timeSinceLastSawPlayer = 0;
            animator.SetBool("isWalking", true);
            agent.SetDestination(player.transform.position);


            
        }

        private void SuspicionBehaviour()
        {
            animator.SetBool("isWalking", false);
            agent.SetDestination(transform.position); // Canavarın durmasını sağlar
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = enemyLocation;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    animator.SetBool("isWalking", false);
                    timeSinceArrivedWayPoint = 0;
                    CycleWayPoint();
                }
                nextPosition = GetNextWayPoint();
            }

            if (timeSinceArrivedWayPoint > wayPointWaitTime)
            {
                animator.SetBool("isWalking", true);
                agent.SetDestination(nextPosition);
                agent.speed = patrolSpeedFraction; // Devriye hızını ayarlayın
            }
        }

        private Vector3 GetNextWayPoint()
        {
            return patrolPath.GetWayPointPosition(currentWayPointIndex);
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetNextWayPoint());
            return distanceToWayPoint < wayPointTolerans;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedWayPoint += Time.deltaTime;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}


//* Monster'ın seni düzgün takip etmesi navmeshagent