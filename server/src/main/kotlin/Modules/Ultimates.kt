package Modules

import kotlinx.coroutines.*
import packets.*
import utils.Vector3
import exceed.*
import packets.utils.*

object Ultimates {
	fun cast(source: Player) {
		GlobalScope.launch {
			fun createDragonFirePacket(): ServerUpdate {
				val character = source.character
				val particle = Particle(
					character.position,
					character.aimDisplacement,
					Vector3(1f, 0.5f, 0f),
					1f,
					1f,
					30,
					ParticleType.Spark,
					10f,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(character.position),
					SoundType.FireHit
				)
				val su = ServerUpdate()
				su.particles.add(particle)
				su.sounds.add(sound)
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