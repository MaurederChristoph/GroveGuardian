using System.Collections.Generic;
using System.Security.Cryptography;

public class RandomSelectorNode : CompositeNode {

    public RandomSelectorNode(params Node[] children) {
        _children = new List<Node>(children);
    }

    public override void Enter() {
        base.Enter();
        Shuffle(_children);
        _children[0].Enter();
    }

    public override void Execute() {
        _children[0].Execute();
    }


    protected override void ChildFinish(ReturnState state) {
        OnFinish.Invoke(state);
        Exit();
    }

    public void Shuffle(List<Node> list) {
        RNGCryptoServiceProvider provider = new();
        var n = list.Count;
        while(n > 1) {
            var box = new byte[1];
            do {
                provider.GetBytes(box);
            } while(!(box[0] < n * (byte.MaxValue / n)));
            var k = box[0] % n;
            n--;
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
