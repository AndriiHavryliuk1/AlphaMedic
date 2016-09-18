angular.module('alphaMedicApp').
service("PaginationArrayService", function() {
    this.Array = function(pages, selectedPage) {
      selectedPage = parseInt(selectedPage);
        var skipPage = 2;
        var array = new Array();
        for (var i = 0; i < pages; i++) {
            array.push(i + 1);
        }

        if (((selectedPage - skipPage) > 0) && (selectedPage + skipPage) <= array[array.length - 1]) {
            return array.slice(selectedPage - skipPage - 1, selectedPage + skipPage);
        }

        if (selectedPage == 1 || selectedPage == 2){
            return array.slice(0, 5);
        }

        if (selectedPage + skipPage > array[array.length - 1]) {
            return array.slice(array[array.length - 6] , array[array.length - 1]);
        }

        return null;
    };

});
