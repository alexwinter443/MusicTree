# This is the Repository for Alex's MusicTree project.

## MusicTree is a Digital Music Streaming Service that gives users access to songs from artists across the world

### *Musicians and producers often have a hard time getting their music heard. I myself find it hard to get any views on my music and platforms such as youtube or soundcloud are oversaturated. For my Capstone I created an app where a community of musicians can showcase their work and gain more attention.*  

# Home Page
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/b1c7e1cb-9d74-4f04-9093-2c8959194326)

# Track page
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/25944a78-6d13-42ef-9bb6-cebbe4f46a14)

# Manage Tracks
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/08ece2d1-7631-4996-9a90-e9213d760b33)

# Edit Instrumental
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/35180314-5afc-4ca5-bc59-a798b6de131e)

# Create Instrumental
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/06c260e7-054f-48ac-b47c-5dc741e825af)



# Requirements
## Functional Requirement for MusicTree:

### 1. Users can login and register

### 2. Users can add tracks.

### 3. Users can stream music on phone or pc

### 4. Musicians can edit or delete tracks

### 5. Users can like or comment on tracks

## Non-functional Requirements for MusicTree

### user friendly and modern design

### fast response rate

# BeatsTree is built using C#, ASP.NET, BOOTSTRAP and AZURE

> I wanted to create a functional app before I used React for my front-end. React will be used after I finished the backend environment.

## Object Oriented programming (OOP) and MVC architecture was used to create a balanced environment.

## MusicTree was deployed to Azure.

# Project Diagrams

## High Level Design Diagram
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/e909c7cb-b344-4b99-aac0-ed424ababd7b)

### The Front end of Music Tree is built using Custom CSS and Bootstrap.

### The Backend uses C# and the ASP.NET framework.

### MusicTree is hosted on Azure (edit: no longer on azure due to cost)

## Logical Diagram

![image](https://github.com/alexwinter443/MusicTree/assets/59127575/8f9c37a8-5f46-4d17-9b0c-6ed819abdfbe)

### •	This is the physical solution design of our application. This depicts the flow of storing and distributing mp3 files for users.
### •	Media source files are delivered using MPEG-DASH. (essentially a streaming protocol that breaks down data into chunks at different quality levels).
### •	Dynamic packaging reduces storage costs and space.
### •	Streaming Endpoint as origin service: Directly delivers content to client player.
### •	Dynamic asset filtering and modification without having to re-encode or re-render content.
### •	Keys and tokens for secure content delivery.


## UML Diagram

![image](https://github.com/alexwinter443/MusicTree/assets/59127575/cd71a166-25b3-4e49-9ca6-8f2c844d622d)

### Since I am utilizing ASP.NET I am also adapting to the MVC framework. 
### We have a total of 7 expected models that we will utilize to have a functional web application. 
### The cornerstone of our project will be the UserModel and AudioFile since they need to be passed to the controller and DAO.
### Interfaces will also be utilized so we can reduce redundant code and allow development to proceed without hinderance.

## What I learned

### Gained a complex understanding of the C# programmning language.
### Cloud Development: Dived into cloud development researching different cloud providers and implementing cloud services in app.
### Uploading and streaming music: learned azure
### Project Management: gained experience in managing products under tight time constraints.

## Risks and Challenges

### Cloud Service Costs: need to monitor spending weekly

### Time Constraints: given a semester to develop this application

### User Experience and design: not much time given to use a frontend framework

## Outstanding Issues

### The only issue I came across was cost on azure services. It became to expensive to keep it hosted so I had to take it down.












