package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import kotlinx.coroutines.*
import com.github.lastexceed.cubeworldnetworking.utils.*

object Ultimates {
	fun cast(source: Player) {
		GlobalScope.launch {//todo: pls no
			repeat(10 + 1) {
				if (it > 0) delay(100)

				val particle = Particle(
					source.character.position,
					source.character.aimDisplacement,
					Vector3(1f, 0.5f, 0f),
					1f,
					1f,
					30,
					Particle.Type.Spark,
					10f,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(source.character.position),
					Sound.Type.FireHit
				)

				val serverUpdate = ServerUpdate(
					particles = listOf(particle),
					sounds = listOf(sound)
				)

				source.layer.broadcast(serverUpdate)
			}
		}
	}
}