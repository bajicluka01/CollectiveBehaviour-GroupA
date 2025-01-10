# CollectiveBehaviour-GroupA
Repository for a group project for the course Collective Behaviour @ FRI Ljubljana




Group members (collaborators):
- Nik Čadež - [RootRooster](https://github.com/RootRooster) 
- Pedro Nuno Ferreira Moura de Macedo - [pedronunomacedo](https://github.com/pedronunomacedo) 
- Primož Mihelak - [PMihelak](https://github.com/PMihelak) 
- Luka Bajić - [bajicluka01](https://github.com/bajicluka01) 




## Topic: Simulation of group behaviour during a protest



### Abstract

The purpose of our project is to model the behaviour of a crowd during a protest as accurately as possible and attempt to observe the emerging behavioural patterns. At the start of a simulation we populate the scene with agents belonging to different subgroups: leader, protester, bystander. Eventually they can fluidly change between groups based on various parameters, such as proneness to defection and recruitment. These parameters depend upon the distribution (in the sense of groups) of agents in an individual's field of view. The movement of the leader is determined by arbitrary goals within the topological map, while the other agents follow the leader when it appears in their field of vision. To give the simulation a practical use, we additionally allow the user to manually place police agents into the scene and observe how they impact the behaviour of the crowd.


### Summary of obtained results

- End-Position-Seeking-Behaviour forms a concentrated mass of agents in the center of a protest, while the bystanders tend to move towards the perimeter.
- Active protesters follow the leader as it appears in their field of view.
- Emotional contagion causes agents to occasionally switch their state depending on their surroundings. 
- A hierarchy of leaders forms when a leader is identified and disperses when the leader either unidentifies itself (i.e. loses motivation) or exits other protesters' fields of view.

### References

- Itatani, Pelechano - Social Crowd Simulation: Improving Realism with Social Rules and Gaze Behavior (2024)
- Lemos, Coelho, Lopes - Agent-Based modeling of protests and violent confrontation: a micro-situational, multi-player, contextual rule-based approach (2014)
- Clements, Fadai - Agent-based modelling of sports riots (2022)




## Instructions for running the simulations

Install the latest version of [Unity](https://unity.com/download)

Run a terminal and clone our repository by typing the following command:
```
git clone https://github.com/bajicluka01/CollectiveBehaviour-GroupA.git
```

Navigate into "code" folder and delete "protest_simulation" folder.

Run Unity and create a blank project using "Universal 2D" template. Name the project "protest_simulation" and save it inside "code" folder. Exit Unity.

Create a ".gitignore" file in the "protest_simulation" folder and paste the contents of [this](https://github.com/bajicluka01/CollectiveBehaviour-GroupA/blob/main/code/protest_simulation/.gitignore) file into it.

Run a terminal, navigate into CollectiveBehaviour_GroupA folder and run the following commands:
```
git fetch --all
```

```
git reset --hard origin/main
```

Thus you have obtained the latest commited files. You can now open the project in Unity and freely change the starting parameters of the simulation by clicking "Protesters" in Scene View.

### Keybinds

- camera movement: arrow keys 
- camera zoom in: C
- camera zoom out: X 
- increase zoom speed: +
- decrease zoom speed: -
- manual leader movement: W, A, S, D
