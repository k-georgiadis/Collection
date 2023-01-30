A simple simulation of stellar objects and their Newtonian gravitational interactions.
The original name for this kind of simulation is called "N-body simulation".

This is a big project that took me a while to bring to a reasonable working level.
Currently, there are only two (2) stellar objects available: Planets and Stars.

Trivia:
	Because this project started out as a simple graphics showcase, the planets are drawn as squares (or blocks as I call them).
	The universe is a 2D image with a black background where stellar objects are drawn on it.
	I used to call it the "Block Universe", since it originally contained only "blocks", but now I call it "Cosmos".

Structure:
	The Cosmos is divided into nine (9) sections.
	These sections have no interaction with one another.
	This means that stellar objects from one section cannot interact with objects from another.
	The user can view only one (1) section at any given time. To choose/change a section, the user can use the Minimap UI to do so.

Simulation:
	The user can choose the type of the simulation, be it realistic or not.
	By default the simulation is not realistic, meaning that the interactions between the stellar objects are toned-up or toned-down.
	This ensures that the user doesn't need to wait decades for a planet to move a few pixels.
	Also, for obvious reasons, the difference in size between stars and planets is such so that the user can see and select them.
	
Movement:
	The Cosmos can be zoomed in/out and moved around on its X, Y axis. 
	To zoom in/out, use mouse scrolling but make sure you are not focused on a textbox or any other control.
	To move the camera, just click on any point on the Cosmos and drag your mouse around (panning).
	Also, at the bottom center of the window, there is a Minimap that shows the whole section and all of it's objects.
	Clicking/panning on the Minimap will move the camera to that corresponding location in the current Cosmos section.

User interaction:
	Currently, there are only two (2) stellar objects available: Planets and Stars.
	The user can create, at will, any number of objects of any type, just by clicking inside the Cosmos.
	On the left side of the window, there are some basic settings for initializing newly created objects.
	These settings include: Initial Velocity, Z Position and Mass.
	Clicking on an object inside the Cosmos, makes the object selected (a white border appears around the object).
	Selected objects can be followed, when the corresponding setting is set in the right side of the window.
	When hovering the mouse cursor over an object, a tooltip will appear containing some basic information about the object.
	The tooltip shows by default on selected objects.

Stellar Object interaction:
	The stellar objects interact with one another by applying Newton's Gravitational Law.
	They can merge with each other, creating bigger and heavier objects.
	On the left side of the window, there are settings for selecting the type of "wall" interaction/collision.
	"Walls" are considered to be the boundaries of the visible image.
	When a stellar objects hits a wall the user can choose the type of interaction that will occur (Tunnel, Bounce, None).
	Objects can traverse in the Z direction as well, meaning they can be behind or infront of other objects.
	The farther away in the Z direction (Z < 0) the object goes, the smaller it becomes. Bigger, when the opposite happens (Z > 0).
	By default all stellar objects are created with Z = 0. The user can changed that setting as mentioned above.

Information:
	Each stellar object has it's basic fundamental properties recorded and calculated in real-time. 
	These properties include: Location, Acceleration, Velocity, Size/Radius and Mass.
	On the bottom left side of the window, there is a list containing all the current stellar objects in the Cosmos along with their information.

Trajectories:
	Planets can have their trajectories drawn but only up to a specific number of points.
	This is due to the way trajectories are drawn. Basically, it's a list of points that are connected with each other, thus forming a path (trajectory).
	The more points the user chooses, the longer the trajectory gets but also more "laggy" the simulation will get, depending on the number of the visible stellar objects.
	Also, Relative trajectories can also be drawn. Relative trajectories can only be drawn when an object is being followed.
	
States:
	The user can choose to save "snapshots" of the Cosmos at any given time, using the "Save" button in the Action menu, on the top side of the window.
	These snapshots include all the objects for all the sections in the Cosmos along with their current information (position, velocity, etc.).
	Later, the user can choose to load any saved snapshot, replacing the current Cosmos, with the one loaded.

This project is a work in progress, as are most of the projects in this repo.
Most of the bugs have been fixed. There are only a couple of bugs that are not top priority right now, but they are certainly in the TODO list.

It took me a while to implement the basic functionality, which is drawing the objects and having them interact in a realistic way.
But I was extremely overjoyed when I managed to pull of the zoom in/out and move functionalities.
It uses a lot of math (I hate linear algebra), plus the concept of handling matrices isn't fun or easy to begin with and if you don't understand what it needs to done, you will never get it.
That is exactly why I was ecstatic, to say the least, when I pulled it off.

I also think that I could do more, if I knew more about matrix transformations and other clever manipulations that can be done to achieve some cool effects.
I'm pretty sure most of the calculations that I do with matrices (especially for the Minimap coordinates etc.) can be done in one or two lines of code, using the correct matrix manipulations.
But for now, that's all I can do.

Enjoy xD
