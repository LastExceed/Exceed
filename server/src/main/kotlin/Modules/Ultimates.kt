package Modules

import kotlinx.coroutines.*
import packets.*
import utils.Vector3
import exceed.*

object Ultimates {
	fun cast(source: Player) {
		GlobalScope.launch {
			fun createDragonFirePacket(): ServerUpdate {
				val character = source.character
				val particle = Particle(
					character.position,
					character.rayHit,
					Vector3(1f, 0.5f, 0f),
					1f,
					1f,
					30,
					ParticleType.Spark,
					10f,
					0
				)
				val sound = Sound(
					GetRidOfThis.creatureToSoundPosition(character.position),
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