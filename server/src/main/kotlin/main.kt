package exceed

import kotlinx.coroutines.delay
import kotlinx.coroutines.runBlocking
import kotlinx.coroutines.sync.Mutex
import kotlinx.coroutines.sync.withLock

suspend fun main() {
	println("=====Exceed=====")
	Server.start()
}



//TODO: features
//login
//join/leave messages
//anticheat - character visuals, illegal items, ect
//visualize manashield using quest HP bar
//tombstones - go back to corpse to revive like in World of Warcraft - use negative time (-10100000) to visualize ghost world
//damage over time - bleeding, burning, poison