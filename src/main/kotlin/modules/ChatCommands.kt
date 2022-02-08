package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import com.github.lastexceed.cubeworldnetworking.utils.*

object ChatCommands {
	private const val prefix: Char = '/'
	suspend fun parse(message: String, source: Player): Boolean {
		if (!message.startsWith(prefix)) {
			return false
		}
		val params = message.trimStart(prefix).split(' ')

		when (params[0].lowercase()) {
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
				val sound = Sound(
					Utils.creatureToSoundPosition(source.character.position),
					SoundType(params[1].toInt())
				)
				source.send(
					ServerUpdate(
						sounds = listOf(sound)
					)
				)
			}
			"time" -> {
				val time = DayTime(0, params[1].toInt())
				source.send(time)
			}
			"q" -> {
				println("spawning quest")
				val mission = Mission(
					Vector2(
						(source.character.position.x / Utils.SIZE_ZONE).toInt() / 8 + params[1].toInt(),
						(source.character.position.y / Utils.SIZE_ZONE).toInt() / 8 + params[2].toInt()
					),
					1,
					1,
					1,
					1,
					1,
					Race(1000),//Race.Turtle,
					500,
					1,
					MissionState.InProgress,
					0x0101,
					100,
					100,
					Vector2(
						(source.character.position.x / Utils.SIZE_ZONE).toInt(),
						(source.character.position.y / Utils.SIZE_ZONE).toInt()
					)
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(source.character.position),
					SoundType.Gate
				)
				val su = ServerUpdate(
					missions = listOf(mission),
					sounds = listOf(sound)
				)
				source.send(su)
			}
			"skillpoint" -> {
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
						Array(32) {
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
				val su = ServerUpdate(
					pickups = listOf(pickup, pickup, pickup, pickup)
				)
				source.send(su)
			}
			"dmg" -> {
				val hit = Hit(
					attacker = CreatureID(source.character.id.value % 2 + 1),
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

				val su = ServerUpdate(
					hits = listOf(hit)
				)
				source.send(su)
			}
			"statuseffect" -> {
				val statusEffect = StatusEffect(
					CreatureID(0),
					source.character.id,
					StatusEffect.Type(params[1].toInt()),
					5000f,
					5000,
					0,
					CreatureID(0)
				)

				val su = ServerUpdate(
					statusEffects = listOf(statusEffect)
				)
				source.layer.broadcast(su)
			}
		}
		return true
	}
}