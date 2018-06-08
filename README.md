# RPi.SenseHat

Forked from [@emmellsoft's](https://github.com/emmellsoft) RPi.SenseHat project to provide support for Mono/.NET Core runtimes. 

A complete Mono/.NET Core class library for the Raspberry Pi "Sense HAT" (C#)

The solution contains the following projects:
*) Rpi.SenseHat
*) RPi.SenseHat.Tools
*) RT.IoT.Sensors

The Rpi.SenseHat is the main library. It contains a nice API to the Raspberry Sense HAT in C#.

The RPi.SenseHat.Tools is a regular Windows console application that was used to test out some of the calculations that was needed in the actual library.
It also contains the process of converting a bitmap holding a font image into a "compiled" byte array that can be used by the font classes of the Sense HAT library.

************************
Regarding thread safety:

The SenseHatFactory.Singleton.GetSenseHat() call is thread-safe, but the rest of the API is not.

It's deliberately not thread-safe to maximize performance, so you should avoid calling (for instance) the Update method on the sensors simultaneously from different threads (but you *may* call it from any thread).