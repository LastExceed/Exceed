package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.CreatureUpdate
import com.github.lastexceed.cubeworldnetworking.utils.*
import java.io.File
import kotlin.math.*

object Warps {
	val vanillaSpawn = 0x8020800000L to 0x8020800000L

	val locations = File("warps").let {
		if (it.createNewFile()) {
			val defaultContent = "vanilla-spawn;${vanillaSpawn.first};${vanillaSpawn.second}"
			it.writeText(defaultContent)
			defaultContent
		} else it.readText()
	}.lines()
		.map {
			it.split(";")
				.let {
					it[0] to (it[1].toLong() to it[2].toLong())
				}
		}.toMap()

	private val customSpawn = locations["spawn"]!!.let { (x,y) -> Vector3(x,y,0) }

	init {
		val warpCommand = Command { caller, parameters ->
			val destination = parameters.nextOrNull()
				?: return@Command ({ caller.notify("available warps: " + locations.keys.joinToString()) })

			val (x, y) = locations[destination]
				?: return@Command ({ caller.notify("unknown destination") })
			{
				ExperimentalStuff.teleport(
					caller,
					Vector3(x, y, 0)
				)
			}
		}
		ChatCommands.commands["warp"] = warpCommand
		ChatCommands.commands["spawn"] = Command { caller, parameters ->
			warpCommand.evaluate(caller, listOf("spawn").listIterator())
		}
	}

	suspend fun onJoin(player: Player) {
		ExperimentalStuff.teleport(
			player,
			customSpawn
		)
	}

	suspend fun onCreatureUpdate(source: Player, packet: CreatureUpdate) {
		val distanceFromSpawn = packet.position?.let {
			listOf(
				it.x - vanillaSpawn.first,
				it.y - vanillaSpawn.second
			).map { it.toDouble().pow(2) }
				.sum()
				.let(::sqrt)
		} ?: return

		if (distanceFromSpawn < 5000 * Utils.SIZE_BLOCK) {
			ExperimentalStuff.teleport(source, customSpawn)
		}
	}
}