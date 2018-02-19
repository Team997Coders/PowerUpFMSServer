# PowerUpFMSServer
This project contains a scoring and control system (a faux FMS if you will) for the 2018 FIRST Power Up game.  Note that it does not connect to the robots like the real FIRST FMS does.  But it does give you a flavor of how scoring will work under real-world field conditions.  Team 997 used it to host a scrimmage in Corvallis, OR for 25 teams.

It has the following features:
- Scores switch and scale ownership in real-time.
- Runs game stages in conjunction with practice mode on the driver station (auto countdown, auto, delay, teleop, and endgame).
- Sets field state for safe and staff-safe conditions.
- Maintains a scoreboard.
- Shows a status page of adruino control modules that control field lights and detect plate state.

It is in 5 parts...three of which are in this repo.
- HTTP Server application to receive commands from all clients using ASPNet Core.
- SPA Scoreboard application to display...the score, using React and ASPNet Core.
- SPA Control panel application to control the state of the field and run the game also using React and ASPNet Core.

This project was developed with VSCode 1.20.1 on Mac OSX 10.12.5 using Dotnet Core 2.0.2.

While I have not tried to deploy this on to a fresh environment (I used my dev machine to run the game), you will at least need to do the following:
- Install dotnet core
- Install npm
- Install webpack

Next, run dotnet restore and dotnet build in each of the project directories.

If the projects build sucessfully, you can run dotnet run for each project.

You will need to search for and replace IP address 192.168.1.54 with the IP address of your machine which will run the server application.  I have not brought this constant out into app settings yet.

Here is a doc that describes how game play works using this scoring system: https://docs.google.com/document/d/1u-EGgbDC8yMkxl0OnWY633JzEyF7c2hVuC16VZJAThY/edit?usp=sharing

To run the scoreboard, we used a Raspberry Pi 3 autoloading Chrome into full screen.  Since we had access to the FIRST A/V setup, we were able to overlay the scoreboard on to live video feeds from the field.  It looked great!
