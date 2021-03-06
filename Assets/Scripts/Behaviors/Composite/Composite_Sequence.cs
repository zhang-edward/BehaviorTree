using UnityEngine;
public class Composite_Sequence : Behavior {

	public Behavior[] behaviors;

	public override void Init() {
		foreach (Behavior b in behaviors) {
			b.Init();
		}
	}

	public override string PrintTreeTraversal(System.Collections.Generic.Stack<int> stack, Entity entity) {
		int i = stack.Count == 0 ? 0 : stack.Pop();
		if (i < behaviors.Length)
			return $"{gameObject.name} (Sequence) \n{behaviors[i].PrintTreeTraversal(stack, entity)}";
		else
			return $"{gameObject.name} (Sequence) all succeeded";
	}

	/// <summary>
	/// performs behavior
	/// </summary>
	/// <returns>behavior return code</returns>
	protected override NodeStatus Act(Entity entity, Memory memory) {
		int i = entity.currentNodes.Count == 0 ? 0 : entity.currentNodes.Pop();
		// When we run through all the behaviors, i = behaviors.length is saved on the stack for 
		// infomation-preserving purposes.
		i %= behaviors.Length;

		NodeStatus status = NodeStatus.Failure;
		while (i < behaviors.Length) {
			// Run the current sub-behavior
			status = behaviors[i].ExecuteAction(entity, memory);
			//if (entity.debugBehavior)
			//	print($"{behaviors[i]}: {status.ToString()}");

			// Fails => continue to next one
			if (status == NodeStatus.Failure) {
				entity.currentNodes.Clear(); // Any downstream tree traversal is now wrong
				i = 0;
				break;
			}
			// Succeeds => break with status SUCCESS
			else if (status == NodeStatus.Success) {
				entity.currentNodes.Clear(); // Any downstream tree traversal is now wrong
				i++;
			}
			// Running => break with status RUNNING
			else if (status == NodeStatus.Running) {
				break;
			}
		}
		entity.currentNodes.Push(i);
		return status;
	}
}