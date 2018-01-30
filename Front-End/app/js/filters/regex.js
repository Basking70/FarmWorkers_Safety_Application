(function(){
	function regexFilter(){
		var patt = new RegExp(regex);      
	      var out = [];
	      for (var i = 0; i < input.length; i++){
	          if(patt.test(input[i][field]))
	              out.push(input[i]);
	      }      
	    return out;
	}
	angular.module("angularApp.filters").filter('regex',regexFilter);
})();