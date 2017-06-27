# Exceed
is another custom server for the game CubeWorld (by picroma). It is particularly focusing on speed and traffic minimization.
To achieve this, the server is filtering out any obsolete data (which makes up a whopping 70% of all traffic between server and client!) before passing it to the other players.
Additionally it allows the player to use a MITM server (which we refer to as "bridge") which filters the upstream of the players.

The bridge translates everything to a custom UDP protocol to allow even more efficient traffic minimization between server and client (the UDP datagrams are roughly 1/4 the size of CubeWorlds standard TCP packets).
Further more it displays a list of currently connected players and logs the chat, allowing the user to finally scroll up.

The bridge opens alot of possibilites for new features since we can now directly access the client and modify any data.
We are also planning to use it to introduce an account system which will allow us to uniquly identify players which will come in handy when dealing with ban evaders and impersonators, as well as when establishing the planned pvp leaderboards.
