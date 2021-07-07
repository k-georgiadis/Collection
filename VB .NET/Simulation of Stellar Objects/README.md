A simple simulation of stellar objects and their in-between gravity force interactions.

This is a big project that took me a while to bring to a reasonable working level.
Currently, there are only two (2) stellar objects available: planets and stars.

Because this project started out as a simple graphics showcase, the planets are drawn as squares (or blocks as I call them).
The universe is a 2D image with a black background where you can "draw" stellar objects on it. I call it the "Block Universe" since it originally contained only "blocks".
The universe can be zoomed in/out and moved around on its X, Y axis. To zoom in/out use mouse scrolling (make sure you are not focused on a textbox or anything but the universe)
and to move the universe just click on any point on the universe/image and drag your mouse around.

Each stellar object has it's basic fundamental properties recorded and calculated in real-time. These properties include: Location, Acceleration, Velocity, Size, Weight etc...
On the right side of the window, along with some basic settings, there is an object list containing all the current objects in the universe along with their location.
Clicking on an object inside the universe, enables the user to track it (white border around the object) and view its current property values (bottom of the window).
The properties are calculated in a realistic way, meaning they follow their corresponding real world physics equations, but for obvious reasons some values are toned down or up.
This is necessary if we want to avoid a star instantly pulling all stellar objects around it or the opposite, which is taking forever for the star to pull the objects towards it.

One example is that during the calculation of gravity forces, velocities, accelerations etc. the distance moved is toned up to make the object visibly move.
Same logic applies when creating the stars and their perspective sizes and weights. A normal star, although in real life is thousands of times bigger than a planet, in our simulation it barely
is 3-4 times bigger.

This project is a work in progress, as are most of the projects in this repo. There are many bugs which I identified and plan on fixing.
It took me a while to implement the basic functionality which is drawing the objects and having them interact in a realistic way.
But I was extremely overjoyed when I managed to pull of the zoom in/out and move functionalities. It uses a lot of math (I hate linear algebra), plus the concept of handling matrixes
isn't fun or easy to begin with and if you don't understand what it needs to done, you will never get it. That is exaclty why I was ecstatic, to say the least, when I pulled it off.
