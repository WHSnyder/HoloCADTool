# HoloCADTool
<<<<<<< HEAD
Basic proportional vertex-pulling tool made w/Unity for Hololens 1
=======
Basic proportional vertex-pulling tool made w/Unity for Hololens 1<br><br><br>

Has:<br><br>
-basic mesh manipulation via Blender linear-falloff proportional editing <br>
-various features (bouncing ball, voice commands) implemented to test HoloLens features<br>
-very inefficient mesh processing routines<br><br><br><br>

In Progress:<br><br>
-mesh processing upgrade via Unity's relatively unknwon DCEL class (hidden away in some animation library) <br>
-more UI knicknacks<br>
-ability to turn app off by presenting middle finger to HoloLens view.  Since Microsoft MR doesnt allow loopback<br>
requests on HoloLens, implementing this is pretty circuitous.  The server files in Python require flask and OpenCV
and must be run on a separate machine.  The py server will recieve a constant stream of depth sensor data 
from the HoloLens (via the Research Mode API -- also pretty unknown) and will determine the presence of a middle finger via 
convexivity defect analysis.  I doubt this will be very performant, but it's a fun experiment nonetheless.
>>>>>>> f24cd2564c24c787efec644d63096f0b1e952b18
