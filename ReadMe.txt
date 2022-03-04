This is a unity project that implements a temporal difference learning AI.

The AI agent is placed on a grid with a predetermined goal and set of obstacles. When the agent runs into a wall or reaches the goal, 
the agent will respawn on a random tile on the grid to ensure the agent will learn how to reach the goal regardless of starting position.
Initially the AI uses an exploration procedure to explore the world and learn about the various moves it can make. As the number of trials 
gets higher and higher, the agent will use an exploitation procedure more and more so that it will make correct moves more consistently.

The project currently runs through 1200 trials and tracks the number of trials as well as how many times the AI reached the goal.