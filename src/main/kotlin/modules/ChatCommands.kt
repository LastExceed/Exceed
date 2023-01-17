package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import java.io.File
import kotlin.math.roundToInt

object ChatCommands {
	private const val prefix: Char = '/'
	private const val defaultPassword = "change-me"

	private val adminPassword = File("admin_pw").let {
		if (it.createNewFile()) {
			it.writeText(defaultPassword)
			defaultPassword
		} else it.readText()
	}

	val commands = mutableMapOf<String, Command>()

	init {
		commands["help"] = Command { caller, parameters ->
			{ caller.notify(commands.keys.joinToString { prefix + it }) }
		}
		commands["who"] = Command { caller, parameters ->
			val playerList = caller.layer.players.values.map {
				"#${it.character.id.value} ${it.character.name}"
			}.joinToString()

			({ caller.notify(playerList) })
		}
		commands["login"] = Command { caller, parameters ->
			val givenPassword = parameters.nextOrNull() ?: return@Command null

			if (caller.isAdmin) return@Command { caller.notify("already logged in") }

			if (givenPassword == adminPassword) {
				{
					caller.isAdmin = true
					caller.notify("login successful")
				}
			} else {
				{ caller.notify("wrong password") }
			}
		}

		commands["tp"] = Command(adminOnly = true) { caller, parameters ->
			val searchQuery = parameters.nextOrNull() ?: return@Command null
			val target = caller.layer.findPlayerByNameOrId(searchQuery)
				?: return@Command { caller.notify("couldn't find player $searchQuery") }

			{ ExperimentalStuff.teleport(caller, target.character.position) }
		}

		commands["tp"] = Command(adminOnly = true) { caller, parameters ->
			val searchQuery = parameters.nextOrNull() ?: return@Command null
			val target = caller.layer.findPlayerByNameOrId(searchQuery)
				?: return@Command { caller.notify("couldn't find player $searchQuery") }

			{ ExperimentalStuff.teleport(caller, target.character.position) }
		}

		commands["reload"] = Command { caller, parameters ->
			{
				caller.clearCreatures()
				caller.layer.creatures.forEach { caller.send(it.value.toCreatureUpdate()) }
			}
		}

		commands["level"] = Command { caller, parameters ->
			val targetLevel =
				(parameters.nextOrNull() ?: return@Command null)
					.toIntOrNull() ?: return@Command ({ caller.notify("invalid input") })
			if (targetLevel > 500) {
				return@Command ({ caller.notify("max level is 500") })
			}
			if (targetLevel <= caller.character.level) {
				return@Command ({ caller.notify("cannot downlevel") })
			}
			val requiredExperience = (caller.character.level until targetLevel).sumOf { Utils.computeMaxExperience(it) } - caller.character.experience

			val dummy = CreatureUpdate(
				CreatureId(9999),
				affiliation = Affiliation.Enemy
			)

			val kill = Kill(
				caller.character.id,
				dummy.id,
				xp = requiredExperience
			)

			val worldUpdate = WorldUpdate(kills = listOf(kill))

			({
				caller.send(dummy)
				caller.send(worldUpdate)
			})
		}

		commands["gear"] = Command { caller, parameters ->
			val lamp = Item.void.copy(
				typeMajor = Item.Type.Major.Lamp,
				rarity = Item.Rarity.Legendary,
				material = Item.Material.Iron,
				level = 1,
			)

			val glider = Item.void.copy(
				typeMajor = Item.Type.Major.Special,
				typeMinor = Item.Type.Minor.Special.HangGlider,
				material = Item.Material.Wood,
				level = 1,
			)

			val boat = glider.copy(typeMinor = Item.Type.Minor.Special.Boat)

			val (weaponTypes, weaponMaterial, armourMaterial) = when (caller.character.combatClassMajor) {
				CombatClassMajor.Warrior -> Triple(
					setOf(
						Item.Type.Minor.Weapon.Sword,
						Item.Type.Minor.Weapon.Sword,
						Item.Type.Minor.Weapon.Axe,
						Item.Type.Minor.Weapon.Axe,
						Item.Type.Minor.Weapon.Mace,
						Item.Type.Minor.Weapon.Mace,
						Item.Type.Minor.Weapon.Greatsword,
						Item.Type.Minor.Weapon.Greataxe,
						Item.Type.Minor.Weapon.Greatmace,
						Item.Type.Minor.Weapon.Shield
					),
					Item.Material.Iron,
					Item.Material.Iron
				)

				CombatClassMajor.Ranger -> Triple(
					setOf(
						Item.Type.Minor.Weapon.Bow,
						Item.Type.Minor.Weapon.Crossbow,
						Item.Type.Minor.Weapon.Boomerang
					),
					Item.Material.Wood,
					Item.Material.Linen
				)

				CombatClassMajor.Mage -> Triple(
					setOf(
						Item.Type.Minor.Weapon.Wand,
						Item.Type.Minor.Weapon.Staff,
						//bracelets
					),
					Item.Material.Wood,
					Item.Material.Silk
				)

				CombatClassMajor.Rogue -> Triple(
					setOf(
						Item.Type.Minor.Weapon.Longsword,
						Item.Type.Minor.Weapon.Dagger,
						Item.Type.Minor.Weapon.Dagger,
						Item.Type.Minor.Weapon.Fist,
						Item.Type.Minor.Weapon.Fist
					),
					Item.Material.Iron,
					Item.Material.Cotton
				)

				else -> Triple(
					setOf(),
					Item.Material.None,
					Item.Material.Iron
				)
			}

			val weapons = weaponTypes.map {
				Item.void.copy(
					typeMajor = Item.Type.Major.Weapon,
					typeMinor = it,
					rarity = Item.Rarity.Legendary,
					material = weaponMaterial,
					level = 500
				)
			}

			val weaponsExtra =
				if (caller.character.combatClassMajor == CombatClassMajor.Mage) {
					val goldBracelet = Item.void.copy(
						typeMajor = Item.Type.Major.Weapon,
						typeMinor = Item.Type.Minor.Weapon.Bracelet,
						rarity = Item.Rarity.Legendary,
						material = Item.Material.Gold,
						level = 500
					)

					setOf(
						goldBracelet,
						goldBracelet,
						goldBracelet.copy(material = Item.Material.Silver),
						goldBracelet.copy(material = Item.Material.Silver)
					)
				} else
					setOf()

			val armour = setOf(
				Item.Type.Major.Chest,
				Item.Type.Major.Gloves,
				Item.Type.Major.Boots,
				Item.Type.Major.Chest,
				Item.Type.Major.Shoulder
			).map {
				Item.void.copy(
					typeMajor = it,
					rarity = Item.Rarity.Legendary,
					material = armourMaterial,
					level = 500
				)
			}

			val accessoires = setOf(
				Item.Type.Major.Ring,
				Item.Type.Major.Ring,
				Item.Type.Major.Amulet
			).flatMap {
				val accessoire = Item.void.copy(
					typeMajor = it,
					rarity = Item.Rarity.Legendary,
					material = Item.Material.Gold,
					level = 500
				)

				setOf(
					accessoire,
					accessoire.copy(material = Item.Material.Silver)
				)
			}

			val potions = List(50) {
				Item.void.copy(
					typeMajor = Item.Type.Major.Food,
					typeMinor = Item.Type.Minor.Food.LifePotion,
					level = 500
				)
			}

			val money = Item.void.copy(
				Item.Type.Major.Coin,
				material = Item.Material.Gold,
				level = 1
			)

			val gear = listOf(
				lamp,
				glider,
				boat,
				money
			) + weapons + weaponsExtra + armour + accessoires + potions

			val worldUpdate = WorldUpdate(
				pickups = gear.map { Pickup(caller.character.id, it) }
			)

			({ caller.send(worldUpdate) })
		}

		commands["player"] = Command { caller, parameters ->
			val searchQuery = parameters.nextOrNull() ?: return@Command null
			val target = caller.layer.findPlayerByNameOrId(searchQuery)
				?: return@Command { caller.notify("couldn't find player $searchQuery") }

			val subclass = when (target.character.combatClassMajor) {
				CombatClassMajor.Warrior -> if (target.character.combatClassMinor == CombatClassMinor.Alternative)
					"Guardian"
				else
					"Berserker"
				CombatClassMajor.Ranger -> if (target.character.combatClassMinor == CombatClassMinor.Alternative)
					"Sniper"
				else
					"Scout"
				CombatClassMajor.Mage -> if (target.character.combatClassMinor == CombatClassMinor.Alternative)
					"Fire Mage"
				else
					"Water Mage"
				CombatClassMajor.Rogue -> if (target.character.combatClassMinor == CombatClassMinor.Alternative)
					"Assassin"
				else
					"Ninja"
				else -> "unknown"
			}

			val message = """
				---
				name: ${target.character.name} (#${target.character.id.value})
				class: $subclass (${target.character.combatClassMajor}, ${target.character.combatClassMinor})
				health: ${target.character.health.roundToInt()}/${target.character.healthMaximum.roundToInt()}
				mana: ${(target.character.mana * 100f).roundToInt()}/100 (${(target.character.manaCharge * 100).roundToInt()} charged)
				---
			""".trimIndent()

			({ caller.notify(message) })
		}

		commands["kick"] = Command(adminOnly = true) { caller, parameters ->
			val searchQuery = parameters.nextOrNull() ?: return@Command null
			val target = caller.layer.findPlayerByNameOrId(searchQuery)
			val reason = parameters.concatRemaining()

			if (target == null) {
				{ caller.notify("couldn't find player $searchQuery") }
			} else {
				{ target.kick(reason) }
			}
		}
		//registerCommands()
	}

	//todo: get rid of this suspend. its only here because model import code is garbage
	suspend fun parse(caller: Player, input: String): Boolean {
		if (!input.startsWith(prefix)) return false

		val splits = input.lowercase().drop(1).split(' ').listIterator()

		if (!splits.hasNext()) return false

		val command = commands[splits.next()] ?: kotlin.run {
			caller.notify("unknown command")
			return true
		}

		if (command.adminOnly && caller.isAdmin.not()) {
			caller.notify("no permission")
			return true
		}

		val action = command.evaluate(caller, splits) ?: kotlin.run {
			caller.notify("too few arguments")
			return true
		}

		if (splits.hasNext()) {
			caller.notify("too many arguments")
		} else {
			action()
		}

		return true
	}
}

data class Command(
	val adminOnly: Boolean = false,
	val evaluate: (Player, ListIterator<String>) -> (suspend () -> Unit)?
)

fun <T> Iterator<T>.nextOrNull() = if (hasNext()) next() else null
fun <T> Iterator<T>.concatRemaining() =
	buildList {
		while (hasNext()) {
			add(next().toString())
		}
	}.joinToString(" ")