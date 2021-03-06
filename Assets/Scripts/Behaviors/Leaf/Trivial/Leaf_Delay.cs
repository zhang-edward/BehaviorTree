using UnityEngine;

public class Leaf_Delay : Behavior {

	public int time;

	private string timerKey {
		get { return "timer: " + GetInstanceID(); }
	}

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(timerKey, time);

		int timer = (int) memory[timerKey];
		timer--;
		if (timer <= 0) {
			timer = time;
			memory[timerKey] = timer;
			return NodeStatus.Success;
		} 
		else {
			memory[timerKey] = timer;
			return NodeStatus.Running;
		}
	}
}