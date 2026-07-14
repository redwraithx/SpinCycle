# Wash Off

> A competitive online multiplayer party game developed by CrazyBeaverLabs as a college capstone project in 2021.

![Engine](https://img.shields.io/badge/Engine-Unity-black?logo=unity)
![Networking](https://img.shields.io/badge/Networking-Photon%20PUN-blue)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey)
![Status](https://img.shields.io/badge/Status-Archived-red)

---

# About

**Wash Off** is a competitive online multiplayer party game where two robotic laundry workers compete aboard a futuristic space station to wash, dry, and deliver as much laundry as possible before time expires.

Players must balance completing laundry with disrupting their opponent using randomly spawned power-ups. Every match encourages strategic decision-making as players choose between maximizing efficiency or sabotaging the opposing player. The result is a fast-paced, lighthearted multiplayer experience that rewards timing, adaptability, and quick thinking.

Developed using **Unity** and **Photon PUN**, Wash Off features online multiplayer gameplay, custom 3D assets, original animations, visual effects, user interface systems, and audio created collaboratively by a multidisciplinary development team.

Although originally developed as a college capstone project, Wash Off was designed using many of the same collaborative workflows found in professional game development, including dedicated production roles, code reviews, technical leadership, and interdisciplinary collaboration.

---

# Gameplay

Players compete against one another in a race to process the most laundry before the match timer expires.

Throughout each match players must:

- Collect dirty laundry
- Wash clothing
- Dry completed loads
- Deliver finished laundry for points
- Collect randomly spawned power-ups
- Sabotage their opponent
- Adapt to changing gameplay situations

Every match becomes a balance between maximizing efficiency and preventing your opponent from doing the same.

---

# Project Highlights

- Online multiplayer gameplay using Photon PUN
- Competitive two-player party gameplay
- Multiplayer lobby system
- Power-up and sabotage mechanics
- Custom 3D assets created in Blender
- Original animations and VFX
- Custom UI/UX
- Original music and sound effects
- Developed over approximately ten months
- Released in September 2021

---

# Technical Achievement

Wash Off represents an important milestone for our Game Development program.

At the time of its completion in 2021, Wash Off was the **first fully networked multiplayer capstone project developed within the program**.

Choosing to build an online multiplayer game significantly increased the technical complexity of the project compared to a traditional capstone. Gameplay systems, player movement, UI, scoring, interactions, animations, and game state all needed to synchronize correctly across multiple connected players while remaining responsive and enjoyable to play.

Successfully delivering a fully playable online multiplayer experience within a ten-month development schedule required close collaboration between programmers, artists, animators, designers, producers, and audio contributors.

---

# Choosing the Networking Solution

One of the earliest technical challenges facing the project was selecting a networking solution that met both the team's requirements and budget.

In 2020, Unity's multiplayer ecosystem offered several networking options, but few were practical for a student project with virtually no budget. Finding a solution required several weeks of research, experimentation, and prototyping before **Photon PUN** was selected as the foundation for the game's online multiplayer architecture.

Once the networking layer had been integrated successfully, internal programming guidelines were established to help the rest of the programming team build multiplayer systems safely and consistently.

These guidelines covered:

- How gameplay data should be synchronized
- How network messages should be structured
- How systems should communicate across the network
- How to reduce synchronization issues
- How to avoid potential network-related crashes

By establishing a shared technical foundation early in development, the team was able to develop multiplayer gameplay systems with greater confidence and maintain a consistent networking architecture throughout the project.

---

# My Contributions

As **Technical Director / Lead Developer**, my role extended beyond implementing gameplay features. My primary responsibility was to establish the project's technical direction while supporting the programming team throughout development.

## Technical Leadership

- Researched and evaluated networking technologies suitable for a no-budget student project.
- Selected and integrated Photon PUN as the project's networking solution.
- Designed and implemented the core multiplayer architecture.
- Established networking standards and synchronization guidelines for the programming team.
- Trained teammates on multiplayer development workflows and network communication.
- Helped programmers understand packet structure, data synchronization, and safe multiplayer practices.
- Assisted teammates with technical challenges throughout development.
- Encouraged modular and decoupled programming practices appropriate for the team's experience level.

## Programming Contributions

Throughout development I contributed to a variety of gameplay and technical systems including:

- Multiplayer networking
- Gameplay programming
- Main menu systems
- Multiplayer lobby systems
- Gameplay interactions
- Feature integration
- General programming support
- Technical troubleshooting

## Code Review & Collaboration

One development practice I strongly believed in was peer review.

Every programming contribution submitted to the repository was reviewed before being merged into the project. The goal was not only to identify bugs but also to encourage knowledge sharing, maintain consistency across the codebase, and help the programming team learn from one another.

Although I served as the project's Technical Director, I believed my own work should be held to the same standard. Every pull request I submitted was reviewed by **Evan Peppler** before being merged into the project.

No developer is immune to mistakes, and having another programmer review your work helps improve software quality while encouraging collaboration and shared responsibility.

---

# Development Team

| Team Member | Role |
|--------------|------|
| Austin Cudmore | 3D Modelling |
| Anthony Rinaldi | Audio / General Programming |
| Brandon Wall | Lead 3D Modeller |
| Brian Purdy | Network Programming / General Programming |
| Chirag Patel | General Programming |
| Evan Peppler | UI / General Programming |
| Faisal Alireza | Animations |
| Hoai Anh Ung | Animation / General Programming |
| Jesse Ottmann | UI / Art Design & Concepting |
| Owen Hooper | VFX / General Programming |
| Taariq Saunders | VFX Artist |

---

# Project Leadership

## Design & Production Lead

**Evan Peppler**

## Producers

### Art Producer

**Brandon Wall**

### Code Producer

**Owen Hooper**

## Technical Director / Lead Developer

**Brian Purdy**

---

# Special Thanks

| Name | Contribution |
|------|--------------|
| Dylan Morrow | Assistant Modeller |
| Amardeep Seeboruth | Assistant Modeller |
| Marc Cacho | VFX Technical Artist |
| Alyssa Lavilla | UI/UX Design |
| Varant Titizian | Technical Artist |
| Hassan Khashoggi | Music |
| Darrel Moen | Sound Effects / Music |

---

# Project Status

This repository has been archived to preserve Wash Off as it was completed following its capstone release.

While a post-release update was published shortly after launch, active development has since concluded.

This repository now serves as a historical archive showcasing the work completed by the CrazyBeaverLabs team during the 2021 capstone project.

---

# A Personal Reflection

Looking back on Wash Off, I am incredibly proud of what our team accomplished together, when we first proposed building an online multiplayer game, we knew we were taking on a challenge that was significantly more ambitious than a traditional capstone project. 

We were a team of students working with a limited budget, a fixed ten-month schedule, and varying levels of experience, yet we committed ourselves to building something that had never been attempted within our program before.

Over those ten months, we researched unfamiliar technologies, solved countless technical challenges, learned from one another, and continuously improved both the game and ourselves as developers.

There were certainly moments where the workload felt overwhelming, but the team's willingness to collaborate and support one another allowed us to continue moving forward.

What I am most proud of is not that every system was perfect.

It is that we set an ambitious goal, remained committed to our vision, implemented nearly every major feature we originally planned, and successfully delivered the game on schedule.

That accomplishment belongs to every member of CrazyBeaverLabs.

I want to sincerely thank everyone who contributed to Wash Off. To our programmers, artists, animators, designers, producers, audio contributors, faculty, friends, and everyone who supported us throughout development—thank you.

Every late night, every bug fixed, every asset created, every meeting attended, every review, and every encouraging conversation helped shape the final game into something we could all be proud of.

Although this repository now serves as an archive, I hope it also serves as a reminder of what a passionate and determined team can accomplish when everyone works toward a shared goal.

Thank you all for making this one of the most memorable and rewarding experiences of my development career.


**— Brian Purdy**  
*Technical Director / Lead Developer*  
*CrazyBeaverLabs (2021)*

## Portfolio

More information, screenshots, and project details can be found here:

[Wash Off Capstone - Brian Purdy Portfolio](https://brianpurdy.ca/experience/Washoff-Capstone/)

---

# License

This repository is provided for archival and educational purposes.

All source code, artwork, audio, visual assets, and other project content remain the intellectual property of their respective creators unless otherwise stated.