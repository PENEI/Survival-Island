using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CGizmo
{
    //오브젝트 주위로 원(평면)을 Gizmo로 그림
    /*
     pos = 기즈모 그릴 위치
    radius = 반지름
    _color = 기즈모 색상
    circleStep = 각 (원을 이루는 점의 갯수)
    ratioLastPt = ?
     */

    public static Vector3 DrawGizmosCircleXZ(Vector3 pos, float radius, Color _color, int circleStep = 20, float ratioLastPt = 1f)
    {
        Gizmos.color = _color;

        float theta, step = (2f * Mathf.PI) / (float)circleStep;
        Vector3 p0 = pos;
        Vector3 p1 = pos;
        for (int i = 0; i < circleStep; ++i)
        {
            theta = step * (float)i;
            p0.x = pos.x + radius * Mathf.Sin(theta);
            p0.z = pos.z + radius * Mathf.Cos(theta);

            theta = step * (float)(i + 1);
            p1.x = pos.x + radius * Mathf.Sin(theta);
            p1.z = pos.z + radius * Mathf.Cos(theta);
            Gizmos.DrawLine(p0, p1);
        }

        theta = step * ((float)circleStep * ratioLastPt);
        p0.x = pos.x + radius * Mathf.Sin(theta);
        p0.z = pos.z + radius * Mathf.Cos(theta);

        return p0;
    }
}
