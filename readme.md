# Games Middleware

This project was for my Games Middleware module which was a module focused on teaching us about various technologies created already for developers to utilise in their games.

## Project Structure
### Physics
Physics was the most in-depth section and went into more detail about how physics works. We also developed a personal physics solution for a plane and sphere separate from Unity's provided one. It is also written in a way that it is possible to be extendable for more objects if necessary.

### Animation
This was implementing animation for a character in Unity. It possessed the following requirements:
- Blend Tree (Run and Walk use blend trees for moving in various directions)
- State Machine 
- Behaviours (I play sounds when the character moves)
- Inverse Kinematics (I used Uniy's Animation Rigging package for this)
- Animations from multiple sources

### Networking
This was done by implementing Unity's Netcode for GameObjects solution. It allows two clients to connect and have synced movements and animations. I used my character created for the animation. The two clients fight each other and it works as intended. The IK I had implemented was a punching animation so I made the fist connect with the other person's head.

### Eye Tracking
I implemented eye tracking using the Tobii 4c Eye Tracker. They have a unity package which I used to make it navigate a maze. The user can only move forwards and backward and must use the eye tracker that serves as the method of moving the camera to look around.
