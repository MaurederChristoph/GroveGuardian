using UnityEngine;

public class EnemyBehaviorTree : MonoBehaviour {
    private Root _root;
    [SerializeField] private Blackboard _blackboard;
    [SerializeField] private EnemyBrain _enemyBrain;
    [SerializeField] private int _damage;
    [SerializeField] private int _treeDamage;
    [SerializeField] private int _searchTime;

    private void Start() {
        _root = new(
            new SelectorNode(
                new SelectorNode(
                    new SequenceNode(
                        new SelectorNode(
                          new InverterNode(
                                new BlackboardCheck(_blackboard, "JustWasDamaged")
                            ),
                            new BlackboardCheck(_blackboard, "PlayerInRange")

                        ),
                        new BlackboardCheck(_blackboard, "CanSeePlayer"),
                        new SelectorNode(
                            new SequenceNode(
                                    new InverterNode(
                                            new BlackboardCheck(_blackboard, "PlayerInRange")
                                        ),
                                    new WalkToNode(_enemyBrain, Destination.Player)
                                ),
                            new SequenceNode(
                                    new AttackNode(_enemyBrain, _damage),
                                    new WalkToNode(_enemyBrain, Destination.Player)
                                )
                            )
                        ),
                        new SequenceNode(
                            new SelectorNode(
                                new BlackboardCheck(_blackboard, "JustSawPlayer"),
                                new BlackboardCheck(_blackboard, "JustWasDamaged")
                            ),
                            new SearchNode(_enemyBrain, _searchTime)
                        )
                ),
                new SelectorNode(
                    new SequenceNode(
                        new BlackboardCheck(_blackboard, "ProtectPoint"),
                        new WalkToNode(_enemyBrain, Destination.Mold)
                        ),
                    new SelectorNode(
                            new SequenceNode(
                                    new InverterNode(
                                            new BlackboardCheck(_blackboard, "IsOnPoint")
                                        ),
                                    new WalkToNode(_enemyBrain, Destination.Tree)
                                ),
                            new SequenceNode(
                                    new BlackboardCheck(_blackboard, "IsOnPoint"),
                                    new AttackNode(_enemyBrain, _treeDamage)
                                )
                        )
                    )
            )
            );
        _root.Enter();
    }

    public void InterruptTree() {
        _root.Interrupt();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.F)) {
            InterruptTree();
        }
    }
}
