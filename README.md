# LargeXmlDeserializer

Deseialization on large xml files. Generic C# Class

Example:

      var Flightes = new List<Flight>();
      var deserializerFlight = new LargeXmlDeserializer<Flight>();
      deserializerFlight.Deserialize("Flight", Flightes, @"E:/temp/test.xml");

OR:

      var itinerary = new List<Itinerary>();
      XMLDesetializerUtils.Deserialize<Itinerary>("Itinerary", itinerary, fileName);

OR:

     Multu-Part, Multi-Class deserialization:
     
            var listObjetct = new List<XMLDataObject>(); // Create Object Containers

            var package = new XMLDataObjectItem<Package>("Package"); // Create Store item. Section Package , Class Package
            listObjetct.Add(package);  // add element to store conteiner

            var itineraryDetail = new XMLDataObjectItem<ItineraryDetail>("ItineraryDetail"); // Create additional item
            listObjetct.Add(itineraryDetail);

             // using custom regular expression:
			 // Create store item with specific regular expression
             
             var bc = new XMLDataObjectItem<BClient>("BClient","(?<data><(?<pair>BClient)[ ]*>.+?</BClient[ ]*></BClient[ ]*>)");
             listObjetct.Add(bc);

             XMLDesetializerContainer.Deserialize(listObjetct, fileName);  // Parse  