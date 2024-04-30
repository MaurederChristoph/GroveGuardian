using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class EnemyManager : Singleton<EnemyManager> {
    private int _enemyCount;
    public void AddEnemyCount() {
        _enemyCount++;
    }
    public void RemoveEnemyCount(GameObject _) {
        _enemyCount--;
        Debug.Log(_enemyCount);
        Debug.Log(GameManager.Instance.State);
        if (_enemyCount == 0 && GameManager.Instance.State == GameState.Combat) {
            GameManager.Instance.ChangeState(GameState.DownTime);
        }
    }

    public Vector3 GetRandomPointInSphere(Point point) {
        var _points = point.PointBorder;
        var rnd = new System.Random();
        Transform firstPoint, SecondPoint;
        var index = rnd.Next(_points.Count);
        firstPoint = _points[index];
        List<Transform> newPoints = new(_points) {
            [index] = null
        };
        newPoints[(index + 1) % newPoints.Count] = null;
        newPoints[(index - 1 + newPoints.Count) % newPoints.Count] = null;
        newPoints = newPoints.Where(p => p != null).ToList();
        SecondPoint = newPoints[rnd.Next(newPoints.Count)];

        return firstPoint.position + Utils.MultiplyVectorWithPoint(SecondPoint.position - firstPoint.position, rnd.NextDouble());
    }
}
