var yelpcontroller = require("../controllers/YelpController");

exports.getRestaurants = function(req, res) {
  yelpcontroller.getRestaurants(req,res);
}