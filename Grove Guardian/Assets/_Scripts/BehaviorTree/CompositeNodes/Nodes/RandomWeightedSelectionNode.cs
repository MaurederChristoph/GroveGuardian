using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
///  Adds up all entries and then proportionally assigns the percentages
/// </summary>
public class RandomWeightedSelectionNode : CompositeNode {
    private readonly List<(Node, float)> _newChildren = new();
    private readonly float _allEntries;
    private Node _child;

    public RandomWeightedSelectionNode(params (Node node, float percentage)[] children) {
        foreach(var (_, percentage) in children) {
            _allEntries += percentage;
        }
        for(var i = 0;i < children.Length;i++) {
            (Node node, var percentage) = children[i];
            var toAdd = (percentage / _allEntries * 100) + (i == 0 ? 0 : children[i - 1].percentage);
            _newChildren.Add((node, toAdd));
        }
    }

    public override void Enter() {
        base.Enter();
        var number = GetRandomNumber();
        var sortedList = _newChildren.OrderBy(c => c.Item2).ToList();
        for(var i = 0;i < sortedList.Count();i++) {
            if(sortedList[i].Item2 > number) {
                _child = sortedList[i - 1].Item1;
            }
        }
    }

    public override void Execute() {
        _child.Execute();
    }

    protected override void ChildFinish(ReturnState state) {
        OnFinish.Invoke(state);
        Exit();
    }

    public float GetRandomNumber() {
        System.Random random = new();
        var num = (float)random.NextDouble();
        var num2 = random.Next(0, (int)Math.Floor(_allEntries));
        return num2 + num > _allEntries ? _allEntries - 0.1f : num2 + num;
    }
}
