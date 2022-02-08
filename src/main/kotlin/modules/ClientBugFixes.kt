package modules

import com.github.lastexceed.cubeworldnetworking.packets.*
import Player

object ClientBugFixes {
	//self heals are already applied client-side so the server musn't apply them again
	fun ignoreSelfHeal(source: Player, hit: Hit): Boolean {
		return hit.damage < 0f &&
				hit.target == source.character.id
	}
	//groudheal shenanigans:
	//- a single shot creates 1 - 3 stacked puddles depending on computer performance
	//- equipping a bracelet in the right hand doubles the amount of stacked puddles created by a single shot
	//  ^ has nothing to do with the animation starting with the right hand
	//- heals are applied both client side and server side, resulting in twice the hp recovered
	//  ^this means with a bracelet you can end up healing up to 12x the intended amount
	//- stacked puddles heal the same amount, but their crit is rolled independently

	//prevent warrior M2 from dealing damage without mana charge
	//M2 jump
}