BUBBLE POPCORE! - A Bubble Pops Clone

Made with Unity 2022.3.22f1

-- THIRD PARTY PLUGINS USED:
DOTween
UniTask
NiceVibrations (Feel package)


-- GUIDE VIDEOS:
I found a second video, which helped me understand many of the game's mechanics. I am also posting it here.
https://www.youtube.com/watch?v=SglS9LQOsRA

https://www.youtube.com/watch?v=jWRIf23YUeA



-- GAME FEATURES:
* Core gameplay is implemented
* "Shuffle" and "push up" buttons are working (First and third buttons, respectively)
* Dropdown occurs randomly with 33% chance (I could not figure out the exact rule for dropdown from the videos)



-- CURRENTLY MISSING FEATURES:
* Bubble doesn't pop at 2048 (work in progress)
* Bubbles cannot spawn in top layer (work in progress)
* Level-up and score system
* Pause button not operational
* In game pop-ups not present
* There is no lose state



-- HOW THE CODEBASE WORKS:
GameFlow class is the starting point for the game. It has a GameStateMachine class that separates the game into different game states:
* Preparation state: Bubbles to be shot are prepared
* Shoot state: Player input is received, target cell and path is calculated
* Move state: Bubble is moved and placed
* Resolve state: Merge chains are calculated and executed, followed by finding and dropping any bubbles that are not connected to the top
* TidyUp state: Calculate and execute dropdown (also, lose state should be calculated here)

The main logic of the game is in these state classes that derive from the GameState abstract class.

The project implements an object pool to handle Bubble spawning and despawning.

The project uses a rudemantary DependencyContainer which dispenses the instances of many necessary classes in the game.
I did not use Zenject because I did not need the complexity, and frankly I don't enjoy how it becomes unremovable once implemented.
Many classes use interfaces for interchangeability.



-- EXTERNAL ASSET REFERENCES:
Fonts: 
* Motley Forces: https://www.fontspace.com/motley-forces-font-f87817
* Jeepers Font: https://www.fontspace.com/jeepers-font-f9772

Game Icon: (Coloured by PhotoPea)
https://www.flaticon.com/free-icon/bubbles_2101367

Button Icons:
* Shuffle: https://www.flaticon.com/free-icon/shuffle_3031711
* Bullseye: https://www.flaticon.com/free-icon/bullseye_796702
* Right: https://www.flaticon.com/free-icon/right_11209951
* Pause: https://www.flaticon.com/free-icon/pause_175681

Sound Effects:
* Pop effects:
https://www.zapsplat.com/music/cartoon-bubble-pop/
https://www.zapsplat.com/music/cartoon-bubble-pop-or-other-popping-sound-version-1/
https://uppbeat.io/sfx/bubble-pop-2/8487/24845
https://uppbeat.io/sfx/mouth-pop-single-loud/12696/28979
* Series of pops effect: https://www.zapsplat.com/music/cartoon-bubble-pop-sequence-fast-ascending-pops-2/
* Ping sound: https://kenney.nl/assets/impact-sounds
* Swoosh sound: https://www.zapsplat.com/music/badminton-or-tennis-racket-fast-whoosh-swoosh-swing-1/
* Jingle: https://kenney.nl/assets/music-jingles
