describe('Controller: Delete Form Controller', function () {
    beforeEach(module("appControllers"));

    var deleteController, scope, modelInstance, testName;

    beforeEach(inject(function ($controller) {
        scope = {};
        modelInstance = {};
        testName = "TestName1";
        deleteController = $controller('DeleteFormCtrl', {
            $scope: scope,
            $modalInstance: modelInstance,
            name: testName
        });
    }));

    it('should set the name property of scope to the provided name', function () {
        expect(scope.name).toBe(testName);
    });
})