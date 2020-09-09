package modules

import packets.*
import Player
import utils.*

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
					id = CreatureID(99L),
					position = source.character.position,
					affiliation = Affiliation.NPC,
					race = Race.Santa,
					master = source.character.id,
					name = "test_NPC"


				)
				source.send(creatureUpdate)
			}
			"sound" -> {
				val serverUpdate = ServerUpdate()
				val sound = Sound(
					Utils.creatureToSoundPosition(source.character.position),
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
				println("spawning quest")
				val mission = Mission(
					Vector2(
						(source.character.position.x / Utils.SIZE_BIOME).toInt(),
						(source.character.position.y / Utils.SIZE_BIOME).toInt()
					),
					1,
					1,
					1,
					1,
					1,
					Race(1000),//Race.Turtle,
					500,
					1,
					MissionState.Ready,
					0x0101,
					100,
					100,
					Vector2(
						(source.character.position.x / Utils.SIZE_BIOME).toInt(),
						(source.character.position.y / Utils.SIZE_BIOME).toInt()
					)
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(source.character.position),
					SoundType.Gate
				)
				val su = ServerUpdate()
				su.missions.add(mission)
				su.sounds.add(sound)
				source.send(su)
			}
			"skillpoint" -> {
				val su = ServerUpdate()
				val pickup = Pickup(
					source.character.id,
					Item(
						ItemTypeMajor.ManaCube,
						ItemTypeMinor(0),
						0,
						0,
						ItemTypeMajor.None,
						0,
						0,
						Rarity.Normal,
						Material.None,
						FlagSet(BooleanArray(8)),
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
			"dmg" -> {
				val h = Hit(
					attacker = CreatureID(0),//CreatureID(source.character.id.value % 2 + 1),
					target = source.character.id,
					damage = 1f,
					critical = true,
					stuntime = 0,
					paddingA = 0,
					position = source.character.position,
					direction = Vector3(0f, 0f, 0f),
					isYellow = false,
					damageType = DamageType(params[1].toByte()),
					flash = true,
					paddingB = 0
				)

				val su = ServerUpdate()
				su.hits.add(h)
				source.send(su)
			}
			"ihp" -> {
				Utils.notify(source, "" + source.character.equipment.rightWeapon.hp)
			}
		}
		return true
	}
}