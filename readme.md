# This is the Repository for Alex's MusicTree project.

## MusicTree is a Digital Music Streaming Service that gives users access to songs from artists across the world

### *Musicians and producers often have a hard time getting their music heard. I myself find it hard to get any views on my music and platforms such as youtube or soundcloud are oversaturated. For my Capstone I created an app where a community of musicians can showcase their work and gain more attention.*  


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

## MusicTree was deployed to Azure but had to be taken down for because it was costing to much.

# Project Diagrams

## High Level Design Diagram
![image](https://github.com/alexwinter443/MusicTree/assets/59127575/e909c7cb-b344-4b99-aac0-ed424ababd7b)

### The Front end of Music Tree is built using Custom CSS and Bootstrap.

### The Backend uses C# and the ASP.NET framework.

### MusicTree is hosted on Azure (edit: no longer on azure due to cost)

## Logical Diagram

![image](https://github.com/alexwinter443/MusicTree/assets/59127575/8f9c37a8-5f46-4d17-9b0c-6ed819abdfbe)

### •	This is the physical solution design of our application. As we can see, we have several processes that depict the flow of storing and distributing mp3 files for users 
### •	We deliver our media source files to users using MPEG-DASH. This is essentially a streaming protocol that breaks down data into chunks at different quality levels. 
### •	From here I implemented a front-end for our user to utilize the cloud streaming service. 
### •	Likewise, we use dynamic packaging to reduce the amount of copies packaged in blob storage. This will help alleviate storage costs and reduce amount of audio content stored.
### •	In media services a Streaming Endpoint is used as the origin service that can deliver content directly to a client player. It is a regular method used for streaming content.
### •	An Advantage to this is that we can dynamically filter and modify assets without having to re-encode o re-render content.
### •	Keys and tokens will also be utilized so we can deliver content to the appropriate user with appropriate permissions.












