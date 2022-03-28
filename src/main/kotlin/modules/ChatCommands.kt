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
				val seed = params[1].toInt()
				source.send(MapSeed(seed))
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
					Sound.Type.values()[params[1].toInt()]
				)
				source.send(
					ServerUpdate(
						sounds = listOf(sound)
					)
				)
			}
			"time" -> {
				source.send(DayTime(0, params[1].toInt()))
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
					Race.Turtle,
					500,
					1,
					Mission.State.InProgress,
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
					Sound.Type.Gate
				)
				val serverUpdate = ServerUpdate(
					missions = listOf(mission),
					sounds = listOf(sound)
				)
				source.send(serverUpdate)
			}
			"skillpoint" -> {
				val pickup = Pickup(
					source.character.id,
					Item(
						Item.Type.Major.ManaCube,
						Item.Type.Minor(0),
						0,
						0,
						Item.Type.Major.None,
						0,
						0,
						Item.Rarity.Normal,
						Item.Material.None,
						FlagSet(BooleanArray(8)),
						0,
						1,
						0,
						Array(32) {
							Item.Spirit(
								Vector3(0, 0, 0),
								Item.Material.None,
								1,
								0
							)
						},
						0
					)
				)
				val serverUpdate = ServerUpdate(
					pickups = listOf(pickup, pickup, pickup, pickup)
				)
				source.send(serverUpdate)
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
					type = Hit.Type.values()[params[1].toInt()],
					flash = true,
					paddingB = 0
				)

				source.send(ServerUpdate(hits = listOf(hit)))
			}
			"statuseffect" -> {
				val statusEffect = StatusEffect(
					CreatureID(0),
					source.character.id,
					StatusEffect.Type.values()[params[1].toInt()],
					0,
					0,
					5000f,
					5000,
					0,
					CreatureID(0)
				)

				source.layer.broadcast(ServerUpdate(statusEffects = listOf(statusEffect)))
			}
		}
		return true
	}
}