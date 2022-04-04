package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.delay

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
					id = CreatureId(99L),
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
				source.send(Miscellaneous(sounds = listOf(sound)))
			}
			"time" -> {
				source.send(WorldClock(0, params[1].toInt()))
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
					position = Utils.creatureToSoundPosition(source.character.position),
					type = Sound.Type.Gate
				)
				val miscellaneous = Miscellaneous(
					missions = listOf(mission),
					sounds = listOf(sound)
				)
				source.send(miscellaneous)
			}
			"skillpoint" -> {
				val pickup = Pickup(
					source.character.id,
					Item(
						typeMajor = Item.Type.Major.ManaCube,
						typeMinor = Item.Type.Minor(0),
						randomSeed = 0,
						recipe = Item.Type.Major.None,
						rarity = Item.Rarity.Normal,
						material = Item.Material.None,
						flags = FlagSet(BooleanArray(8)),
						level = 0,
						spirits = Array(32) {
							Item.Spirit(
								position = Vector3(0, 0, 0),
								material = Item.Material.None,
								level = 1
							)
						},
						spiritCounter = 0
					)
				)
				val miscellaneous = Miscellaneous(
					pickups = listOf(pickup, pickup, pickup, pickup)
				)
				source.send(miscellaneous)
			}
			"dmg" -> {
				val hit = Hit(
					attacker = CreatureId(source.character.id.value % 2 + 1),
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

				source.send(Miscellaneous(hits = listOf(hit)))
			}
			"statuseffect" -> {
				val statusEffect = StatusEffect(
					source = CreatureId(0),
					target = source.character.id,
					type = StatusEffect.Type.values()[params[1].toInt()],
					paddingA = params[1].toByte(),
					modifier = 5000f,
					duration = 5000,
					creatureId3 = CreatureId(0)
				)

				source.layer.broadcast(Miscellaneous(statusEffects = listOf(statusEffect)))
			}
			"heal" -> {
				val hit = Hit(
					attacker = CreatureId(0),
					target = source.character.id,
					damage = -5000f,
					critical = false,
					stuntime = 0,
					position = source.character.position,
					direction = Vector3(0f, 0f, 0f),
					isYellow = false,
					type = Hit.Type.Default,
					flash = true,
				)

				source.send(Miscellaneous(hits = listOf(hit)))
			}
			"tp" -> {
				val serverEntity = CreatureUpdate(
					CreatureId(0),
					source.layer.creatures[CreatureId(params[1].toLong())]!!.position,
					affiliation = Affiliation.Pet,
					animation = Animation.Riding
				)
				source.send(serverEntity)
				delay(100)
				source.clearCreatures() //todo: deduplicate
				source.layer.creatures.forEach { source.send(it.value.toCreatureUpdate()) }//todo: what happens when you receive other creature updates in between?
			}
			"reload" -> {
				source.clearCreatures()
				source.layer.creatures.forEach { source.send(it.value.toCreatureUpdate()) }
			}
			"particle" -> {
				val particle = Particle(
					position = source.character.position,
					velocity = Vector3(0f, 0f, 1f),
					color = Vector3(0f, 0f, 1f),
					alpha = 1f,
					size = 1f,
					count = 50,
					type = Particle.Type.NoGravity,
					paddingA = params[1].toByte(),
					spread = 1f
				)

				source.send(Miscellaneous(particles = listOf(particle)))
			}

			"projectile" -> {
				val projectile = Projectile(
					attacker = CreatureId(99L),
					chunk = Vector2(-1, -1),
					position = source.character.position.copy(z = source.character.position.z + 0x40000L),
					velocity = Vector3(15f, 15f, 15f),
					legacyDamage = 0f,
					scale = 5f,
					mana = 1f,
					particles = 1f,
					skill = 0,
					type = Projectile.Type.Boulder,
					paddingD = params[1].toByte()
				)

				source.send(Miscellaneous(projectiles = listOf(projectile)))
			}

			"worldobjects" -> {
				val thelist = WorldObject.Type.values.mapIndexed { index, it ->
					WorldObject(
						chunk = source.character.zone,
						objectID = it.ordinal,
						type = it,
						position = source.character.position.copy(
							x = source.character.position.x + index % 9 * Utils.SIZE_BLOCK * 2,
							y = source.character.position.y + index / 9 * Utils.SIZE_BLOCK * 2
						),
						orientation = WorldObject.Orientation.South,
						paddingE = 1,
						size = Vector3(1f, 1f, 1f),
						isClosed = true,
						transformTime = 1000,
						interactor = source.character.id.value
					)
				}

				source.send(Miscellaneous(worldObjects = thelist))
			}
		}
		return true
	}
}