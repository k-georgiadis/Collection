A small application for inserting maps to the Tiberian Sun Client (https://github.com/CnCNet/xna-cncnet-client).

Currently, the only way to add maps to Tiberian Sun Client is create an image preview of the map, add it to the desired map folder together with the map file itself and then insert the proper records in the "MPMaps.ini" file.

As you can see the process is quite convoluted, so I decided to make this application to easily add maps and create their previews at the same time. TS Client has trouble with aligning the spawn markers in the correct position so if the map size is not symmetrical, the spawn markers will not be positioned correctly. 

The applicaction is not heavily tested, so if it crashes during execution, you can take a look at the code and figure out why and where the crash occurs :)

Some QoL changes can be done so when I decide to come back to it, I'll see what I can do.

UPDATE: The latest versions of TS Client import custom maps automatically without adding them to the "MPMaps.ini" file.
They appear in the "Custom Map" category inside the map selection screen.
This renders my application useless since there is no need for it. Nevertheless, I will keep it here.
Who knows, maybe in the future there will be need of it again.

-- Kosmas Georgiadis aka CosmaOne
