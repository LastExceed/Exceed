package modules

import Player
import java.io.File

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
		registerCommands()
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