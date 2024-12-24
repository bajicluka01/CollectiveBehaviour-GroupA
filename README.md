# CollectiveBehaviour-GroupA
Repository for a group project for the course Collective Behaviour




Group members (collaborators):
- Nik Čadež - [RootRooster](https://github.com/RootRooster) 
- Pedro Nuno Ferreira Moura de Macedo - [pedronunomacedo](https://github.com/pedronunomacedo) 
- Primož Mihelak - [PMihelak](https://github.com/PMihelak) 
- Luka Bajić - [bajicluka01](https://github.com/bajicluka01) 




## Topic: Simulation of group behaviour during a protest



### Abstract

The purpose of our project is to study the behavior of a crowd during a protest. In order to do so, we will first create a unified modular development environment that implements the basic flocking model. 

Then we will add obstacle avoidance and place the model in a topological map of Ljubljana. Furthermore, we will divide agents into [different groups](https://www.researchgate.net/publication/281938638_Agent-Based_modeling_of_protests_and_violent_confrontation_a_micro-situational_multi-player_contextual_rule-based_approach) (e.g. leader, regular protest member and bypasser) and create different behavioural patterns for each group based on [group psychology](https://repozitorij.uni-lj.si/IzpisGradiva.php?id=66052). 

Finally, we will add agents for [crowd control](https://www.researchgate.net/publication/347869556_Testing_Various_Riot_Control_Police_Formations_through_Agent-Based_Modeling_and_Simulation) (e.g. police) and examine the effect they have on the behaviour of the crowd. 



### Milestones:

#### First report (November 16th)
  - Review and implementation of existing models
  - Creation of the Ljubljana topological map

#### Second report (December 7th)
- Implementation of obstacle avoidance
- Creation of behavioral patterns for each group

#### Third report (January 11th)
- Add agents for crowd control
- Evaluation of implemented models
- Final conclusions
- Final presentation



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
