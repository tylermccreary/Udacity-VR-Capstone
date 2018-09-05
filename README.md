# Capstone Project

This project is part of [Udacity](https://www.udacity.com "Udacity - Be in demand")'s [VR Developer Nanodegree](https://www.udacity.com/course/vr-developer-nanodegree--nd017).

## Versions
- Unity 2018.1.0b13

## Instructions
- Clone Repository
- Import Viking Village from the link
- Import the SteamVR Plugin for Unity
- Add all of the Scenes to the build settings.
- Navigate to the VikingMenu Scene.
- Play the scene in the editor or run a build.

#Sources
- Viking Village: https://assetstore.unity.com/packages/essentials/tutorial-projects/viking-village-29140
- Goal Horn Audio: https://freesound.org/people/santino_c/sounds/170826/
- Goal Crowd Audio: https://freesound.org/people/paulw2k/sounds/196461/
- Clock Beeping Audio: https://freesound.org/people/jeckkech/sounds/391650/
- Ball Audio: https://freesound.org/people/BranRainey/sounds/108737/
- Game Over Audio: https://freesound.org/people/guitarguy1985/sounds/54047/
- Crowd Ambiance Audio: https://freesound.org/people/eguobyte/sounds/360703/

## Information
- Tyler McCreary
- Video: https://www.youtube.com/watch?v=vVLiXWmqf8c
- Achievements:https://youtu.be/mG17x-XIc7c
- Lighting Video: https://youtu.be/C6Lw9x2co9I
  Fundamentals (500):
   - Scale (100) On the menu, click Play->XL. This is a much larger court than the regular one. I could have also made the AI larger to make the user feel like the only small entity.
   - Animation (100) The AI moves while running. Also, the goals animate upward when the game starts.
   - Lighting (100) The lighting for each scene is baked. When a goal is scored, a real-time point light will appear in the goal with a pulsing intensity.
   - Locomotion (100) The player can run, jump, grab, and throw.
   - Physics (100) The ball and players use physics.
  Completeness (750):
   - Gamification (250) This is a game, so I think it applies. I wanted to reward users with powerups and keep stats, but ran out of time.
   - Diegetic UI (250) Tutorial, menu feedback, and other UI giving the player feedback (i.e. The goals are lit up and turn green when the player scores.)
   - AI (250) The AI could use some work. In the game's current state, it should be using a NavMesh, but with my original plans a NavMesh would not result in the ideal AI behavior.
  Challenges (500):
   - Speech Recognition (500) If your teammate has the ball you can say "open" or "shoot".
      Open: The teammate with the ball will turn toward you and pass the ball.
      Shoot: The teammate with the ball will shoot from their current position.
- Emotion: Joy or accomplishment when scoring. I added a crowd roaring sound and a horn when a goal is scored to try to add to the emotion aspect.

Writeup: https://medium.com/@tjmccreary/vr-capstone-9d376b59e708

Brainstorming List:
Sport— Drawing inspiration from games like Rocket League, Echo Arena, Fortnite, CSGO, and more, I came up with an idea for a multiplayer game in which players would try to score in one of multiple goals on each side. In between rounds, players would be rewarded with powerups and the ability to build walls.
Puzzle Game — Many different levels in which the players would have to solve a puzzle or reach a specific location using abilities like growing and shrinking objects as well as themselves.
Shooter — Similar to Zombies in Call of Duty, enemies would attach in waves and the player would try to stay alive for as long as possible.

Sport - Doable, but I will need to make certain features a priority. Powerups and walls will be low priority.
Puzzle Game - Doable. Need to come up with the basic levels and just add on to it.
Shooter. Doable. Would need to get a lot of assets from the store (enemies, guns, etc.)
