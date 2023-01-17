package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.CreatureUpdate
import com.github.lastexceed.cubeworldnetworking.utils.*
import java.io.File
import kotlin.math.*

object Warps {
	val locations = File("warps").let {
		if (it.createNewFile()) {
			""
		} else it.readText()
	}.lines()
		.map { it.split(";") }

	val trueSpawn = Vector3(
		0x8020800000L,
		0x8020800000L,
		0L
	)
	val customSpawn = Vector3(
		549310656995L,
		551632558116L,
		0L
	)

	init {
		ChatCommands.commands["spawn"] = Command { caller, parameters ->
			{
				ExperimentalStuff.teleport(
					caller,
					customSpawn
				)
			}
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
				it.x - trueSpawn.x,
				it.y - trueSpawn.y,
				it.z - trueSpawn.z,
			).map { it.toDouble().pow(2) }.sum().let { sqrt(it) }
		} ?: return

		if (distanceFromSpawn < 1000 * Utils.SIZE_BLOCK) {
			ExperimentalStuff.teleport(source, customSpawn)
		}
	}
}