package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.delay

object ExperimentalStuff {
	suspend fun teleport(player: Player, destination: Vector3<Long>) {
		val serverEntity = CreatureUpdate(
			CreatureId(0),
			destination,
			affiliation = Affiliation.Pet,
			animation = Animation.Riding
		)
		player.send(serverEntity)
		delay(100)
		player.clearCreatures() //todo: deduplicate
		player.layer.creatures.forEach { player.send(it.value.toCreatureUpdate()) }//todo: what happens when you receive other creature updates in between?
	}
}

fun registerCommands() {
	ChatCommands.commands["skillpoint"] = Command { caller, parameters ->
		val pickup = Pickup(
			interactor = caller.character.id,
			item = Item.void.copy(typeMajor = Item.Type.Major.ManaCube)
		)
		val miscellaneous = WorldUpdate(pickups = listOf(pickup, pickup, pickup, pickup))

		({ caller.send(miscellaneous) })
	}

	ChatCommands.commands["reload"] = Command { caller, parameters ->
		{
			caller.clearCreatures()
			caller.layer.creatures.forEach { caller.send(it.value.toCreatureUpdate()) }
		}
	}

	ChatCommands.commands["speed"] = Command { caller, parameters ->
		val camo = StatusEffect(
			caller.character.id,
			caller.character.id,
			StatusEffect.Type.Camouflage,
			modifier = 99999f,
			duration = 10_000,
			creatureId3 = caller.character.id
		)
		val speed = StatusEffect(
			caller.character.id,
			caller.character.id,
			StatusEffect.Type.Swiftness,
			modifier = 99999f,
			duration = 10_000,
			creatureId3 = caller.character.id
		)

		({ caller.send(WorldUpdate(statusEffects = listOf(speed, camo))) })
	}

	ChatCommands.commands["block"] = Command { caller, parameters ->
		val we = WorldEdit(
			position = Vector3(
				(caller.character.position.x / Utils.SIZE_BLOCK).toInt(),
				(caller.character.position.y / Utils.SIZE_BLOCK).toInt(),
				(caller.character.position.z / Utils.SIZE_BLOCK).toInt()
			),
			color = Vector3(-1, 0, -1),
			blockType = WorldEdit.BlockType.Solid
		)

		({ caller.send(WorldUpdate(worldEdits = listOf(we))) })
	}

	ChatCommands.commands["oj"] = Command { caller, parameters ->
		{ ModelImport.onJoin(caller) }
	}

	ChatCommands.commands["level"] = Command { caller, parameters ->
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

	ChatCommands.commands["gear"] = Command { caller, parameters ->
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
				level = 60
			)
		}

		val weaponsExtra =
			if (caller.character.combatClassMajor == CombatClassMajor.Mage) {
				val goldBracelet = Item.void.copy(
					typeMajor = Item.Type.Major.Weapon,
					typeMinor = Item.Type.Minor.Weapon.Bracelet,
					rarity = Item.Rarity.Legendary,
					material = Item.Material.Gold,
					level = 60
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
}