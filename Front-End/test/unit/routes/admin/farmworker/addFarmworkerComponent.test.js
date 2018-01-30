describe("addFarmworkerComponent test", function() {
    var sandbox;
    var $q;
    var controller;
    var scope;
    var userService;
    beforeEach(module('angularApp'));

    beforeEach(inject(function($rootScope, $componentController, _userService_) {
        sandbox = sinon.sandbox.create();
        scope = $rootScope.$new();
        controller = $componentController('addFarmworker', {
            $scope: scope
        });
        controller.newFarmworker = {};
        userService = _userService_;
        sandbox.spy(userService,"createLoginCredential");
    }));

    afterEach(function() {
        sandbox.restore();
    });

    it("called doAddFamrworker function should called an api to backend", function() {
        controller.doAddFarmworker();
        expect(userService.createLoginCredential.calledOnce).to.be.true
    });
    

});