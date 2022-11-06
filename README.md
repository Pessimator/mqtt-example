# mqtt-example

## Task-Description
Task: Simulation of a Temperature Sensor - Implement the following three parts of a software in C#

Producer:
Simulate a temperature sensor which creates a random temperature every 500 milliseconds.
The sensor sends the values via MQTT to an MQTT broker.

Consumer:
Implement a software which receives the temperature values and persists them in an appropriate way (e.g. a file).

User interface:
Implement a user interface to visualize the temperature values as a chart.

Notes:
Ensure that the temperature values are displayed in exactly the same order as they were created by the sensor
Ensure that processing a temperature value on the consumer side is finished before the next one arrives
Ensure that the producer recognizes a failure of the sensor (e.g. the sensor does not send values for 5 seconds)

## Producer

## Consumer/Subscriber

## UserInterface





## Todos
- Think about how to present the parts
- Clean up the example webb app
- dependency injection in subscriber and publisher
- use .net core logger instead of own implementation
- add example unit test
-- to service (mock client/network)
-- to angular app?
- add some kind of logic for understanding the sequenceNumber overflow (or at least calculate approximate runtime when it will occur)
- instead of magic numbers/ports/servernames use parameters of .net core
- currently subscriber:publisher => 1:1, if multiple subscriber some logic will break

