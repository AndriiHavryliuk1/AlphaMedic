
angular.module('alphaMedicApp')

.factory('FilterFactory',  function() {
  // private variable
  var _FilterId = {};

  // public API
  return {
    FilterId: _FilterId
  };
})
