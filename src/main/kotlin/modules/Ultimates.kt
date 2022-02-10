package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import kotlinx.coroutines.*
import com.github.lastexceed.cubeworldnetworking.utils.*

object Ultimates {
	fun cast(source: Player) {
		GlobalScope.launch { //todo: pls no
			fun createDragonFirePacket(): ServerUpdate {
				val character = source.character
				val particle = Particle(
					character.position,
					character.aimDisplacement,
					Vector3(1f, 0.5f, 0f),
					1f,
					1f,
					30,
					Particle.Type.Spark,
					10f,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(character.position),
					Sound.Type.FireHit
				)
				val su = ServerUpdate(
					particles = listOf(particle),
					sounds = listOf(sound)
				)
				return su
			}
			source.layer.broadcast(createDragonFirePacket())
			repeat(10) {
				delay(100)
				source.layer.broadcast(createDragonFirePacket())
			}
		}
	}
}