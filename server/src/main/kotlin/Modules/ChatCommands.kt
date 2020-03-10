package Modules

import packets.*
import utils.Vector3
import exceed.*
import utils.Vector2

object ChatCommands {
	private const val prefix: Char = '/'
	suspend fun parse(message: String, source: Player): Boolean {
		if (!message.startsWith(prefix)) {
			return false
		}
		val params = message.trimStart(prefix).split(' ')

		when (params[0].toLowerCase()) {
			"seed" -> {
				if (params.size != 1) {
					//invalid syntax
				}
				val seed = params[1].toIntOrNull()
				if (seed == null) {
					//invalid seed
				}
				val mapSeed = MapSeed(seed!!)
				source.send(mapSeed)
			}
			"npc" -> {
				val creatureUpdate = CreatureUpdate(
					id = CreatureID(2L),
					position = Vector3(234234, 234234, 0xaa0000),
					affiliation = Affiliation(params[1].toByte()),
					race = Race.Turtle,
					master = source.character.id,
					name = "test_NPC"


				)
				source.send(creatureUpdate)
			}
			"sound" -> {
				val serverUpdate = ServerUpdate()
				val sound = Sound(
					GetRidOfThis.creatureToSoundPosition(source.character.position),
					SoundType(params[1].toInt())
				)
				serverUpdate.sounds.add(sound)
				source.send(serverUpdate)
			}
			"time" -> {
				val time = DayTime(0, params[1].toInt())
				source.send(time)
			}
			"q" -> {
				val mission = Mission(
					Vector2(0, 0),
					0,
					0,
					0,
					1,
					1,
					Race.Turtle,
					1,
					0,
					MissionState.InProgress,
					0,
					4,
					10,
					Vector2(
						(source.character.position.x / 0x1000000).toInt(),
						(source.character.position.y / 0x1000000).toInt()
					)
				)
				val su = ServerUpdate()
				su.missions.add(mission)
				source.send(su)
			}
			"skillpoint" -> {
				val su = ServerUpdate()
				val pickup = Pickup(
					source.character.id,
					Item(
						ItemMainType.ManaCube,
						1,
						0,
						0,
						ItemMainType.None,
						Rarity.Normal,
						Material.None,
						false,
						0,
						1,
						0,
						Array<Spirit>(32) {
							Spirit(
								Vector3(0, 0, 0),
								Material.None,
								1,
								0
							)
						},
						0
					)
				)
				su.pickups.add(pickup)
				su.pickups.add(pickup)
				su.pickups.add(pickup)
				su.pickups.add(pickup)
				source.send(su)
			}
		}
		return true
	}
}