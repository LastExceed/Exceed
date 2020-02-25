package Modules

import exceed.GetRidOfThis.computePower
import exceed.Player
import packets.CreatureUpdate
import packets.Equipment
import packets.Item
import packets.Rarity

object AntiCheat {
	private const val LEVEL_CAP = 500
	fun inspectCreatureUpdate(creatureUpdate: CreatureUpdate, source: Player): Boolean {
		val previous = source.character
		with(creatureUpdate) {
			//ID

			if (MP != null && MP!! > 1f) {
				trigger("MP hack")
				return true
			}
			if (level != null && level!! > LEVEL_CAP) {
				trigger("character level too high")
			}
			if (equipment != null) {
				checkEQ(equipment!!, source)
			}
		}
		return false
	}

	private fun checkEQ(equipment: Equipment, source: Player) {
		fun Item.inspect(): Boolean {
			if (this.rarity.value > Rarity.Legendary.value) {
				trigger("mythic items are banned")
				return true
			}
			if (computePower(this.level.toInt()) > computePower(source.character.level)) {
				trigger("not enough power to wear this item")
				return true
			}
			return false
		}
		//maintype/subtype: based on slot
		//material: based on slot and class
	}

	private fun trigger(reason: String) {

	}
}