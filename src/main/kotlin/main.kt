import com.andreapivetta.kolor.*

suspend fun main() {
	println(Kolor.foreground("=====Exceed=====", Color.MAGENTA))
	Server.start()
}

//rock of justice
//status effect heart/swirl visible to target/source/id3 ?
//blocking gauge false positive
//teams (visible on top? can i reassign id?)
//cant heal to full
//broadcast poison sound

//gathering point

//remove poison on death
//shop using respawning ground items

//v hotkey while falling = crash everyone
//animation cancel with glider
//mages/warrior-dualwield no damage first few hits (because combo scales armor/resi penetration?)
//dont show downlevel message to every1

//intercept during tp = hit everyone

//===test these===
//ClosedReceiveChannelException trying to read next packet id from closed stream
//fast blocking spam

//===fix these===
//allow mana cube pickup animation ?
//intercept stops updating velocity too early


//===AC===
//shuriken glitch - too many false positives - check for charge during shuriken only
//sailing on a glider (and vice versa?)
//recheck sniping on class change
//recheck animation when class changes
//recheck appearance when race changes
//low gravity flag (and others?)
//mage cant charge



//===parsing===
//rarity 6 items
//pet name = invalid spirits

//block in the sky
//small jumps get filtered
//can heal enemy
//billion items to inventory