package exceed

import packets.*
import utils.Vector3
import kotlin.math.*

object GetRidOfThis {
	suspend fun notify(session: Player, message: String) {
		val chatMessage = ChatMessage(CreatureID(0), message)
		session.send(chatMessage)
	}
}