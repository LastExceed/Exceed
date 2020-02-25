package exceed

import Modules.Pvp
import packets.*
import utils.Vector3

object GetRidOfThis {
	fun creatureToSoundPosition(creaturePosition: Vector3<Long>): Vector3<Float> {
		return Vector3(
			(creaturePosition.x / 0x10000).toFloat(),
			(creaturePosition.y / 0x10000).toFloat(),
			(creaturePosition.z / 0x10000).toFloat()
		)
	}

	fun notify(session: Player, message: String) {
		val chatMessage = ChatMessage(0, message)
		session.send(chatMessage)
	}

	fun computePower(level: Int): Int {
		val power = 101 - 100 / (0.05 * (level - 1) + 1)
		return power.toInt()
	}
}