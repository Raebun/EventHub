# EventHub

The app is designed to assist users in organizing, planning, attending, and managing events. This includes catering to the needs of both individuals who regularly attend events and organizers who plan and manage events. The app provides a user-friendly interface where users can register, log in, and manage their profile information.

Key functionalities include:

-	User management: Registration, login, and profile editing.
-	Events overview: A list of all available events with the ability to filter by date and location.
-	Event details: Detailed information about specific events, including date, location, ticket prices, and other relevant details.
-	Ticket functionality: Users can purchase tickets for events and view and manage their purchased tickets.
-	Organizational features: Organizers can add, edit, and delete events.
-	Favorites: Users can save favorite events for easy access and follow-up.
-	Reviews and ratings: Users can leave reviews and ratings for events they have attended.
-	
The design of the app will ensure that these functionalities are accessible to both users and organizers. This includes a clear user interface, easy navigation, and clear action buttons. The backend will be designed to store and process the required data, such as user information, event details, ticket sales, and reviews. Attention will also be paid to authentication to ensure a smooth user experience.


This repository contains the following projects:

- Api
- Data
- Shared
- Webclient (Main App)
- Webclient (Organizer App)
- Unit Tests

## Table of Contents
- [Getting Started](#getting-started)
- [Project Information](#project-information)

## Getting Started

To get started with this project, follow these steps:

**1. Clone the repository**: 
   ```bash
   git clone https://github.com/Raebun/EventHub.git
   ```
**2. For Visual Studio Users**

Make sure your VS is up-to-date and have these workloads installed in the installer:
- ASP.NET and web development
- .NET Multi-platform App UI development
- .NET desktop development

**3. Emulator**

Make sure you have a working emulator on your IDE.

**4. Environment SQL Infrastructure**

Make sure you have an environment ready to manage SQL in. 
An example of this: SQL Server Management Studio (SSMS).

**5. Get Database**

IDE:
1. Make sure there are no errors in your repository.
2. Make sure package are up-to-date of the Entity Framework.
3. Open the Package Manager Console (PMC).
4. Put API as startup project.
5. In the PMC, select Data as the default project.

## Project Information
**Api**

The Api project contains an ASP.NET Core WebApi. It serves as the backend for the EventHub application, handling all HTTP requests and providing data to the front-end applications. It includes controllers and services to manage events, users, and other core functionalities.

**Data**

The Data project is a C# Library Class and includes only the migrations and data context for the database. It is set up using Entity Framework with a code-first approach.

**Shared**

The Shared project is a C# Library Class that contains shared models and entities used across multiple projects within the solution. These entities are essential for Entity Framework operations, and the shared models promote code reuse and consistency across the application.

**Webclient (Main App)**

The Webclient (Main App) project is the primary front-end application for EventHub, targeting Android and Windows platforms. It is built using .NET Multi-platform App UI (MAUI) and provides the main user interface for end-users to interact with the system. This app includes features for browsing events, user registration, and other core functionalities.

**Webclient (Organizer App)**

The Webclient (Organizer App) project is a Blazor application designed for event organizers. It provides tools and interfaces for organizers to create, manage, and track events.

**Unit Tests**

The Unit Tests project contains automated tests for the solution, focusing primarily on viewmodel tests from the main app. These tests ensure the correctness of the code and help maintain the reliability and stability of the application.
