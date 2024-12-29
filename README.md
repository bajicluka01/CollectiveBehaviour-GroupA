# CollectiveBehaviour-GroupA
Repository for a group project for the course Collective Behaviour




Group members (collaborators):
- Nik Čadež - [RootRooster](https://github.com/RootRooster) 
- Pedro Nuno Ferreira Moura de Macedo - [pedronunomacedo](https://github.com/pedronunomacedo) 
- Primož Mihelak - [PMihelak](https://github.com/PMihelak) 
- Luka Bajić - [bajicluka01](https://github.com/bajicluka01) 




## Topic: Simulation of group behaviour during a protest



### Abstract

The purpose of our project is to model the behaviour of a crowd during a protest as accurately as possible and attempt to observe the emerging behavioural patterns. At the start of a simulation we populate the scene with agents that belong in different subgroups (leader, protester, bystander), but eventually they can fluidly change between the groups based on various parameters, such as levels of aggression. The movement of the leader can either be manually controlled by the user, or determined by arbitrary goals within the topological map, while the other agents follow the leader when it appears in their field of vision, depending also on their aggression parameters. Additionally, the user is able to manually place police agents anywhere on the map to serve as a repulsive force for other agents. 


# References

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
