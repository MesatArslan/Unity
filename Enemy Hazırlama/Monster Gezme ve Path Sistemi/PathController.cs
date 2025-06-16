using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Controller
{
    public class PathController : MonoBehaviour
        {
            const float wayPointRadius = 0.4f;  //* const kullandıktan sonra bu kod başka bir yerd edeğiştirilemiyor

            private void OnDrawGizmos()
            {
                for(int i = 0; i<transform.childCount;i++)
                {
                    int j = GetNextIndex(i);
                    // transform.GetChild(0);    //* burada patrolpath'in childlarına ulaşıyoruz
                    Gizmos.DrawSphere(GetWayPointPosition(i), wayPointRadius);
                    Gizmos.DrawLine(GetWayPointPosition(i), GetWayPointPosition(j));

                }


            }

            public int GetNextIndex(int i)
            {
                if(i + 1 == transform.childCount)
                {
                    return 0;
                }

                return i + 1;
            }

            public Vector3 GetWayPointPosition(int i)      //* Vector3 == konum demek
            {
                return transform.GetChild(i).position;
            }
        }
}