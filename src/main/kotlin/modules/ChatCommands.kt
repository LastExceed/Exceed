package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.delay

object ChatCommands {
	private const val prefix: Char = '/'
	suspend fun parse(message: String, source: Player): Boolean {
		if (!message.startsWith(prefix)) {
			return false
		}
		val params = message.trimStart(prefix).split(' ')

		when (params[0].lowercase()) {
			"time" -> {
				source.send(WorldClock(0, params[1].toInt()))
			}
			"skillpoint" -> {
				val pickup = Pickup(
					interactor = source.character.id,
					item = Item.void.copy(typeMajor = Item.Type.Major.ManaCube)
				)
				val miscellaneous = Miscellaneous(
					pickups = listOf(pickup, pickup, pickup, pickup)
				)
				source.send(miscellaneous)
			}
			"tp" -> {
				val serverEntity = CreatureUpdate(
					CreatureId(0),
					source.layer.creatures[CreatureId(params[1].toLong())]!!.position,
					affiliation = Affiliation.Pet,
					animation = Animation.Riding
				)
				source.send(serverEntity)
				delay(100)
				source.clearCreatures() //todo: deduplicate
				source.layer.creatures.forEach { source.send(it.value.toCreatureUpdate()) }//todo: what happens when you receive other creature updates in between?
			}
			"reload" -> {
				source.clearCreatures()
				source.layer.creatures.forEach { source.send(it.value.toCreatureUpdate()) }
			}
		}
		return true
	}
}