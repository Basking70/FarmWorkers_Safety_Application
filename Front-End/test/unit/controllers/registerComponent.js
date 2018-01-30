// describe("registerComponent test", function() {
//     var sandbox;
//     var $q;
//     var controller;
//     var scope;
//     beforeEach(module('angularApp'));

//     beforeEach(inject(function($rootScope, $componentController) {
//         sandbox = sinon.sandbox.create();
//         scope = $rootScope.$new();
//         controller = $componentController('register', {
//             $scope: scope
//         });
//         scope.myForm = {
//             passwordInput: {
//                 $setValidity: function() {}
//             },
//             rePasswordInput: {
//                 $setValidity: function() {}
//             }
//         }
//         controller.data = {};
//         sandbox.spy(scope.myForm.passwordInput,"$setValidity");
//         sandbox.spy(scope.myForm.rePasswordInput,"$setValidity");
//     }));

//     afterEach(function() {
//         sandbox.restore();
//     });

//     it("test password min length", function() {
//         controller.data.password = "1234";
//         controller.validatePassword();
//         expect(scope.myForm.passwordInput.$setValidity.calledWith('pattern', false)).to.be.true
//     });
//     it("test password max length", function() {
//         controller.data.password = "123456789012345678901234567";
//         controller.validatePassword();
//         expect(scope.myForm.passwordInput.$setValidity.calledWith('pattern', false)).to.be.true
//     });
//     it("test password no uppercase length", function() {
//         controller.data.password = "123456789_";
//         controller.validatePassword();
//         expect(scope.myForm.passwordInput.$setValidity.calledWith('pattern', false)).to.be.true
//     });
//     it("test password no special char length", function() {
//         controller.data.password = "123456789A";
//         controller.validatePassword();
//         expect(scope.myForm.passwordInput.$setValidity.calledWith('pattern', false)).to.be.true
//     });
//     it("test password valid", function() {
//         controller.data.password = "12345678A_";
//         controller.validatePassword();
//         expect(scope.myForm.passwordInput.$setValidity.calledWith('pattern', true)).to.be.true
//     });

//      it("test confirm password must match with password", function() {
//         controller.data.password = "12345678A_";
//         controller.data.repassword = "12345678A_";
//         controller.validateConfirmPassword();
//         expect(scope.myForm.rePasswordInput.$setValidity.calledWith('value', true)).to.be.true
//     });

// });