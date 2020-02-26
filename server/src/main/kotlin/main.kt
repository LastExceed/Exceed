package exceed

import kotlinx.coroutines.runBlocking

fun main() {
	println("=====Exceed=====")
	runBlocking {
		Server.start()
	}
}

//TODO: features
//login
//join/leave messages
//anticheat - character visuals, illegal items, ect
//visualize manashield using quest HP bar
//tombstones - go back to corpse to revive like in World of Warcraft - use negative time (-10100000) to visualize ghost world
//damage over time - bleeding, burning, poison

//async everything