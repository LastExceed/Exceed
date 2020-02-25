package exceed

import kotlinx.coroutines.io.ByteChannel
import kotlinx.coroutines.io.close
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import java.io.IOException

class Myclass(var name: String)

fun main() {
	println("=====Exceed=====")
	runBlocking {
		Server.start()
	}
}


//TODO: code quality
//immutable packets + factory functions instead of constructors
//be consistent about parameter order (packets and source)

//TODO: features
//login
//join/leave messages
//anticheat - character visuals, illegal items, ect
//visualize manashield using quest HP bar
//tombstones - go back to corpse to revive like in World of Warcraft - use negative time (-10100000) to visualize ghost world
//damage over time - bleeding, burning, poison


//TODO:
//readfunction one line
//internal