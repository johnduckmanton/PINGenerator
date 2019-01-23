# PinGenerator - Demo PIN Generator App

## Design Decisions

### What type of persistent storage

 - Used json as easy to do and list of can be externally managed if required

### Fastest way to verify whether the number has been used before

- Array vs List.Contains vs Dictionary vs HashSet

### Don't over engineer the solution

 - No requirements for format, security, encryption or compression of saved data
 - Several 'utility' functions that could be refactored into utility libraries for greater reuse

### Could make it a static class

## Potential issues

If the program is run many times, the list of previously used numbers will grow incrementally larger.
This will lead to degraded performance as it will more attempts to find an unused number. 
