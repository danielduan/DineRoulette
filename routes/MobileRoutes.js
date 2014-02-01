var yelpcontroller = require("../controllers/YelpControllers.js");

exports.getRestaurants = function(req, res) {
  yelpcontroller.getRestaurants(req,res);
}