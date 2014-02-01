var yelp = require("yelp").createClient({
  consumer_key: process.env.YELP_CONSUMER_KEY, 
  consumer_secret: process.env.YELP_CONSUMER_SECRET,
  token: process.env.YELP_TOKEN,
  token_secret: process.env.YELP_TOKEN_SECRET
});

//req format:
//lat: latitude of search area
//lon: longitude of search area
//outputs a random restaurant selected from 20 restaurants in the area
exports.getRestaurants = function(req, res) {
  var lat = req.body.lat;
  var lon = req.body.lon;
  yelp.search({term: "restaurants", ll: lat + "," + lon, limit: "20"}, function(error, data) {
    console.log(error);
    console.log(data);
    var num = Math.ceil(Math.random() * 20);
    res.json(data.businesses[num]);
  });
}
