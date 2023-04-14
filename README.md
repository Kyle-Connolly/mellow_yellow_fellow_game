# mellow_yellow_fellow_game

This project was created for the CSC2034 Introducing Contemporary Topics in Computing module "Project and Report" assessment at Newcastle University.

A report was produced alongside this project and explains approaches, reasoning, implementation, and contains a full list of references. The report is available upon request.

This Unity project was created using Unity 2022.1.3f1

-------------------------------------------------
Scenario: "Your development work on the Mellow Yellow Fellow has attracted the attention of a local game development studio, who want you to develop a new game prototype.  They want to see how you would develop a sequel to your game, with new and improved features."

Task: The Mellow Yellow Fellow game is a currently 'unfinished' replica of an existing game.  Using the provided Unity project as a starting point, enhance the game to include multiple new features:

- The Fellow and the Ghosts should be able to use the tunnel at the sides of the game arena to 'teleport' from side to side.
- Ghosts should return to the centre of the maze to be restored after having been 'eaten' by a powered-up Fellow.
- The player should have multiple lives, with the game resetting the location of the Fellow and Ghosts at the start of each 'life'.
- Rather than ending, the game should reset, allowing the player to complete a new 'level'.
- The game should have a user interface, informing the player of the current score, best high score,  life count, current level, and so on.
- New high scores should be managed, so that they can be saved for later plays of the game.

Extension Task: Modern games have many additional features that you might want to explore implementations of.

Extension features were chosen from a list of suggested features, and completely unique ones in an attempt to balance the game:
- System Balancing: disabling power ups and implementing respawn immunity.
- New “Crazy Ghost” Behaviour: every five rounds one Ghost is converted to a Crazy Ghost (ignores powerup behaviour and locks on to player location from the moment it obtains line of sight on the player).
- Strong Visual Style: converted project render pipeline to URP. Implemented a skybox and materials downloaded from the Unity asset store. Implemented post processing.
-------------------------------------------------
Assets:
- rgwhitelock. (2021) AllSky Free - 10 Sky / Skybox Set. Available at: https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014 (Accessed 10 March 2023)
- LowlyPoly. (2021) Stylized Sci-fi Texture. Available at: https://assetstore.unity.com/packages/2d/textures-materials/stylized-sci-fi-texture-192893 (Accessed 10 March 2023)

Code References:
- Jimmy Vegas (2019) HOW TO WRITE A TELEPORT SCRIPT IN C# UNITY TUTORIAL. 25 November. Available at: https://www.youtube.com/watch?v=WzzxjFD6-Mg (Accessed 15 March 2023)
- Zoe Straw (2022) Unity writing to text files – Unity Quickies. 29 March. Available at: https://www.youtube.com/watch?v=R3AELllFqbU (Accessed 20 March 2023)
- DiGiaCom-Tech. (2016) ‘Teleport Script’, Unity Answers, 25 May. Available at: https://answers.unity.com/questions/1191795/teleport-script-2.html
- Markus Olsson, Glorfindel (2022) ‘How to delete a line from a text file in C#?’, StackOverflow, 5 June. Available at: https://stackoverflow.com/questions/668907/how-to-delete-a-line-from-a-text-file-in-c
