### TL;DR:
There is this RPG called "CubeWorld" whose developer vanished. There is still an active community playing the game but it is slowly dieing due to the lack of new content and the online experience being awful: While the game itself works great, the vanilla server is near unusable. I want to save this game so I started making my own server from scratch, and while doing so I discovered the huge potential of this game.

### DETAILED DESCRIPTION:
This project (namely "Exceed") is what I've been working on and learned programming with for the past months in my free time. 
CubeWorld came out in 2013 and was very hyped back then. For whatever reason the developer ("Wollay") disappeared, without ever releasing any updates. Even though the game is still in its first alpha version today, it is surprisingly well playable and actually still has an active community playing it. Among the game files there is a server (we call it "vanilla") that any player can host to play online with others. This server however is broken and overall bad in pretty much every way possible:
1. It is unstable and constantly crashes
2. It is slow and causes lots of lag
3. It fries your CPU
4. It uses ridiculous amounts of bandwidth
5. It is restricted to 4 simultanous connections

I want to save this game by jumping into the developers role. The one big handicap with that is: We don't have the source code of either the game client or the vanilla server and there is no API or anything like that, which makesthe creation of custom patches (also known as "mods" in the gaming world) crazy difficult: you'd have to hack your stuff into the assembly or use dll injections, you could as well just rewrite the game. We did lots of reverse engineering anyway figured out the network protocol used for online game play. We also learned that almost everything is processed in the client and the server mostly only serves for passing data from A to B, which is why we decided to completely recreate the vanilla server from scratch rather than trying to fix it. Someone recreated the vanilla server this way, but while his server (called "CuWo") solves #1, #3 and #5, the problems #2 and #4 persist.

I think we can do better than that.

During the data digging I found lots of hidden, unused content that can easily be brought to daylight without much work. I also realized that most of the traffic in cubeworld multiplayer is redundant, so I came up with an idea that pretty much reinvents the way multiplayer works in CubeWorld: I introduced a 3rd component into the network, the so-called "bridge" which is localhosted by every player. Instead of connecting the client directly to the server, we now connect it to the bridge which in turn is connected to the server. The bridge can be used to do all sorts of things, such as filtering out the redundant data from the data stream (to solve the bandwidth and lag problems), adding an authentification layer to the connection (to be able to ban hackers) or write into the games memory while it is running (to trigger unused content) and much more.

I started realizing this idea by creating my own server (I call it "Exceed"), and I already managed to bring it into a playable state. Currently Exceed is less stable than CuWo, but way faster, efficient and more promising due to the way it works. We now have alot of freedom and possibilites to what we can do with the game, and the best part is that there is a community that will be happy about anything we bring to them since they've been waiting for this game to be updated for over 4 years by now. I am open to any suggestions about what we should implement, here is a list with ideas the community and I came up with:

### PLANS AND GOALS:
- bring all hidden/unused content to daylight that we can find in the game (there is A LOT of hidden/unused stuff and I probably don't even know about all of it yet)
- add an account system to identify players (right now, if someone changes his character and ip address there is no way to recognize him, making permanent bans impossible)
- invent an account-bound clan system (for some reason the CubeWorld community really likes the idea of clans) 
- add a website with a livemap and leaderboards so players won't ever be forgotten (something the community was complained about alot in the past 4 years)
- a peer2peer connection system to cut ping in half (server will only remain as a matchmaker and anticheat instance)
- balancing the game (currently the playable classes aren't equally strong, making pvp very one-sided)
- add whatever content we can come up with (there are many small things that can be added up to a new feature, eg red particles + damage over time = bleeding, or lots of arrow projectiles = new ranger ability "arrow rain", there is endless possibilites)
- fix bugs in the game (the game is still in its first alpha version, there have never been any patches. We have the ability to write into the games memory, we can use that to fix bugs while theing game is running without even knowing the source code)
- anything else we come up with, this is basically a sandbox now

### TEAM SIZE:
We'll need an SE and an Id that will mostly be focusing on designing and creating a website for the server, and another SE working on the backend (server and bridge). The latter person should at least be familiar with OOP, and preferably familiar with C# because that's the language everything is written in. I myself will be in a hybrid role between SE and PM, because on the one side i need to introduce the other SEs to the current code and on the other side I am (probably) the only one familiar with the game and the community. The community is very important because without them this project is useless. There is definitelyroom for another PM here.
So in short (role and what they'll roughly do):
- ID #1: Frontend, web design (and possibly structuring a desktop application)
- SE #1: Frontend, web implementation
- SE #2: Backend, database and networking (networking is easy, I can teach you) 
- PM #1: Team and community communication
With myself included that would be a team of 5 in total.

### SOFTWARE REQUIREMENTS:
- SE#2 will need to use Visual Studio. It is probably the best IDE for C# out there and working with different editors would make helping each other difficult at our skill level. This isn't a commercial project (yet) so the "community editon" (free) will be enough.
- SE#1 and ID#1 can use whatever they prefer since the website doesn't exist yet
- Please, for the love of god, get discord. Slacks performance is horrible!

### CONFIDENTIALITY:
This project is, has been, and will (probably) alsways be: OPEN SOURCE. there is no confidential data or intellectual property that requires special regulation.

### PROJECT DURATION/WORKLOAD:
I suggest the project to be B-type, meaning it will cover 10 weeks. though I do want to note that I have no experience with deadlines yet, especially not for projects this big.
