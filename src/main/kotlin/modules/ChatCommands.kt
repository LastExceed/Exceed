package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import kotlinx.coroutines.delay
import java.io.File

object ChatCommands {
	private const val prefix: Char = '/'
	private val adminPassword = File("admin_pw").let {
		if (it.createNewFile()) {
			it.writeText("change-me")
		}
		it.readText()
	}

	suspend fun parse(message: String, source: Player): Boolean {
		if (!message.startsWith(prefix)) {
			return false
		}
		val params = message.trimStart(prefix).split(' ')

		if (params[0].lowercase() == "login" && params.size == 2) {
			if (params[1] == adminPassword) {
				source.isAdmin = true
				source.notify("logged in")
			} else {
				source.notify("wrong password")
			}
			return true
		}
		if (!source.isAdmin) {
			source.notify("only admins can use commands for now")
			return true
		}

		when (params[0].lowercase()) {
			"time" -> {
				source.send(GameDateTime(0, params[1].toInt()))
			}
			"skillpoint" -> {
				val pickup = Pickup(
					interactor = source.character.id,
					item = Item.void.copy(typeMajor = Item.Type.Major.ManaCube)
				)
				val miscellaneous = WorldUpdate(
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
			"day" -> {
				source.layer.broadcast(GameDateTime(0, 20_000_000))
			}
			"speed" -> {
				val camo = StatusEffect(
					source.character.id,
					source.character.id,
					StatusEffect.Type.Camouflage,
					modifier = 99999f,
					duration = 10_000,
					creatureId3 = source.character.id
				)
				val speed = StatusEffect(
					source.character.id,
					source.character.id,
					StatusEffect.Type.Swiftness,
					modifier = 99999f,
					duration = 10_000,
					creatureId3 = source.character.id
				)
				source.send(WorldUpdate(statusEffects = listOf(speed, camo)))
			}
		}
		return true
	}
}