# Wildcard: Azure Function Gateway

## Proof of Concept Project

I wanted to see if I could use one Azure Function as a gateway to a set of api's, mostly as an experiment but also maybe it could be useful on a consumption plan to save a few pennies.

## Architecture

* Azure Function with a wild card route
* ServiceStack Api that for this example just proxies a couple external api calls

## End Points

* /api/cat/facts -> https://alexwohlbruck.github.io/cat-facts/
* /api/dog/random -> https://dog.ceo/dog-api/
* /api/picsum/{id} -> https://picsum.photos/
* /api/nasa  -> https://api.nasa.gov/

### Base Url

https://sbworld-wildcard.azurewebsites.net/api/{*fullPath}?code=uJw9kzauvZaf0NVOqruuPLkJW8ravEkA9qfHAaf46XHsN56NhmRK1w==

Samples: 

* https://sbworld-wildcard.azurewebsites.net/api/picsum/1?code=uJw9kzauvZaf0NVOqruuPLkJW8ravEkA9qfHAaf46XHsN56NhmRK1w==
* https://sbworld-wildcard.azurewebsites.net/api/cat/facts/?code=uJw9kzauvZaf0NVOqruuPLkJW8ravEkA9qfHAaf46XHsN56NhmRK1w==
* https://sbworld-wildcard.azurewebsites.net/api/dog/random?code=uJw9kzauvZaf0NVOqruuPLkJW8ravEkA9qfHAaf46XHsN56NhmRK1w==
* https://sbworld-wildcard.azurewebsites.net/api/nasa?code=uJw9kzauvZaf0NVOqruuPLkJW8ravEkA9qfHAaf46XHsN56NhmRK1w==


Future Plans:
* Add a sql lite db and a few internal services
* Authentication layer that returns a JWT token
* Authenticated services
* Validations and error handling


Notes:
* You can signup for a free Service Stack Individual License: https://servicestack.net/free
* Pulling from these non authenticated apis: https://github.com/public-apis/public-apis