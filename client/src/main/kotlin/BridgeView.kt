import javafx.beans.property.SimpleBooleanProperty
import javafx.beans.property.SimpleIntegerProperty
import javafx.beans.property.SimpleListProperty
import tornadofx.*

class BridgeView : View("My View") {
	private val viewModel: BridgeViewModel by inject()
	private val adaptedChecked = SimpleBooleanProperty(false)
	private val seed = SimpleIntegerProperty(42)
	private val itemTypes = observableListOf(
		"None",
		"Food",
		"Formula",
		"Weapon",
		"Chest Armor",
		"Gloves",
		"Boots",
		"Shoulder Armor",
		"Amulet",
		"Ring",
		"Block",
		"Resource",
		"Coin",
		"Platinum",
		"Leftover",
		"Beak",
		"Painting",
		"Vase/Pot",
		"Candle",
		"Pet",
		"Pet Food",
		"Quest Item",
		"Unknown",
		"Special",
		"Lamp",
		"Mana Cube"
	)

	override val root = hbox {
		listview<String> {
			this.prefWidth = 100.0
			this.items.addAll("using",
				"unknown",
				"neck",
				"chest",
				"feet",
				"hands",
				"shoulder",
				"Lweapon",
				"Rweapon",
				"leftRing",
				"rightRing",
				"lamp",
				"special",
				"pet")
		}
		form {
			fieldset {
				field("Item Type") {
					combobox<String> {
						items = itemTypes
					}
					label("->")
					combobox<String> {
						items = itemTypes
						enableWhen(adaptedChecked)

						setOnAction {
							seed.value = items.indexOf(selectedItem)
						}
					}
				}
				field("Sub Type") {
					combobox<String> {  }
				}
				field("Material") {
					combobox<String> {
						items.addAll("None",
							"Iron",
							"Wood",
							" ",
							" ",
							"Obsidian",
							" ",
							"Bone",
							" ",
							" ",
							" ",
							"Gold",
							"Silver",
							"Emerald",
							"Sapphire",
							"Ruby",
							"Diamond",
							"Sandstone",
							"Saurian",
							"Parrot",
							"Mammoth",
							"Plant",
							"Ice",
							"Licht",
							"Glass",
							"Silk",
							"Linen",
							"Cotton",
							" ",
							"Fire",
							"Unholy",
							"Ice",
							"Wind")
					}
				}
				field("Rarity") {
					combobox<String> {
						items.addAll("Normal",
							"Uncommon",
							"Rare",
							"Epic",
							"Legendary",
							"Mythic")
					}
				}
				field("Random Seed") {
					spinner<Number>(Int.MIN_VALUE, Int.MAX_VALUE, editable = true, property = seed, enableScroll = true)
				}
				field("Level") {
					spinner<Number> {

					}
					label("= power 94")
				}
				field("Adapted") {
					checkbox(null, adaptedChecked)
				}
			}
		}
	}
}
