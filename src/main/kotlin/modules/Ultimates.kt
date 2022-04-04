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
					position = source.character.position,
					velocity = source.character.aimDisplacement,
					color = Vector3(1f, 0.5f, 0f),
					alpha = 1f,
					size = 1f,
					count = 30,
					type = Particle.Type.Spark,
					spread = 10f
				)
				val sound = Sound(
					position = Utils.creatureToSoundPosition(source.character.position),
					type = Sound.Type.FireHit
				)

				val miscellaneous = Miscellaneous(
					particles = listOf(particle),
					sounds = listOf(sound)
				)

				source.layer.broadcast(miscellaneous)
			}
		}
	}
}