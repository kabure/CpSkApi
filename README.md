### Cp SK web Api

#### API getRoutes
- return the three closest connections 
##### Example
- http://localhost:5171/api/getRoutes?from=KE&to=BA

| parameter | type    | description                                                                                                                                         |
| --------- | ------- | --------------------------------------------------------------------------------------------------------------------------------------------------- |
| `vehicle` | string  | Vehicle to travel with. Default `vlakbus`. Available: `vlak`, `bus`, `vlakbusmhd`, for public transport, the name of the city. For example "kosice" |
| `time`    | string  | Departure time. Defaults to current time.                                                                                                           |
| `date`    | string  | Departure date. Defaults to current date.                                                                                                           |
| `from`    | string  | Boarding station                                                                                                                                    |
| `to`      | string  | Exit station                                                                                                                                        |
