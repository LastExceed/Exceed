### TL;DR:
There is this RPG called "CubeWorld" whose developer vanished. While the game itself works great, the vanilla server is near unusable. There is still an active community playing the game but it is slowly dieing due to the online experience being awful. I want to save this game so I started making my own server from scratch, and while doing so I discovered the huge potential of this game.

### DETAILED DESCRIPTION:
This project (namely "Exceed") is what I've been working on and learned programming with for the past months in my free time. 
CubeWorld came out in 2013 and was very hyped back then. For whatever reason the developer ("Wollay") disappeared, without ever releasing any updates. Even though the game is still in its first alpha version today, it is surprisingly well playable and actually still has an active community playing it. Among the game files there is a server (we call it "vanilla") that any player can host to play online with others. This server however is broken and overall bad in pretty much every way possible:
1. It is unstable and constantly crashes
2. It is slow and causes lots of lag
3. It fries your CPU
4. It uses ridiculous amounts of bandwidth
5. It is restricted to 4 simultanous connections even though the game would work great with bigger numbers of players than that

The one big handicap is: We don't have the source code of either the game client or the vanilla server and there is no API or anything like that, which makes custom patches (also known as "mods" in the gaming world) crazy difficult (you'd have to hack your stuff into the assembly and use dll injections, you could as well just make your own game). We did lots of reverse engineering anyway figured out the network protocol used for online game play. We also learned that almost everything is processed in the client and the server mostly only serves for passing data from A to B, which is why we decided to completely recreate the vanilla server from scratch rather than trying to fix it.

Someone recreated the vanilla server, but while his server (called "CuWo") solves #1, #3 and #5, the problems #2 and #4 persist.

I think we can do better than that. 



I started making my own server (I call it "Exceed"), and I already managed to bring it into a playable state. 

Currently Exceed is less stable than CuWo but way faster and efficient because I found a way to drastically reduce 




Exceed is the attempt to save a dieing RPG (RolePlayGame) called "CubeWorld" whose developer vanished. All that is left is the game client and a broken, almost unusable server (called "vanilla"). We do not have the source code to either of those, but we do have the ability to read and understand the communication between the two, and thus the opportunity to reconstruct the server by simply copying what the vanilla server would respond to the clients requests. Someone else has done this before and created an open source server called "cuwo", but while its running stable and doesn't fry your CPU like the vanilla server does, It's still slow and requires ridiculous amounts of bandwidth, both problems that cannot be fixed in his server due to the way it operates, so I decided to reinvent the wheel once more:
While building my own server and examining the networking protocol I realized that
a) the vanilla server wastes lots of bandwidth with redundant traffic
b) there is lots of hidden/unused content in the game
c) almost everything is processed client side, the server mostly just serves for passing information from one clien to another.

I saw the potential to completely reinvent the online experience of CubeWorld, because I realized I could show the players lots of content they have never seen before without even altering the game because of b)
c) means that technically a server isn't even necessary other than for the purpose of matchmaking.
I decided to introduce a 3rd component to the network structure called "bridge". The bridge is a localhosted application every player needs to connect to the server. The bridge translates the bandwidth-wasting TCP protocol of CubeWorld to an efficient UDP protocol to fix a). It can also directly write into the games memory to trigger hidden/unused content that is otherwise inaccessible. With the bridge we can do all sorts of things because we now have a component on both the clients end AND the server end whose source code is available to us.

right now the server is in a playable state but to the player it doesn't look much different from a cuwo or vanilla server except that connecting is more complicated and the connection is much smoother and requires less bandwidth.
### PLANS AND GOALS:
- bring all hidden/unused content to daylight that we can find in the game (there is A LOT of hidden/unused stuff and I probably don't even know about all of it yet)
- add an account system to identify players (right now, if someone changes his character and ip address there is no way to recognize him, making permanent bans impossible)
- invent an account-bound clan system (for some reason the CubeWorld community really likes the idea of clans) 
- add a website with a livemap and leaderboards so players won't ever be forgotten (something the community was complained about alot in the past 4 years)
- a peer2peer connection system to cut ping in half (server will only remain as a matchmaker and anticheat instance)
- balancing the game (currently the playable classes aren't equally strong, making pvp very one-sided)
- add whatever content we can come up with (there are many small things that can be added up to a new feature, eg red particles + damage over time = bleeding, or lots of arrow projectiles = new ranger ability "arrow rain", there is endless possibilites)
- fix bugs in the game (the game is still in its first alpha version, there have never been any patches. We have the ability to write into the games memory, we can use that to fix bugs while the game is running without even knowing the source code)
- anything else we come up with, this is basically a sandbox

### TEAM SIZE:
- I'll definitely need an ID and an SE who are familiar with website stuff.
- 1 or 2 SE coding the server and bridge (these should at least be familiar with OOP, and preferably familiar with C# because that's the lan1guage everything is written in). I'll be working here.
- I don't know about PM because
-- I am probably the only one familiar with the game and the project
-- I have yet to experience the usefulness of a project manager (the 3rd orientation project will probably clear that one up for me)
but if we take a PM on board, 1 should be enough. Since I know nothing about PMs I have no idea what competencies to require here.
- Maybe a second ID for the bridge but I'm not sure if that will be necessary, because if we have a dedicated SE for the website, then the first ID should be able to handle designing both the website and the bridge (the server is a console application and thus doesn't require design).

In total that makes
- SE: 2-3
- ID: 1-2
- PM: 0-1

### SOFTWARE REQUIREMENTS:
- The backend SEs will need to use Visual Studio. It is probably the best IDE for C# out there and working with different editors would make helping each other difficult at our skill level. This isn't a commercial project (yet) so the "community editon" (free) will be enough.
- The frontend SE and other roles can use whatever they prefer as nothing apart from the backend exists yet as of now.
- Please, for the love of god, get discord. Slacks performance is horrible!

### CONFIDENTIALITY:
This project is, has been, and will (probably) alsways be: OPEN SOURCE. there is no confidential data or intellectual property that requires special regulation.

### PROJECT DURATION/WORKLOAD:
I suggest the project to be B-type, meaning it will cover 10 weeks. though I do want to note that I have no experience with deadlines yet, especially not for projects this big.
